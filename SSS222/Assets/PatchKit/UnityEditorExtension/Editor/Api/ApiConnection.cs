using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using PatchKit.Logging;
using PatchKit.Network;

namespace PatchKit.Api
{
    /// <summary>
    /// Base Api Connection.
    /// </summary>
    public class ApiConnection
    {
        private enum RequestMethod
        {
            Get,
            Post
        }

        private struct Request
        {
            public string Path;

            public string Query;

            public RequestMethod Method;

            public string Body;

            public List<Exception> MainServerExceptions;

            public List<Exception> CacheServersExceptions;
        }

        private readonly ApiConnectionSettings _connectionSettings;

        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public IRequestTimeoutCalculator RequestTimeoutCalculator = new SimpleRequestTimeoutCalculator();

        public IRequestRetryStrategy RequestRetryStrategy = new NoneRequestRetryStrategy();

        public IHttpClient HttpClient = new DefaultHttpClient();

        public ILogger Logger = DummyLogger.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiConnection"/> class.
        /// </summary>
        /// <param name="connectionSettings">The connection settings.</param>
        /// <exception cref="System.ArgumentNullException">
        /// connectionSettings - <see cref="ApiConnectionServer.Host"/> of one of the servers is null.
        /// or
        /// connectionSettings - <see cref="ApiConnectionServer.Host"/> of one of the servers is empty.
        /// </exception>
        public ApiConnection(ApiConnectionSettings connectionSettings)
        {
            ThrowIfServerIsInvalid(connectionSettings.MainServer);
            if (connectionSettings.CacheServers != null)
            {
                foreach (var cacheServer in connectionSettings.CacheServers)
                {
                    ThrowIfServerIsInvalid(cacheServer);
                }
            }

            _connectionSettings = connectionSettings;
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ThrowIfServerIsInvalid(ApiConnectionServer server)
        {
            if (server.Host == null)
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("connectionSettings",
                    "ApiConnectionServer.Host of one of the servers is null.");
            }

            if (string.IsNullOrEmpty(server.Host))
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("connectionSettings",
                    "ApiConnectionServer.Host of one of the servers is empty");
            }
        }

        /// <summary>
        /// Parses the response data to structure.
        /// </summary>
        protected T ParseResponse<T>(IApiResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Body, _jsonSerializerSettings);
        }

        private bool TrySendRequest(ApiConnectionServer server, Request request, ServerType serverType,
            out IApiResponse response)
        {
            Logger.LogDebug(
                string.Format(
                    "Trying to get response from server ({0}): '{1}:{2}' (uses HTTPS: {3})...",
                    serverType,
                    server.Host,
                    server.RealPort,
                    server.UseHttps));

            response = null;

            List<Exception> exceptionsList;

            switch (serverType)
            {
                case ServerType.MainServer:
                    exceptionsList = request.MainServerExceptions;
                    break;
                case ServerType.CacheServer:
                    exceptionsList = request.CacheServersExceptions;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(serverType.ToString(), serverType, null);
            }

            try
            {
                var uri = new UriBuilder
                {
                    Scheme = server.UseHttps ? "https" : "http",
                    Host = server.Host,
                    Path = request.Path,
                    Query = request.Query,
                    Port = server.RealPort
                }.Uri;

                var httpResponse = MakeResponse(uri, request);

                Logger.LogDebug("Received response. Checking whether it is valid...");
                Logger.LogTrace(
                    string.Format(
                        "Response status code: {0}",
                        httpResponse.StatusCode));

                if (IsResponseValid(httpResponse, serverType))
                {
                    Logger.LogDebug("Response is valid.");
                    response = new ApiResponse(httpResponse);
                    return true;
                }

                Logger.LogWarning("Response is not valid.");

                if (IsResponseUnexpectedError(httpResponse, serverType))
                {
                    throw new ApiResponseException((int) httpResponse.StatusCode);
                }

                throw new ApiServerConnectionException(
                    string.Format(
                        "Server \'{0}\' returned code {1}",
                        server.Host,
                        (int) httpResponse.StatusCode));
            }
            catch (WebException webException)
            {
                Logger.LogWarning("Error while connecting to the API server.", webException);
                exceptionsList.Add(webException);
                return false;
            }
            catch (ApiServerConnectionException e)
            {
                Logger.LogWarning("Error while connecting to the API server.", e);
                exceptionsList.Add(e);
                return false;
            }
        }

        private IHttpResponse MakeResponse(Uri uri, Request request)
        {
            switch (request.Method)
            {
                case RequestMethod.Get:
                {
                    var httpRequest = new HttpGetRequest
                    {
                        Address = uri,
                        Timeout = RequestTimeoutCalculator.Timeout
                    };

                    Logger.LogTrace(
                        string.Format("timeout = {0}ms", httpRequest.Timeout));

                    return HttpClient.Get(httpRequest);
                }
                case RequestMethod.Post:
                {
                    var httpRequest = new HttpPostRequest
                    {
                        Address = uri,
                        Timeout = RequestTimeoutCalculator.Timeout,
                        FormData = request.Body
                    };

                    Logger.LogTrace(
                        string.Format("timeout = {0}ms", httpRequest.Timeout));

                    return HttpClient.Post(httpRequest);
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsResponseValid(IHttpResponse httpResponse, ServerType serverType)
        {
            switch (serverType)
            {
                case ServerType.MainServer:
                    return IsStatusCodeOk(httpResponse.StatusCode);
                case ServerType.CacheServer:
                    return httpResponse.StatusCode == HttpStatusCode.OK;
                default:
                    throw new ArgumentOutOfRangeException(serverType.ToString(), serverType, null);
            }
        }

        private bool IsResponseUnexpectedError(IHttpResponse httpResponse, ServerType serverType)
        {
            switch (serverType)
            {
                case ServerType.MainServer:
                    return !IsStatusCodeOk(httpResponse.StatusCode) &&
                           !IsStatusCodeServerError(httpResponse.StatusCode);
                case ServerType.CacheServer:
                    return false; // ignore any api cache error
                default:
                    throw new ArgumentOutOfRangeException(serverType.ToString(), serverType, null);
            }
        }

        private bool IsStatusCodeOk(HttpStatusCode statusCode)
        {
            return IsWithin((int) statusCode, 200, 299);
        }

        private bool IsStatusCodeServerError(HttpStatusCode statusCode)
        {
            return IsWithin((int) statusCode, 500, 599);
        }

        private bool IsWithin(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public IApiResponse Get(string path, string query)
        {
            return SendRequest(() => {
                Logger.LogDebug(
                    string.Format(
                        "Getting response for GET request with path: '{0}' and query: '{1}'...",
                        path,
                        query));

                return new Request
                {
                    Path = path,
                    Query = query,
                    MainServerExceptions = new List<Exception>(),
                    CacheServersExceptions = new List<Exception>()
                };
            });
        }

        public IApiResponse Post(string path, string query, string body)
        {
            return SendRequest(() => {
                Logger.LogDebug(
                    string.Format(
                        "Getting response for POST request with path: '{0}', query: '{1}' and data '{2}'...",
                        path,
                        query,
                        body));

                return new Request
                {
                    Path = path,
                    Query = query,
                    Method = RequestMethod.Post,
                    Body = body,
                    MainServerExceptions = new List<Exception>(),
                    CacheServersExceptions = new List<Exception>()
                };
            });
        }

        private IApiResponse SendRequest(Func<Request> requestBuilder)
        {
            try
            {
                var request = requestBuilder();
                
                IApiResponse apiResponse;

                bool retry;

                do
                {
                    if (!TrySendRequest(_connectionSettings.MainServer, request, ServerType.MainServer,
                        out apiResponse))
                    {
                        if (_connectionSettings.CacheServers != null &&
                            request.Method == RequestMethod.Get)
                        {
                            foreach (var cacheServer in _connectionSettings.CacheServers)
                            {
                                if (TrySendRequest(cacheServer, request, ServerType.CacheServer, out apiResponse))
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (apiResponse == null)
                    {
                        Logger.LogWarning(
                            "Connection attempt to every server has failed. Checking whether retry is possible...");
                        RequestTimeoutCalculator.OnRequestFailure();
                        RequestRetryStrategy.OnRequestFailure();

                        retry = RequestRetryStrategy.ShouldRetry;

                        if (!retry)
                        {
                            Logger.LogError("Retry is not possible.");
                            throw new ApiConnectionException(request.MainServerExceptions,
                                request.CacheServersExceptions);
                        }

                        Logger.LogDebug(
                            string.Format(
                                "Retry is possible. Waiting {0}ms before next attempt...",
                                RequestRetryStrategy.DelayBeforeNextTry));

                        Thread.Sleep(RequestRetryStrategy.DelayBeforeNextTry);

                        Logger.LogDebug("Trying to get response from servers once again...");
                    }
                    else
                    {
                        retry = false;
                    }
                } while (retry);

                Logger.LogDebug("Successfully got response.");
                Logger.LogTrace(
                    string.Format("Response body: {0}", apiResponse.Body));

                RequestTimeoutCalculator.OnRequestSuccess();
                RequestRetryStrategy.OnRequestSuccess();

                return apiResponse;
            }
            catch (Exception e)
            {
                Logger.LogError("Failed to get response.", e);
                throw;
            }
        }

        private enum ServerType
        {
            MainServer,
            CacheServer
        }
    }
}
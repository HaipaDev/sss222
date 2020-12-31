using System;
using System.Collections.Generic;

namespace PatchKit.Api
{
    /// <summary>
    /// Occurs when there are problems with connection to API.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ApiConnectionException : Exception
    {
        private readonly IEnumerable<Exception> _mainServerExceptions;
        private readonly IEnumerable<Exception> _cacheServersExceptions;

        /// <inheritdoc />
        public ApiConnectionException(IEnumerable<Exception> mainServerExceptions,
            IEnumerable<Exception> cacheServersExceptions) : base("Unable to connect to any of the API servers.")
        {
            _mainServerExceptions = mainServerExceptions;
            _cacheServersExceptions = cacheServersExceptions;
        }

        /// <summary>
        /// Exceptions that occured during attempts to connect to main server.
        /// </summary>
        public IEnumerable<Exception> MainServerExceptions
        {
            get { return _mainServerExceptions; }
        }

        /// <summary>
        /// Exceptions that occured during attempts to connect to cache servers.
        /// </summary>
        public IEnumerable<Exception> CacheServersExceptions
        {
            get { return _cacheServersExceptions; }
        }

        /// <inheritdoc />
        public override string Message
        {
            get
            {
                var t = base.Message;

                t += "\n" +
                     "Main server exceptions:\n" +
                     ExceptionsToString(MainServerExceptions) +
                     "Cache servers exceptions:\n" +
                     ExceptionsToString(CacheServersExceptions);

                return t;
            }
        }

        private static string ExceptionsToString(IEnumerable<Exception> exceptions)
        {
            var result = string.Empty;

            int i = 1;
            foreach (var t in exceptions)
            {
                result += string.Format("{0}. {1}\n", i, t);
                i++;
            }

            if (i == 1)
            {
                result = "(none)";
            }

            return result;
        }
    }
}
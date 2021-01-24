using System;

namespace PatchKit.Api
{
    /// <summary>
    /// Occurs when there are problems with API response.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ApiResponseException : Exception
    {
        private readonly int _statusCode;

        /// <summary>
        /// API status code.
        /// </summary>
        public int StatusCode
        {
            get { return _statusCode; }
        }

        /// <inheritdoc />
        public ApiResponseException(int statusCode) : base(
            string.Format(
                "API server returned invalid status code {0}",
                statusCode))
        {
            _statusCode = statusCode;
        }
    }
}
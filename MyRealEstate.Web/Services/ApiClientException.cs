using System.Net;

namespace MyRealEstate.Web.Services
{
    /// <summary>
    /// Thrown by typed API clients when the backend returns a non-success status.
    /// Carries the raw HTTP status code for friendly error mapping in controllers.
    /// </summary>
    public class ApiClientException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiClientException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
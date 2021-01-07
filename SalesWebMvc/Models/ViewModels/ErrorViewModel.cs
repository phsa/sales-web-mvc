using Microsoft.AspNetCore.Http;

namespace SalesWebMvc.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string StatusCodeTitle
        {
            get
            {
                switch (StatusCode)
                {
                    case StatusCodes.Status404NotFound:
                        return "Not Found";
                    case StatusCodes.Status400BadRequest:
                        return "Bad Request";
                    case StatusCodes.Status503ServiceUnavailable:
                        return "Service Unavailable";
                    default:
                        return "";
                }
            }
        }
    }
}
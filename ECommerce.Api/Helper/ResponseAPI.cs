namespace ECommerce.Api.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string message = null)
        {
            Message = message ?? GetMessageFromStatusCode(statusCode);
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }

        private string GetMessageFromStatusCode(int statusCode)
        {
            return statusCode switch
            {
                200 => "OK",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => "Unknown Status"
            };
        }
    }
}

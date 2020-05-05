namespace UntisExportService.Outputs
{
    public class HttpResponse
    {
        public bool IsSuccess { get; private set; }

        public int StatusCode { get; private set; }

        public string Content { get; private set; }

        public string Response { get; private set; }

        public HttpResponse(bool isSuccess, int statusCode, string content, string response)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Content = content;
            Response = response;
        }
    }
}

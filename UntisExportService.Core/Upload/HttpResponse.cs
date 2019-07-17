namespace UntisExportService.Core.Upload
{
    public class HttpResponse
    {
        public bool IsSuccess { get; private set; }

        public int StatusCode { get; private set; }

        public string Content { get; private set; }

        public HttpResponse(bool isSuccess, int statusCode, string content)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Content = content;
        }
    }
}

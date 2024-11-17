namespace real_estate_api.ErrorHandling
{
    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }
        public string TraceId { get; set; }
    }
}

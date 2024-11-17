namespace real_estate_api.ErrorHandling
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message = "Invalid request") : base(message) { }
    }
}

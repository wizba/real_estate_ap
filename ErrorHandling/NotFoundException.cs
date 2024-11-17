namespace real_estate_api.ErrorHandling
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Resource not found Test") : base(message) { }
    }
}

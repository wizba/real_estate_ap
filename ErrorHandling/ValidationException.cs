namespace real_estate_api.ErrorHandling
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
        {
            Errors = errors;
        }
    }
}

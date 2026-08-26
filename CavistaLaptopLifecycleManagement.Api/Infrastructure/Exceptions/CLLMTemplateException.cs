namespace CavistaLaptopLifecycleManagement.Api.Infrastructure.Exceptions
{
    public abstract class CLLMTemplateException(
    string message,
    int statusCode
    ) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }
}

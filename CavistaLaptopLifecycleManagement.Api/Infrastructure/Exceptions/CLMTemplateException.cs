namespace CavistaLaptopLifecycleManagement.Api.Infrastructure.Exceptions
{
    public abstract class CLMTemplateException(
    string message,
    int statusCode
    ) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }
}

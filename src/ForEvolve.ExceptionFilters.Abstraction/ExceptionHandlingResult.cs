namespace ForEvolve.ExceptionFilters
{
    public class ExceptionHandlingResult
    {
        public ExceptionHandlingResult(bool exceptionHandled)
        {
            ExceptionHandled = exceptionHandled;
        }

        public bool ExceptionHandled { get; }
    }


    //public abstract class ExceptionSerializerHandlerBase : IExceptionHandler
    //{
    //    public const int DefaultSerializerOrder = 100;
    //    public virtual int Order => DefaultSerializerOrder;

    //    public abstract Task HandleAsync(HttpContext httpContext, Exception exception);

    //    public virtual Task<bool> KnowHowToHandleAsync(Exception exception)
    //        => Task.FromResult(true);
    //}

    //public class JsonExceptionSerializerHandler : ExceptionSerializerHandlerBase
    //{
    //    public override Task HandleAsync(HttpContext httpContext, Exception exception)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
    //public class ExceptionHandlingOptions
    //{
    //    public bool SerializeExceptions { get; set; }
    //}
}
namespace ForEvolve.ExceptionMapper
{
    public abstract class ExceptionHandler<TException> : IExceptionHandler
        where TException : Exception
    {
        public abstract int StatusCode { get; }
        public virtual int Order => HandlerOrder.DefaultOrder;

        public virtual Task<bool> KnowHowToHandleAsync(Exception exception)
        {
            return Task.FromResult(exception is TException);
        }

        public virtual Task ExecuteAsync(ExceptionHandlingContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            context.Result = new ExceptionHandledResult(context.Error);
            return ExecuteCoreAsync(new ExceptionHandlingContext<TException>(context));
        }

        protected virtual Task ExecuteCoreAsync(ExceptionHandlingContext<TException> context)
        {
            return Task.CompletedTask;
        }
    }
}
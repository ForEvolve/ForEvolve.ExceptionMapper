namespace ForEvolve.ExceptionMapper.FluentMapper;

public class FluentExceptionHandler<TException> : IExceptionHandler
        where TException : Exception
{
    public int Order { get; set; } = HandlerOrder.DefaultOrder;
    private Func<ExceptionHandlingContext, Task>? _handler;

    public Task ExecuteAsync(ExceptionHandlingContext context)
    {
        if (_handler == null)
        {
            return Task.CompletedTask;
        }
        var task = _handler?.Invoke(context);
        if (!context.Result.ExceptionHandled)
        {
            context.Result = new ExceptionHandledResult(context.Error);
        }
        return task ?? Task.CompletedTask;
    }

    public Task<bool> KnowHowToHandleAsync(Exception exception)
    {
        return Task.FromResult(exception is TException);
    }

    public void AppendHandler(Func<ExceptionHandlingContext, Task> handler)
    {
        if (handler == null) { throw new ArgumentNullException(nameof(handler)); }
        if (_handler == null)
        {
            _handler = handler;
            return;
        }
        _handler = (context) =>
        {
            var task1 = _handler.Invoke(context);
            var task2 = handler.Invoke(context);
            return Task.WhenAll(task1, task2);
        };
    }

    public void PrependHandler(Func<ExceptionHandlingContext, Task> handler)
    {
        if (handler == null) { throw new ArgumentNullException(nameof(handler)); }
        if (_handler == null)
        {
            _handler = handler;
            return;
        }
        _handler = (context) =>
        {
            var task1 = handler.Invoke(context);
            var task2 = _handler.Invoke(context);
            return Task.WhenAll(task1, task2);
        };
    }

    public void ReplaceHandler(Func<ExceptionHandlingContext, Task> handler)
    {
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
    }
}

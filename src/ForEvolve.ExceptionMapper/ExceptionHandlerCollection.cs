using ForEvolve.ExceptionMapper;
using System.Collections;

namespace Microsoft.Extensions.DependencyInjection;

public class ExceptionHandlerCollection : IList<IExceptionHandler>
{
    private List<IExceptionHandler> _exceptionHandlers = new();
    public ExceptionHandlerCollection(IEnumerable<IExceptionHandler> exceptionHandlers)
    {
        _exceptionHandlers.AddRange(exceptionHandlers);
    }

    public IExceptionHandler this[int index] { get => ((IList<IExceptionHandler>)_exceptionHandlers)[index]; set => ((IList<IExceptionHandler>)_exceptionHandlers)[index] = value; }

    public int Count => ((ICollection<IExceptionHandler>)_exceptionHandlers).Count;

    public bool IsReadOnly => ((ICollection<IExceptionHandler>)_exceptionHandlers).IsReadOnly;

    public void Add(IExceptionHandler item)
    {
        ((ICollection<IExceptionHandler>)_exceptionHandlers).Add(item);
    }

    public void Clear()
    {
        ((ICollection<IExceptionHandler>)_exceptionHandlers).Clear();
    }

    public bool Contains(IExceptionHandler item)
    {
        return ((ICollection<IExceptionHandler>)_exceptionHandlers).Contains(item);
    }

    public void CopyTo(IExceptionHandler[] array, int arrayIndex)
    {
        ((ICollection<IExceptionHandler>)_exceptionHandlers).CopyTo(array, arrayIndex);
    }

    public IEnumerator<IExceptionHandler> GetEnumerator()
    {
        return ((IEnumerable<IExceptionHandler>)_exceptionHandlers).GetEnumerator();
    }

    public int IndexOf(IExceptionHandler item)
    {
        return ((IList<IExceptionHandler>)_exceptionHandlers).IndexOf(item);
    }

    public void Insert(int index, IExceptionHandler item)
    {
        ((IList<IExceptionHandler>)_exceptionHandlers).Insert(index, item);
    }

    public bool Remove(IExceptionHandler item)
    {
        return ((ICollection<IExceptionHandler>)_exceptionHandlers).Remove(item);
    }

    public void RemoveAt(int index)
    {
        ((IList<IExceptionHandler>)_exceptionHandlers).RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_exceptionHandlers).GetEnumerator();
    }
}
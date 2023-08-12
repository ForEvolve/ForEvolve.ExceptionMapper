using System.Text.Json;

namespace ForEvolve.ExceptionMapper.Serialization.Json;

public class ProblemDetailsSerializationOptions
{
    public bool SerializeExceptions { get; set; } = true;
    public Func<bool> DisplayDebugInformation { get; set; } = () => false;
#if NET6_0
    private const string _obsoleteMessage = "This property was removed when targeting .NET 7+. The library now leverages the `IProblemDetailsService` interface instead.";

    [Obsolete(_obsoleteMessage)]
    public string ContentType { get; set; } = "application/problem+json; charset=utf-8";
#endif
}

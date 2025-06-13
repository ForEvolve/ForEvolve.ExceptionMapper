using System.Text.Json;

namespace ForEvolve.ExceptionMapper.Serialization.Json;

public class ProblemDetailsSerializationOptions
{
    public bool SerializeExceptions { get; set; } = true;
    public Func<ExceptionHandlingContext, bool> DisplayDebugInformation { get; set; } = (ExceptionHandlingContext ctx) => false;
}

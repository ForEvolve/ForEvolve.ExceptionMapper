using System.Text.Json;

namespace ForEvolve.ExceptionMapper.Serialization.Json
{
    public class ProblemDetailsSerializationOptions
    {
        public bool SerializeExceptions { get; set; } = true;
        public Func<bool> DisplayDebugInformation { get; set; } = () => false;
#if NET6_0
        public string ContentType { get; set; } = "application/problem+json; charset=utf-8";
        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions(JsonSerializerDefaults.Web);
#endif
    }
}

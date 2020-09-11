using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace ForEvolve.ExceptionMapper.Serialization.Json
{
    public class ProblemDetailsSerializationOptions
    {
        public ProblemDetailsSerializationOptions()
        {
            SerializeExceptions = true;
            DisplayDebugInformation = () => false;
            ContentType = "application/problem+json; charset=utf-8";
        }

        public bool SerializeExceptions { get; set; }
        public Func<bool> DisplayDebugInformation { get; set; }
        public string ContentType { get; set; }
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
    }
}

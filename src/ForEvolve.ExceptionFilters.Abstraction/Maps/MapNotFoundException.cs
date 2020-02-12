using System;
using System.Runtime.Serialization;

namespace ForEvolve.ExceptionFilters
{
    public class MapNotFoundException : ForEvolveException
    {
        public MapNotFoundException(Type exceptionType)
            : base($"No map for '{exceptionType.FullName}' was found.")
        {
        }
    }
}
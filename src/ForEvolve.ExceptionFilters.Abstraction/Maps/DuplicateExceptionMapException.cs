using System;
using System.Runtime.Serialization;

namespace ForEvolve.ExceptionFilters
{
    public class DuplicateExceptionMapException : ForEvolveException
    {
        public ExceptionMap ExceptionMap { get; }
        public ExceptionMap ExistingMap { get; }

        public DuplicateExceptionMapException(ExceptionMap exceptionMap, ExceptionMap existingMap)
            : base($"A map for '{exceptionMap.ExceptionName}' already exists.")
        {
            ExceptionMap = exceptionMap ?? throw new ArgumentNullException(nameof(exceptionMap));
            ExistingMap = existingMap ?? throw new ArgumentNullException(nameof(existingMap));
        }
    }
}
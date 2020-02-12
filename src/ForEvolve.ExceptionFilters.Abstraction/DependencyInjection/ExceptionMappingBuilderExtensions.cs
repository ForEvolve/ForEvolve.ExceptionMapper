using System;

namespace ForEvolve.ExceptionFilters
{
    public static class ExceptionMappingBuilderExtensions
    {
        public static FluentBuilder<TException> Map<TException>(this IExceptionMappingBuilder builder)
            where TException : Exception
        {
            return new FluentBuilder<TException>(builder);
        }
    }
}

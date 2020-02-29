using System;

namespace ForEvolve.ExceptionFilters
{
    public class InternalServerErrorException : ServerErrorException
    {
        public InternalServerErrorException(Exception innerException)
            : base(innerException.Message, innerException)
        {
            
        }
    }
}

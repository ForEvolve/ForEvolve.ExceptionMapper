using System;

namespace ForEvolve.ExceptionMapper
{
    public abstract class ServerErrorException : ForEvolveException
    {
        public ServerErrorException()
        {
        }

        public ServerErrorException(string message) : base(message)
        {
        }

        public ServerErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

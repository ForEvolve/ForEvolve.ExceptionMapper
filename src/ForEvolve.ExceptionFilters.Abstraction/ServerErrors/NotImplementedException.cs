namespace ForEvolve.ExceptionFilters
{
    internal class NotImplementedException : ServerErrorException
    {
        public NotImplementedException(System.NotImplementedException innerException)
            : base(innerException.Message, innerException)
        {

        }
    }
}

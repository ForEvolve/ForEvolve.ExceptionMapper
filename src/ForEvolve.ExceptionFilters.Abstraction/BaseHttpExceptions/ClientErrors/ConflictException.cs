﻿using System;

namespace ForEvolve.ExceptionFilters
{
    public class ConflictException : ClientErrorException
    {
        public ConflictException()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

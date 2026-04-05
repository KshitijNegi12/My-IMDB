using System;

namespace IMDB.Exceptions
{
    class InvalidFieldException : Exception
    {
        public InvalidFieldException(string message) : base(message)
        {
        }

        public InvalidFieldException(string message, Exception innerException) 
        : base(message, innerException) { }
    }
}

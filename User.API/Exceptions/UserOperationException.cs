using System;

namespace User.API.Exceptions
{
    public class UserOperationException : Exception
    {
        public UserOperationException() { }
        public UserOperationException(string message) : base(message) { }
        public UserOperationException(string message, Exception innerExceptions) : base(message, innerExceptions) { }
    }
}

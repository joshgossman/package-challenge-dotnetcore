using System;

namespace com.mobiquity.packer.Exceptions
{
    public class APIException : Exception
    {
        public APIException()
        {
        }

        public APIException(string message)
            : base(message)
        {
        }

        public APIException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

namespace SampleForElastic.Common.Base
{
    public class KeyNotFoundApplicationException : ApplicationException.ApplicationException
    {
        public KeyNotFoundApplicationException()
        {
        }

        public KeyNotFoundApplicationException(string? message) : base(message)
        {
        }

        public KeyNotFoundApplicationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}

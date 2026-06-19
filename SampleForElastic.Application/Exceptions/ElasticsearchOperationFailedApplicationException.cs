namespace SampleForElastic.Common.Base
{

    public class ElasticsearchOperationFailedApplicationException : ApplicationException.ApplicationException
    {
        public ElasticsearchOperationFailedApplicationException() { }
        public ElasticsearchOperationFailedApplicationException(string? message) : base(message) { }
        public ElasticsearchOperationFailedApplicationException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}



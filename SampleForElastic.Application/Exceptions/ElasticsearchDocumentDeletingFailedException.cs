namespace SampleForElastic.Application.Exceptions
{
    public class ElasticsearchDocumentDeletingFailedException : SampleForElastic.Common.Base.ElasticsearchOperationFailedApplicationException
    {
        public ElasticsearchDocumentDeletingFailedException(Guid userId, string details) 
            : base($"Elasticsearch Document Deleting Failed for User {userId}: {details}") { }
    }
}

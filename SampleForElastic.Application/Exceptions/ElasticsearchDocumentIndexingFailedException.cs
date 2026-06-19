
namespace SampleForElastic.Application.Exceptions
{
    public class ElasticsearchDocumentIndexingFailedException : SampleForElastic.Common.Base.ElasticsearchOperationFailedApplicationException
    {
        public ElasticsearchDocumentIndexingFailedException(Guid userId, string details)
            : base($"Elasticsearch Document Indexing Failed for User {userId}: {details}") { }
    }
}

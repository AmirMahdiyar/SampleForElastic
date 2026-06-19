
namespace SampleForElastic.Application.Exceptions
{
    public class ElasticsearchDocumentUpdatingFailedException : SampleForElastic.Common.Base.ElasticsearchOperationFailedApplicationException
    {
        public ElasticsearchDocumentUpdatingFailedException(Guid userId, string details)
            : base($"Elasticsearch Document Updating Failed for User {userId}: {details}") { }
    }
}

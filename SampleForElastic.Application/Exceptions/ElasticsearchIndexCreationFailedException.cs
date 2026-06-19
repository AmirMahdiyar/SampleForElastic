
namespace SampleForElastic.Application.Exceptions
{
    public class ElasticsearchIndexCreationFailedException : SampleForElastic.Common.Base.ElasticsearchOperationFailedApplicationException
    {
        public ElasticsearchIndexCreationFailedException(string details)
            : base($"Elasticsearch Index Creation Failed: {details}") { }
    }
}

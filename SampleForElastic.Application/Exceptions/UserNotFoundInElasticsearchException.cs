
namespace SampleForElastic.Application.Exceptions
{
    public class UserNotFoundInElasticsearchException : SampleForElastic.Common.Base.KeyNotFoundApplicationException
    {
        public UserNotFoundInElasticsearchException(Guid userId)
            : base($"User with ID {userId} was not found in Elasticsearch read-model cluster.") { }
    }
}

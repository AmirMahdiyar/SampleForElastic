using MediatR;
using SampleForElastic.Application.Contracts;

namespace SampleForElastic.Application.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserSearchModel?>
    {
        public Guid UserId { get; set; }

        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}

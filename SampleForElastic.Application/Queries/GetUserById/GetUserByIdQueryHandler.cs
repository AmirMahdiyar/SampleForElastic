using MediatR;
using Mokeb.Application.Queries.GetUserById;
using SampleForElastic.Application.Contracts;
using SampleForElastic.Application.Exceptions;

namespace SampleForElastic.Application.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResponse>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserByIdQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<GetUserByIdQueryResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            UserSearchModel? user = await GetUser(request, cancellationToken);
            return new GetUserByIdQueryResponse { User = user };
        }




        private async Task<UserSearchModel?> GetUser(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundInElasticsearchException(request.UserId);
            }

            return user;
        }
    }
}

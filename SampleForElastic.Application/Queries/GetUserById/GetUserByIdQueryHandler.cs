using MediatR;
using SampleForElastic.Application.Contracts;

namespace SampleForElastic.Application.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserSearchModel?>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserByIdQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<UserSearchModel?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userReadRepository.GetByIdAsync(request.UserId, cancellationToken);
        }
    }
}

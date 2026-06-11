using MediatR;
using SampleForElastic.Application.Contracts;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SampleForElastic.Application.Queries.SearchUsers
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, IEnumerable<UserSearchModel>>
    {
        private readonly IUserReadRepository _userReadRepository;

        public SearchUsersQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<IEnumerable<UserSearchModel>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                return Array.Empty<UserSearchModel>();
            }

            return await _userReadRepository.SearchAsync(request.SearchTerm, cancellationToken);
        }
    }
}

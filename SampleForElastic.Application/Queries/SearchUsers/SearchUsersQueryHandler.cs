using MediatR;
using Mokeb.Application.Queries.SearchUsers;
using SampleForElastic.Application.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleForElastic.Application.Queries.SearchUsers
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, SearchUsersQueryResponse>
    {
        private readonly IUserReadRepository _userReadRepository;

        public SearchUsersQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<SearchUsersQueryResponse> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                return new SearchUsersQueryResponse();
            }

            var users = await _userReadRepository.SearchAsync(request.SearchTerm, cancellationToken);
            return new SearchUsersQueryResponse { Users = users.ToList() };
        }
    }
}

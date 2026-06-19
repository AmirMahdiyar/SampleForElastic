using MediatR;
using Mokeb.Application.Queries.SearchUsers;

namespace SampleForElastic.Application.Queries.SearchUsers
{
    public class SearchUsersQuery : IRequest<SearchUsersQueryResponse>
    {
        public string SearchTerm { get; set; }

        public SearchUsersQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}

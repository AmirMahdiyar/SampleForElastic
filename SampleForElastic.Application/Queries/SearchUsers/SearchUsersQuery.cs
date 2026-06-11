using MediatR;
using SampleForElastic.Application.Contracts;
using System.Collections.Generic;

namespace SampleForElastic.Application.Queries.SearchUsers
{
    public class SearchUsersQuery : IRequest<IEnumerable<UserSearchModel>>
    {
        public string SearchTerm { get; set; }

        public SearchUsersQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}

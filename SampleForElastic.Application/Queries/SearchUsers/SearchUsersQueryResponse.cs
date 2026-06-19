using SampleForElastic.Application.Contracts;
using System.Collections.Generic;

namespace Mokeb.Application.Queries.SearchUsers
{
    public class SearchUsersQueryResponse
    {
        public List<UserSearchModel> Users { get; set; } = new();
    }
}

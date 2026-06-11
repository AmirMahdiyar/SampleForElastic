using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SampleForElastic.Application.Contracts
{
    public interface IUserReadRepository
    {
        Task<UserSearchModel?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<UserSearchModel>> SearchAsync(string searchTerm, CancellationToken ct);
        Task IndexAsync(UserSearchModel model, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
        Task InitializeIndexAsync(CancellationToken ct);
    }
}

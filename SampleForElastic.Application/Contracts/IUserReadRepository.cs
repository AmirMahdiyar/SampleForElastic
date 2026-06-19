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
        Task<bool> IndexAsync(UserSearchModel model, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
        Task<bool> InitializeIndexAsync(CancellationToken ct);
        Task<bool> AddCarAsync(Guid userId, CarSearchModel car, CancellationToken ct);
        Task<bool> RemoveCarAsync(Guid userId, Guid carId, CancellationToken ct);
        Task<bool> UpdateUserFieldsAsync(Guid userId, string username, string about, DateOnly birthDate, CancellationToken ct);
    }
}

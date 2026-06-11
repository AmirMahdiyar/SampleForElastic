using SampleForElastic.Domain.Models.UserAggregate;

namespace SampleForElastic.Application.Commands.Contracts
{
    public interface IUserWriteRepository
    {
        Task<bool> IsExistAsync(string username, CancellationToken ct);
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(User user, CancellationToken ct);
    }
}

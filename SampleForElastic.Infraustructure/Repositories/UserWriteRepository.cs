using Microsoft.EntityFrameworkCore;
using SampleForElastic.Application.Commands.Contracts;
using SampleForElastic.Domain.Models.UserAggregate;

namespace SampleForElastic.Infraustructure.Repositories
{
    public class UserWriteRepository : IUserWriteRepository
    {
        private readonly SampleDbContext _context;
        private readonly DbSet<User> _users;

        public UserWriteRepository(SampleDbContext context)
        {
            _context = context;
            _users = context.Set<User>();
        }

        public async Task<bool> IsExistAsync(string username, CancellationToken ct)
        {
            return await _users.AnyAsync(x => x.Username == username, ct);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _users
                .Include(x => x.Cars)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task AddAsync(User user, CancellationToken ct)
        {
            await _users.AddAsync(user, ct);
        }
    }
}

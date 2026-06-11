using SampleForElastic.Application.Contracts;
using static SampleForElastic.Application.Contracts.IUnitOfWork;

namespace SampleForElastic.Infraustructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SampleDbContext _context;

        public UnitOfWork(SampleDbContext context)
        {
            _context = context;
        }

        public async Task<SavingResult> Commit(CancellationToken ct)
        {
            var savedChangedStateCount = await _context.SaveChangesAsync(ct);
            return new SavingResult { ChangesCount = savedChangedStateCount };
        }
    }
}

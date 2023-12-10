using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using GymManagement.Infrastructure.Common.Persistence;

namespace GymManagement.Infrastructure.Gyms.Persistence;

public class GymsRepository(GymManagementDbContext dbContext) : IGymsRepository
{
    private readonly GymManagementDbContext _dbContext = dbContext;

    public async Task AddAsync(Gym gym, CancellationToken cancellationToken = default)
    {
        await _dbContext.Gyms.AddAsync(gym, cancellationToken);
    }

    public ValueTask<Gym?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Gyms.FindAsync(id, cancellationToken);
    }

    public void Update(Gym gym)
    {
        _dbContext.Gyms.Update(gym);
    }
}
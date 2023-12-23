using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IReadOnlyList<Gym>> ListBySubscriptionIdAsync(Guid subscriptionId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Gyms
            .Where(gym => gym.SubscriptionId == subscriptionId)
            .ToListAsync(cancellationToken);
    }
    
    public void RemoveRange(IEnumerable<Gym> gyms)
    {
        _dbContext.Gyms.RemoveRange(gyms);
    }
}
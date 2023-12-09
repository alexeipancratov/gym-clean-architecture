using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using GymManagement.Infrastructure.Common.Persistence;

namespace GymManagement.Infrastructure.Gyms.Persistence;

public class GymsRepository : IGymsRepository
{
    private readonly GymManagementDbContext _dbContext;

    public GymsRepository(GymManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(Gym gym, CancellationToken cancellationToken = default)
    {
        await _dbContext.Gyms.AddAsync(gym, cancellationToken);
    }
}
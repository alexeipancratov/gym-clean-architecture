using GymManagement.Domain.Gyms;

namespace GymManagement.Application.Common.Interfaces;

public interface IGymsRepository
{
    Task AddAsync(Gym gym, CancellationToken cancellationToken = default);
    
    ValueTask<Gym?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    void Update(Gym gym);
    
    Task<IReadOnlyList<Gym>> ListBySubscriptionIdAsync(Guid subscriptionId,
        CancellationToken cancellationToken = default);
    
    void RemoveRange(IEnumerable<Gym> gyms);
}
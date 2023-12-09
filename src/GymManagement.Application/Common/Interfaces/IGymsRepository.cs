using GymManagement.Domain.Gyms;

namespace GymManagement.Application.Common.Interfaces;

public interface IGymsRepository
{
    Task AddAsync(Gym gym, CancellationToken cancellationToken = default);
}
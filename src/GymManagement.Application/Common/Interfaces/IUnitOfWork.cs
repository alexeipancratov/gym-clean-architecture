namespace GymManagement.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task<int> CommitChangesAsync(CancellationToken cancellationToken = default);
}
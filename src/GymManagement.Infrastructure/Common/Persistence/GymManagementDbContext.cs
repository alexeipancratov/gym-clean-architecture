using System.Reflection;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Domain.Gyms;
using GymManagement.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymManagementDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    
    public DbSet<Gym> Gyms => Set<Gym>();
    
    public DbSet<Admin> Admins => Set<Admin>();

    public Task<int> CommitChangesAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
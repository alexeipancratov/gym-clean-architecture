using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Infrastructure.Common.Persistence;

namespace GymManagement.Infrastructure.Admins;

public class AdminsRepository(GymManagementDbContext dbContext) : IAdminsRepository
{
    private readonly GymManagementDbContext _dbContext = dbContext;
    
    public ValueTask<Admin?> GetByIdAsync(Guid adminId)
    {
        return _dbContext.Admins.FindAsync(adminId);
    }

    public void Update(Admin admin)
    {
        _dbContext.Admins.Update(admin);
    }
}
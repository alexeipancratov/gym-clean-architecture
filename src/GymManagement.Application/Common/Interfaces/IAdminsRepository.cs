using GymManagement.Domain.Admins;

namespace GymManagement.Application.Common.Interfaces;

public interface IAdminsRepository
{
    ValueTask<Admin?> GetByIdAsync(Guid adminId);
    
    void Update(Admin admin);
}
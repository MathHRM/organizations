using app_backend.App.Models;

namespace app_backend.App.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<User?> GetUserWithOrganizationsAsync(int userId);
        Task<User?> GetUserWithOwnedOrganizationAsync(int userId);
        Task<OrganizationUser> AddOrganizationUserAsync(OrganizationUser organizationUser);
    }
}
using app_backend.App.Models;

namespace app_backend.App.Services.IServices
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(string email);
        Task<User?> ValidateUserCredentialsAsync(string email, string password, int? organizationId = null);
        Task<User?> GetUserWithOrganizationsAsync(int userId);
        Task<User?> GetUserWithOwnedOrganizationAsync(int userId);
    }
}
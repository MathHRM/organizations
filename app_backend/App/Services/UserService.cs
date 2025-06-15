using app_backend.App.Enums;
using app_backend.App.Models;
using app_backend.App.Repositories.IRepositories;
using app_backend.App.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace app_backend.App.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly AppDbContext _context;

        public UserService(IUserRepository userRepository, AppDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.OrganizationUsers)
                .ThenInclude(ou => ou.Organization)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            var organization = new Organization { Name = $"{user.Name}'s Organization" };
            var organizationUser = new OrganizationUser
            {
                User = user,
                Organization = organization,
                Role = (int) Role.Owner
            };
            user.OrganizationUsers = new List<OrganizationUser> { organizationUser };

            await _context.Users.AddAsync(user);
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Name = user.Name ?? existingUser.Name;
            existingUser.Email = user.Email ?? existingUser.Email;

            if (!string.IsNullOrEmpty(user.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            await _userRepository.UpdateUserAsync(existingUser);
            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserAsync(id);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);
            return user != null;
        }

        public async Task<User?> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return user;
        }

        public async Task<User?> GetUserWithOrganizationsAsync(int userId)
        {
            return await _userRepository.GetUserWithOrganizationsAsync(userId);
        }

        public async Task<User?> GetUserWithOwnedOrganizationAsync(int userId)
        {
            var user = await _userRepository.GetUserWithOwnedOrganizationAsync(userId);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public Organization? GetUserOwnedOrganization(User user)
        {
            return user.OrganizationUsers.FirstOrDefault(ou => ou.Role == (int) Role.Owner)?.Organization;
        }
    }
}
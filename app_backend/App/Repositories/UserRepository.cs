using app_backend.App.Enums;
using app_backend.App.Models;
using app_backend.App.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace app_backend.App.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<User?> GetUserWithOrganizationsAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.OrganizationUsers)
                .ThenInclude(ou => ou.Organization)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithOwnedOrganizationAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.OrganizationUsers)
                .ThenInclude(ou => ou.Organization)
                .FirstOrDefaultAsync(u => u.Id == userId && u.OrganizationUsers.Any(ou => ou.Role == (int) Role.Owner));
        }

        public async Task<OrganizationUser> AddOrganizationUserAsync(OrganizationUser organizationUser)
        {
            await _context.OrganizationUsers.AddAsync(organizationUser);
            await _context.SaveChangesAsync();
            return organizationUser;
        }
    }
}
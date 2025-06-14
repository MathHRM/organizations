using app_backend.App.Models;
using app_backend.App.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace app_backend.App.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AppDbContext _context;

        public OrganizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Organization?> GetOrganizationByIdAsync(int id)
        {
            return await _context.Organizations.FindAsync(id);
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            return await _context.Organizations.ToListAsync();
        }

        public async Task<Organization> AddOrganizationAsync(Organization organization)
        {
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task<bool> UpdateOrganizationAsync(Organization organization)
        {
            _context.Organizations.Update(organization);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrganizationAsync(int id)
        {
            var Organization = await _context.Organizations.FindAsync(id);
            if (Organization != null)
            {
                _context.Organizations.Remove(Organization);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
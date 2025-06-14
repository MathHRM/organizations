using app_backend.App.Models;

namespace app_backend.App.Repositories.IRepositories
{
    public interface IOrganizationRepository
    {
        Task<Organization?> GetOrganizationByIdAsync(int id);
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
        Task<Organization> AddOrganizationAsync(Organization organization);
        Task<bool> UpdateOrganizationAsync(Organization organization);
    }
}
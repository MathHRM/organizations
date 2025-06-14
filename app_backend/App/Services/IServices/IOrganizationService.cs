using app_backend.App.Models;

namespace app_backend.App.Services.IServices
{
    public interface IOrganizationService
    {
        Task<Organization?> GetOrganizationByIdAsync(int id);
        Task<IEnumerable<Organization>> GetAllOrganizationsAsync();
        Task<Organization> CreateOrganizationAsync(Organization organization);
        Task<Organization?> UpdateOrganizationAsync(int id, Organization organization);
    }
} 
using app_backend.App.Models;
using app_backend.App.Repositories.IRepositories;
using app_backend.App.Services.IServices;

namespace app_backend.App.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<Organization?> GetOrganizationByIdAsync(int id)
        {
            return await _organizationRepository.GetOrganizationByIdAsync(id);
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
        {
            return await _organizationRepository.GetAllOrganizationsAsync();
        }

        public async Task<Organization> CreateOrganizationAsync(Organization organization)
        {
            return await _organizationRepository.AddOrganizationAsync(organization);
        }

        public async Task<Organization?> UpdateOrganizationAsync(int id, Organization organization)
        {
            var existing = await _organizationRepository.GetOrganizationByIdAsync(id);
            if (existing == null) return null;

            existing.Name = organization.Name ?? existing.Name;
            await _organizationRepository.UpdateOrganizationAsync(existing);
            return existing;
        }
    }
}
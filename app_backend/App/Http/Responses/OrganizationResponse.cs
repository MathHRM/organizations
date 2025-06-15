using app_backend.App.Enums;

namespace app_backend.App.Http.Responses
{
    public class OrganizationResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public Role Role { get; set; }
    }
}
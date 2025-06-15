using System.ComponentModel.DataAnnotations;

namespace app_backend.App.Http.Requests
{
    public class EnterOrganizationRequest
    {
        [Required(ErrorMessage = "OrganizationId is required")]
        public int OrganizationId { get; set; }
    }
}
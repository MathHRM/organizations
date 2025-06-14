using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_backend.App.Models
{
    public class OrganizationUser
    {
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int OrganizationId { get; set; }

        public Organization Organization { get; set; } = null!;

        public int Role { get; set; }
    }
}
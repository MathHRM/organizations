using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_backend.App.Models
{
    [Table("organization_user")]
    public class OrganizationUser
    {
        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("role")]
        public int Role { get; set; }

        public User User { get; set; } = null!;

        public Organization Organization { get; set; } = null!;
    }
}
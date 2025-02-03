using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_backend.Models
{
    [Table("organization_user")]
    public class OrganizationUser
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        [Column("role")]
        public int RoleId { get; set; }
    }
}
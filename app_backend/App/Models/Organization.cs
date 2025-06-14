using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_backend.App.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<OrganizationUser> OrganizationUsers { get; set; } = [];
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for user roles
    /// </summary>
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        public required string Name { get; set; }

        // Связь с пользователями (One-to-Many: Role -> Users)
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

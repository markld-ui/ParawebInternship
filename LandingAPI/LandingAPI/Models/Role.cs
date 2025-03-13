namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for user roles
    /// </summary>
    public class Role
    {
        public int RoleId { get; set; }
        public required string Name { get; set; }

        // Связь с пользователями (One-to-Many: Role -> Users)
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

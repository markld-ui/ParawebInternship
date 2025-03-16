namespace LandingAPI.Models
{
    /// <summary>
    /// Data-class for user roles
    /// </summary>
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }

        // Связь с ролью (Many-to-Many: Role <-> Users) -> (One-to-Many; Many-to-One)
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}

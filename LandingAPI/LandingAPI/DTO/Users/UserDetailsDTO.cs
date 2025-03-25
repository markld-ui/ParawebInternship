using System;
using System.Collections.Generic;

namespace LandingAPI.DTO.Users
{
    public class UserDetailsDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RoleDTO> Roles { get; set; } = new();
    }

    public class RoleDTO
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
    }
}

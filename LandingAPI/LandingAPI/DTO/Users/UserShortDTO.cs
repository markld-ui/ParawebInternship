using System;

namespace LandingAPI.DTO.Users
{
    public class UserShortDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public string MainRole { get; set; }
    }
}

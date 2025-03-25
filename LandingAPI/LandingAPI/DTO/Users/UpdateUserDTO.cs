using System.ComponentModel.DataAnnotations;

namespace LandingAPI.DTO.Users
{
    public class UpdateUserDTO
    {
        [StringLength(50, ErrorMessage = "Имя пользователя не должно превышать 50 символов")]
        public string? Username { get; set; }

        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string? Email { get; set; }

        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        public string? Password { get; set; }

        public int? RoleId { get; set; }
    }
}

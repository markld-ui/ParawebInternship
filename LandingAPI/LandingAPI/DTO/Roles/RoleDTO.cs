namespace LandingAPI.DTO.Roles
{
    #region Класс RoleDTO
    /// <summary>
    /// Data Transfer Object (DTO) для представления роли пользователя.
    /// </summary>
    public class RoleDTO
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор роли.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
    #endregion
}

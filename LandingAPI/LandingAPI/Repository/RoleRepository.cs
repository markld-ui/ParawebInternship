#region Заголовок файла

/// <summary>
/// Файл: RoleRepository.cs
/// Реализация репозитория для работы с сущностью <see cref="Role"/>.
/// Предоставляет методы для доступа к данным о ролях в базе данных.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.Data;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace LandingAPI.Repository
{
    #region Класс RoleRepository

    /// <summary>
    /// Реализация репозитория для работы с сущностью <see cref="Role"/>.
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        #region Поля

        private readonly DataContext _context;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RoleRepository"/>.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public RoleRepository(DataContext context)
        {
            _context = context;
        }

        #endregion

        #region Методы

        /// <summary>
        /// Получает список всех ролей.
        /// </summary>
        /// <returns>Коллекция ролей.</returns>
        public async Task<ICollection<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        /// <summary>
        /// Получает роль по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор роли.</param>
        /// <returns>Роль, если найдена, иначе <c>null</c>.</returns>
        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        /// <summary>
        /// Получает роль по ее названию.
        /// </summary>
        /// <param name="name">Название роли.</param>
        /// <returns>Роль, если найдена, иначе <c>null</c>.</returns>
        public async Task<Role> GetRoleByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }

        /// <summary>
        /// Добавляет новую роль в базу данных.
        /// </summary>
        /// <param name="role">Роль для добавления.</param>
        public async Task AddRoleAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновляет существующую роль в базе данных.
        /// </summary>
        /// <param name="role">Роль для обновления.</param>
        public async Task UpdateRoleAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет роль из базы данных.
        /// </summary>
        /// <param name="role">Роль для удаления.</param>
        public async Task DeleteRoleAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        #endregion
    }

    #endregion
}
﻿#region Заголовок файла

/// <summary>
/// Файл: MappingProfiles.cs
/// Класс для настройки маппинга между сущностями и их DTO с использованием AutoMapper.
/// Определяет правила преобразования объектов моделей в DTO и наоборот.
/// </summary>

#endregion

#region Пространства имен

using AutoMapper;
using LandingAPI.DTO.Events;
using LandingAPI.DTO.Files;
using LandingAPI.DTO.News;
using LandingAPI.DTO.Users;
using LandingAPI.Models;

#endregion

namespace LandingAPI.Helper
{
    #region Класс MappingProfiles

    /// <summary>
    /// Класс для настройки маппинга между сущностями и их DTO.
    /// </summary>
    public class MappingProfiles : Profile
    {
        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MappingProfiles"/> и настраивает маппинг.
        /// </summary>
        public MappingProfiles()
        {
            // Настройка маппинга для сущности News и NewsDTO
            CreateMap<News, NewsDTO>();
            CreateMap<News, CreateNewsDTO>();
            CreateMap<News, NewsDetailsDTO>();
            CreateMap<News, NewsShortDTO>();
            CreateMap<News, UpdateNewsDTO>();

            // Настройка маппинга для сущности User и UserDTO
            CreateMap<User, UserDTO>();
            CreateMap<User, UserDetailsDTO>();
            CreateMap<User, UserShortDTO>();
            CreateMap<User, UpdateUserDTO>();

            // Настройка маппинга для сущности Event и EventDTO
            CreateMap<Event, EventDTO>();

            // Настройка маппинга для сущности Files и FilesDTO
            CreateMap<Files, FilesDTO>();
        }

        #endregion
    }

    #endregion
}
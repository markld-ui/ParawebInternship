#region Заголовок файла

/// <summary>
/// Файл: FilesController.cs
/// Контроллер для управления файлами.
/// Предоставляет методы для получения списка файлов, поиска файла по идентификатору, а также фильтрации файлов по идентификаторам новостей и событий.
/// </summary>

#endregion

#region Пространства имен

using LandingAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Services;

#endregion

namespace LandingAPI.Controllers
{
    #region Класс FilesController

    /// <summary>
    /// Контроллер для управления файлами.
    /// </summary>
    [Route("/api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        #region Поля и свойства

        private readonly IFilesRepository _filesRepository;
        private readonly IMapper _mapper;
        private readonly FileService _fileService;

        #endregion

        #region Конструктор

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FilesController"/>.
        /// </summary>
        /// <param name="filesRepository">Репозиторий для работы с файлами.</param>
        /// <param name="mapper">Объект для маппинга данных между моделями и DTO.</param>
        /// /// <param name="configuration">Конфигурация приложения.</param>
        public FilesController(IFilesRepository filesRepository, IMapper mapper, IConfiguration configuration, FileService fileService)
        {
            _filesRepository = filesRepository;
            _mapper = mapper;
            _fileService = fileService;
        }

        #endregion

        #region Методы

        #region GetFilesAsync

        /// <summary>
        /// Получает список всех файлов.
        /// </summary>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если файлы не найдены.
        /// - 200 OK с списком файлов в формате <see cref="FilesDTO"/>.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FilesDTO>), 200)]
        public async Task<IActionResult> GetFilesAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var files = await _filesRepository.GetFilesAsync();
            if (files == null)
                return NotFound();

            var filesDtos = _mapper.Map<List<FilesDTO>>(files);
            return Ok(filesDtos);
        }

        #endregion

        #region GetFileAsync

        /// <summary>
        /// Получает файл по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор файла.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если файл с указанным идентификатором не найден.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 200 OK с данными файла в формате <see cref="FilesDTO"/>.
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<FilesDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetFileAsync(int id)
        {
            if (!await _filesRepository.FileExistsByIdAsync(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var file = await _filesRepository.GetFileByIdAsync(id);
            var fileDtos = _mapper.Map<FilesDTO>(file);
            return Ok(fileDtos);
        }

        #endregion

        #region GetFileByNewsId

        /// <summary>
        /// Получает файл по идентификатору новости.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если файл не найден.
        /// - 200 OK с данными файла в формате <see cref="FilesDTO"/>.
        /// </returns>
        [HttpGet("news/{newsId}")]
        [ProducesResponseType(typeof(IEnumerable<FilesDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetFileByNewsId(int newsId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var file = await _filesRepository.GetFileByNewsIdAsync(newsId);
            if (file == null)
                return NotFound();

            var fileDtos = _mapper.Map<FilesDTO>(file);
            if (fileDtos == null)
                return NotFound();

            return Ok(fileDtos);
        }

        #endregion

        #region GetFileByEventId

        /// <summary>
        /// Получает файл по идентификатору события.
        /// </summary>
        /// <param name="eventId">Идентификатор события.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если файл не найден.
        /// - 200 OK с данными файла в формате <see cref="FilesDTO"/>.
        /// </returns>
        [HttpGet("events/{eventId}")]
        [ProducesResponseType(typeof(IEnumerable<FilesDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetFileByEventId(int eventId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var file = _filesRepository.GetFileByEventIdAsync(eventId);
            if (file == null)
                return NotFound();

            var fileDtos = _mapper.Map<FilesDTO>(file);
            if (fileDtos == null)
                return NotFound();

            return Ok(fileDtos);
        }

        #endregion

        #region UploadFile
        /// <summary>
        /// Загружает файл на сервер.
        /// </summary>
        /// <param name="file">Файл для загрузки.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 400 BadRequest, если файл не был предоставлен.
        /// - 200 OK с данными загруженного файла.
        /// </returns>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(FilesDTO), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не был предоставлен.");

            try
            {
                var uploadedFile = await _fileService.UploadFileAsync(file);
                var fileDto = _mapper.Map<FilesDTO>(uploadedFile);
                return Ok(fileDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при загрузке файла: {ex.Message}");
            }
        }

        #endregion

        #region DownloadFile
        /// <summary>
        /// Скачивает файл по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор файла.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если файл не найден.
        /// - 200 OK с содержимым файла.
        /// </returns>
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var (file, stream) = await _fileService.GetFileStreamAsync(id);
            if (file == null || stream == null)
                return NotFound();

            return File(stream, "application/octet-stream", file.FileName);
        }

        #endregion

        #region DeleteFile
        /// <summary>
        /// Удаляет файл по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор файла.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 404 NotFound, если файл не найден.
        /// - 204 NoContent, если файл успешно удален.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var result = await _fileService.DeleteFileAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        #endregion

        #endregion
    }

    #endregion
}
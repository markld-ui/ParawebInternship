#region Заголовок файла

/// <summary>
/// Файл: FilesController.cs
/// Контроллер для управления файлами.
/// Предоставляет методы для получения списка файлов, поиска файла по идентификатору, а также фильтрации файлов по идентификаторам новостей и событий.
/// </summary>

#endregion

#region Пространства имен

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Services;
using LandingAPI.DTO.Files;
using LandingAPI.Helper;

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
        /// - 200 OK с списком файлов в формате <see cref="FileDetailsDTO"/> в случае успешного запроса.
        /// - 400 BadRequest, если модель данных невалидна.
        /// - 404 NotFound, если файлы не найдены.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/files?page=1&size=10&sort=UploadedAt&asc=false
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "data": [
        ///     {
        ///       "fileId": 1,
        ///       "fileName": "example.txt",
        ///       "uploadedAt": "2023-01-01T12:00:00Z",
        ///       "downloadUrl": "http://example.com/api/files/download/1",
        ///       "fileSize": 2048
        ///     },
        ///     {
        ///       "fileId": 2,
        ///       "fileName": "sample.pdf",
        ///       "uploadedAt": "2023-01-02T12:00:00Z",
        ///       "downloadUrl": "http://example.com/api/files/download/2",
        ///       "fileSize": 10240
        ///     }
        ///   ],
        ///   "totalCount": 2,
        ///   "page": 1,
        ///   "pageSize": 10
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (файлы не найдены):
        /// ```json
        /// {
        ///   "error": "Файлы не найдены"
        /// }
        /// ```
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<FileDetailsDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFilesAsync(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string sort = "UploadedAt",
            [FromQuery] bool asc = false)
        {
            var (files, totalCount) = await _filesRepository.GetFilesAsync(page, size, sort, asc);
            if (files == null || !files.Any())
                return NotFound("Файлы не найдены");

            var filesDtos = files.Select(f => new FileDetailsDTO
            {
                FileId = f.FileId,
                FileName = f.FileName,
                UploadedAt = f.UploadedAt,
                DownloadUrl = Url.Action("DownloadFile", "Files", new { id = f.FileId }, Request.Scheme),
                FileSize = new FileInfo(f.FilePath).Length
            }).ToList();

            return Ok(new PagedResponse<FileDetailsDTO>
            {
                Data = filesDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = size
            });
        }

        #endregion

        #region GetFileAsync

        /// <summary>
        /// Получает файл по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор файла.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с данными файла в формате <see cref="FileDetailsDTO"/> в случае успешного запроса.
        /// - 404 NotFound, если файл с указанным идентификатором не найден.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/files/1
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "fileId": 1,
        ///   "fileName": "example.txt",
        ///   "uploadedAt": "2023-01-01T12:00:00Z",
        ///   "downloadUrl": "http://example.com/api/files/download/1",
        ///   "fileSize": 2048
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (файл не найден):
        /// ```json
        /// {
        ///   "error": "Файл не найден"
        /// }
        /// ```
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FileDetailsDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFileAsync(int id)
        {
            var file = await _filesRepository.GetFileByIdAsync(id);
            if (file == null)
                return NotFound("Файл не найден");

            return Ok(new FileDetailsDTO
            {
                FileId = file.FileId,
                FileName = file.FileName,
                UploadedAt = file.UploadedAt,
                DownloadUrl = Url.Action("DownloadFile", "Files", new { id = file.FileId }, Request.Scheme),
                FileSize = new FileInfo(file.FilePath).Length
            });
        }

        #endregion

        #region GetFileByNewsId

        /// <summary>
        /// Получает файл по идентификатору новости.
        /// </summary>
        /// <param name="newsId">Идентификатор новости.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с данными файла в формате <see cref="FilesDTO"/> в случае успешного запроса.
        /// - 404 NotFound, если файл не найден.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/files/news/1
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "fileId": 1,
        ///   "fileName": "news_image.jpg",
        ///   "uploadedAt": "2023-01-01T12:00:00Z",
        ///   "downloadUrl": "http://example.com/api/files/download/1",
        ///   "fileSize": 5120
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (файл не найден):
        /// ```json
        /// {
        ///   "error": "Файл не найден"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (невалидная модель):
        /// ```json
        /// {
        ///   "error": "Некорректные данные запроса"
        /// }
        /// ```
        /// </remarks>
        [HttpGet("news/{newsId}")]
        [ProducesResponseType(typeof(IEnumerable<FilesDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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
        /// - 200 OK с данными файла в формате <see cref="FilesDTO"/> в случае успешного запроса.
        /// - 404 NotFound, если файл не найден.
        /// - 400 BadRequest, если модель данных невалидна.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/files/events/1
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "fileId": 1,
        ///   "fileName": "event_image.png",
        ///   "uploadedAt": "2023-01-01T12:00:00Z",
        ///   "downloadUrl": "http://example.com/api/files/download/1",
        ///   "fileSize": 10240
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (файл не найден):
        /// ```json
        /// {
        ///   "error": "Файл не найден"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (невалидная модель):
        /// ```json
        /// {
        ///   "error": "Некорректные данные запроса"
        /// }
        /// ```
        /// </remarks>
        [HttpGet("events/{eventId}")]
        [ProducesResponseType(typeof(IEnumerable<FilesDTO>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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
        /// <param name="dto">Файл для загрузки.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>:
        /// - 200 OK с данными загруженного файла в формате <see cref="FileDetailsDTO"/>.
        /// - 400 BadRequest, если файл не был предоставлен.
        /// - 500 InternalServerError, если произошла ошибка при загрузке файла.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// POST /api/files/upload
        /// 
        /// ### Пример тела запроса:
        /// ```json
        /// {
        ///   "file": "файл в формате multipart/form-data"
        /// }
        /// ```
        /// 
        /// ### Пример успешного ответа:
        /// ```json
        /// {
        ///   "fileId": 1,
        ///   "fileName": "uploaded_file.png",
        ///   "uploadedAt": "2023-01-01T12:00:00Z",
        ///   "downloadUrl": "http://example.com/api/files/download/1",
        ///   "fileSize": 20480
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (файл не был предоставлен):
        /// ```json
        /// {
        ///   "error": "Файл не был предоставлен"
        /// }
        /// ```
        /// 
        /// ### Пример ошибки (внутренняя ошибка сервера):
        /// ```json
        /// {
        ///   "error": "Ошибка при загрузке файла: [описание ошибки]"
        /// }
        /// ```
        /// </remarks>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(FileDetailsDTO), 200)]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDTO dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("Файл не был предоставлен");

            try
            {
                var uploadedFile = await _fileService.UploadFileAsync(dto.File);
                return Ok(new FileDetailsDTO
                {
                    FileId = uploadedFile.FileId,
                    FileName = uploadedFile.FileName,
                    UploadedAt = uploadedFile.UploadedAt,
                    DownloadUrl = Url.Action("DownloadFile", "Files", new { id = uploadedFile.FileId }, Request.Scheme),
                    FileSize = dto.File.Length
                });
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
        /// - 200 OK с содержимым файла в виде потока.
        /// - 404 NotFound, если файл не найден.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// GET /api/files/download/1
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 200 OK**
        /// - **Content-Disposition**: attachment; filename="example_file.png"
        /// - **Content-Type**: application/octet-stream
        /// 
        /// (Содержимое файла будет возвращено в теле ответа.)
        /// 
        /// ### Пример ошибки (файл не найден):
        /// ```json
        /// {
        ///   "error": "Файл не найден"
        /// }
        /// ```
        /// </remarks>
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
        /// - 204 NoContent, если файл успешно удален.
        /// - 404 NotFound, если файл не найден.
        /// </returns>
        /// <remarks>
        /// ### Пример запроса:
        /// DELETE /api/files/{id}
        /// 
        /// ### Пример успешного ответа:
        /// - **HTTP/1.1 204 No Content**
        /// 
        /// (Тело ответа пустое.)
        /// 
        /// ### Пример ошибки (файл не найден):
        /// ```json
        /// {
        ///   "error": "Файл не найден"
        /// }
        /// ```
        /// </remarks>
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
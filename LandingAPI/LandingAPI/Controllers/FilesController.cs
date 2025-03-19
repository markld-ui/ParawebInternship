using LandingAPI.Interfaces;
using LandingAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LandingAPI.Models;

namespace LandingAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {
        private readonly IFilesRepository _filesRepository;
        private readonly IMapper _mapper;
        public FilesController(IFilesRepository filesRepository, IMapper mapper)
        {
            _filesRepository = filesRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FilesDTO>))]
        public IActionResult GetFiles()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var files = _mapper.Map<List<FilesDTO>>(_filesRepository.GetFiles());
            return Ok(files);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(FilesDTO))]
        [ProducesResponseType(400)]
        public IActionResult GetFile(int id)
        {
            if (!_filesRepository.FileExistsById(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var file = _mapper.Map<FilesDTO>(_filesRepository.GetFileById(id));
            return Ok(file);
        }

        // Новый метод для получения файла, связанного с новостью
        [HttpGet("news/{newsId}")]
        [ProducesResponseType(200, Type = typeof(FilesDTO))]
        [ProducesResponseType(400)]
        public IActionResult GetFileByNewsId(int newsId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var file = _mapper.Map<FilesDTO>(_filesRepository.GetFileByNewsId(newsId));
            if (file == null)
                return NotFound();

            return Ok(file);
        }

        // Новый метод для получения файла, связанного с событием
        [HttpGet("events/{eventId}")]
        [ProducesResponseType(200, Type = typeof(FilesDTO))]
        [ProducesResponseType(400)]
        public IActionResult GetFileByEventId(int eventId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var file = _mapper.Map<FilesDTO>(_filesRepository.GetFileByEventId(eventId));
            if (file == null)
                return NotFound();

            return Ok(file);
        }
    }
}
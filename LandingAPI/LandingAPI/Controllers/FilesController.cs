using LandingAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LandingAPI.Models;
using Microsoft.EntityFrameworkCore;
using LandingAPI.Interfaces.Repositories;

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
    }
}
using AutoMapper;
using LandingAPI.DTO;
using LandingAPI.Interfaces;
using LandingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LandingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        public NewsController(INewsRepository newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NewsDTO>), 200)]
        public async Task<IActionResult> GetNewsAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var news = await _newsRepository.GetNewsAsync();
            var newsDtos = _mapper.Map<List<NewsDTO>>(news);
            return Ok(newsDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<NewsDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNewAsync(int id)
        {
            if (!await _newsRepository.NewsExistsAsync(id))
                return NotFound();

            var news = await _newsRepository.GetNewsAsync(id);
            var newsDtos = _mapper.Map<NewsDTO>(news);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(newsDtos);
        }

        [HttpGet("search/{title}")]
        [ProducesResponseType(typeof(IEnumerable<NewsDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetNewByTitleAsync(string title)
        {
            if (!await _newsRepository.NewsExistsByTitleAsync(title))
                return NotFound();

            var news = await _newsRepository.GetNewsByTitleAsync(title);
            var newsDtos = _mapper.Map<NewsDTO>(news);
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(newsDtos);
        }
    }
}

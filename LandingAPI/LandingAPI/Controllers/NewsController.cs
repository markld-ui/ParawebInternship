using AutoMapper;
using LandingAPI.DTO;
using LandingAPI.Interfaces;
using LandingAPI.Models;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(200, Type = typeof(IEnumerable<NewsDTO>))]
        public IActionResult GetNews()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var news = _mapper.Map<List<NewsDTO>>(_newsRepository.GetNews());
            return Ok(news);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(NewsDTO))]
        [ProducesResponseType(400)]
        public IActionResult GetNew(int id)
        {
            if (!_newsRepository.NewsExists(id))
                return NotFound();

            var news = _mapper.Map<NewsDTO>(_newsRepository.GetNews(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(news);
        }

        [HttpGet("search/{title}")]
        [ProducesResponseType(200, Type = typeof(NewsDTO))]
        [ProducesResponseType(400)]
        public IActionResult GetNewByTitle(string title)
        {
            if (!_newsRepository.NewsExistsByTitle(title))
                return NotFound();

            var news = _mapper.Map<NewsDTO>(_newsRepository.GetNewsByTitle(title));
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(news);
        }
    }
}

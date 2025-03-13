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
        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<News>))]
        public IActionResult GetNews()
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var news = _newsRepository.GetNews();
            return Ok(news);
        }
    }
}

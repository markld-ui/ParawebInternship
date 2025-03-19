using LandingAPI.DTO;
using LandingAPI.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace LandingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        public EventsController(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventDTO>), 200)]
        public IActionResult GetEvents()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var events = _mapper.Map<List<EventDTO>>(_eventRepository.GetEvents());
            return Ok(events);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<EventDTO>), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetEvent(int id)
        {
            if (!_eventRepository.EventExistsById(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            var event_ = _mapper.Map<EventDTO>(_eventRepository.GetEventById(id));
            return Ok(event_);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<EventDTO>), 200)]
        [ProducesResponseType(400)]
        public IActionResult GetEventsByUserId(int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var events = _mapper.Map<List<EventDTO>>(_eventRepository.GetEventsByUserId(userId));
            return Ok(events);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LandingAPI.DTO;
using LandingAPI.Interfaces.Repositories;
using LandingAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LandingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
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
        public async Task<IActionResult> GetEvents()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var events = await _eventRepository.GetEventsAsync();
            if (events == null)
                return NotFound();

            var eventDtos = _mapper.Map<List<EventDTO>>(events);
            return Ok(eventDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EventDTO), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEvent(int id)
        {
            if (!await _eventRepository.EventExistsByIdAsync(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var event_ = await _eventRepository.GetEventByIdAsync(id);
            var eventDto = _mapper.Map<EventDTO>(event_);
            return Ok(eventDto);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<EventDTO>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEventsByUserId(int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var events = await _eventRepository.GetEventsByUserIdAsync(userId);
            if (events == null)
                return NotFound();

            var eventDtos = _mapper.Map<List<EventDTO>>(events);
            return Ok(eventDtos);
        }
    }
}
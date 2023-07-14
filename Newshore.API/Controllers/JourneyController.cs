using Microsoft.AspNetCore.Mvc;

using Newshore.Business.Contracts;
using Newshore.Business.DTOs;

namespace Newshore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JourneyController : ControllerBase
    {
        private IJourneyService _journeyService;

        public JourneyController(IJourneyService journeyService)
        {
            _journeyService = journeyService;
        }

        [HttpGet(Name = "GetJourney")]
        public async Task<List<JourneyDTO>> Get([FromQuery] JourneyGetDTO journeyDto)
        {
            return await _journeyService.GetMatchingFlightsAsync(journeyDto);
        }
    }
}
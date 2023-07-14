using Newshore.Business.DTOs;

namespace Newshore.Business.Contracts
{
    public interface IJourneyService
    {
        Task<List<JourneyDTO>> GetMatchingFlightsAsync(JourneyGetDTO journeyDTO);
    }
}

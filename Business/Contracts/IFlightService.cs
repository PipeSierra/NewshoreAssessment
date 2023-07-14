using Newshore.Business.DTOs;
using Newshore.Business.Models;

namespace Newshore.Business.Contracts
{
    public interface IFlightService
    {
        Task<List<FlightAvailableDTO>?> GetAvailableFlightsAsync();
    }
}

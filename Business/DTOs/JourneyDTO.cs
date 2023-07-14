using Newshore.Business.Models;

namespace Newshore.Business.DTOs
{
    public class JourneyDTO
    {
        public string Destination { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public List<Flight> Flights { get; set; } = new();
        public double Price { get; set; }
    }
}

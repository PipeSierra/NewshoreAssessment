namespace Newshore.Business.DTOs
{
    public class FlightAvailableDTO
    {
        public string DepartureStation { get; set; } = string.Empty;
        public string ArrivalStation { get; set; } = string.Empty;
        public string FlightCarrier { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;
        public double Price { get; set; }

    }
}

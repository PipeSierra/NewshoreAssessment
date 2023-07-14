namespace Newshore.Business.Models
{
    public class Flight
    {
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public double Price { get; set; }
        public Transport Transport { get; set; } = new();
    }
}
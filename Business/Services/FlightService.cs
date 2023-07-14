using DataAccess;

using Microsoft.Extensions.Options;

using Newshore.Business.Contracts;
using Newshore.Business.DTOs;
using Newshore.Business.Models;

using System.Net.Http.Json;

namespace Newshore.Business.Services
{
    /// <summary>
    /// Manages the flights.
    /// </summary>
    public class FlightService : IFlightService
    {
        private IHttpClientService _iHttpClientService;
        private IOptions<Settings> _settings;

        public FlightService(IHttpClientService httpClientService, IOptions<Settings> settings)
        {
            _iHttpClientService = httpClientService;
            _settings = settings;
        }

        /// <summary>
        /// Gets the available flights.
        /// </summary>
        /// <returns>A list available flights.</returns>
        public async Task<List<FlightAvailableDTO>?> GetAvailableFlightsAsync()
        {
            List<FlightAvailableDTO>? flights = new();

            HttpResponseMessage response = await _iHttpClientService.GetAsync(_settings.Value.FlightsAPI);
            if (response.IsSuccessStatusCode)
                flights = await response.Content.ReadFromJsonAsync<List<FlightAvailableDTO>?>();

            return flights;
        }
    }
}

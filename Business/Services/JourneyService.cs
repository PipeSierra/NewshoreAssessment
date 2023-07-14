using Newshore.Business.Contracts;
using Newshore.Business.DTOs;
using Newshore.Business.Models;

namespace Newshore.Business.Services
{
    /// <summary>
    /// Manages the journey creation and validation.
    /// </summary>
    public class JourneyService : IJourneyService
    {
        private readonly IFlightService _flightService;
        private readonly Dictionary<string, List<(string FlightNum, string Arrival)>> originDestinationPairs;
        private readonly List<string> flightNumberForJourney;

        public JourneyService(IFlightService flightService)
        {
            _flightService = flightService;
            originDestinationPairs = new();
            flightNumberForJourney = new();
        }

        /// <summary>
        /// Finds the flights that matches with the input parameter origin and destination.
        /// </summary>
        /// <param name="journeyDTO">An object with origin and destination.</param>
        /// <returns>A list with all the possible journeys based on the origin and destination and availability.</returns>
        /// <exception cref="Exception">An error in case of a mismatch with the parameters against the availability.</exception>
        public async Task<List<JourneyDTO>> GetMatchingFlightsAsync(JourneyGetDTO journeyDTO)
        {
            ValidateInput(journeyDTO);

            List<JourneyDTO> journeys = new();

            List<FlightAvailableDTO>? availableFlights = await _flightService.GetAvailableFlightsAsync();

            if (availableFlights is not null && availableFlights.Any())
            {
                foreach (var flight in availableFlights)
                {
                    if (originDestinationPairs.ContainsKey(flight.DepartureStation))
                        originDestinationPairs[flight.DepartureStation].Add((flight.FlightNumber, flight.ArrivalStation));
                    else
                        originDestinationPairs.Add(flight.DepartureStation, new List<(string, string)>() { (flight.FlightNumber, flight.ArrivalStation) });
                }

                if (originDestinationPairs.ContainsKey(journeyDTO.Origin))
                    FindRoutes(journeyDTO.Origin, journeyDTO.Destination, originDestinationPairs[journeyDTO.Origin], new List<string> { journeyDTO.Origin });
                else
                    throw new Exception("No se puede calcular ruta a partir los parámetros seleccionados.");

                journeys = CreateJourney(flightNumberForJourney, availableFlights, journeyDTO.Origin, journeyDTO.Destination);
            }

            return journeys;
        }

        /// <summary>
        /// Based on a list of detinations finds the possible routes from the origin, calls the method recursively if there is an option to go another level down,
        /// </summary>
        /// <param name="departure">Origin of the route.</param>
        /// <param name="destination">Arrival point of the desired route.</param>
        /// <param name="arrivals">Available detinations to arrive.</param>
        /// <param name="visitedOrigins">Keeps the track of the visited point in order not create a circular reference.</param>
        /// <param name="level">Since the set of data is a matrix, it indicates the top level to complete the routing before leaving the function.</param>
        /// <returns>Recursively returns the number of the flight if the detination was found.</returns>
        private string? FindRoutes(string departure, string destination, List<(string FlightNum, string Arrival)> arrivals, List<string> visitedOrigins, int level = 0)
        {
            foreach (var (FlightNum, Arrival) in arrivals)
            {
                if (Arrival == departure) continue;
                else if (Arrival == destination)
                {
                    if (level == 0)
                    {
                        flightNumberForJourney.Add(FlightNum);
                        continue;
                    }
                    else
                        return FlightNum;
                }

                if (originDestinationPairs.ContainsKey(Arrival) && !visitedOrigins.Contains(Arrival))
                {
                    visitedOrigins.Add(Arrival);
                    string? ret = FindRoutes(departure, destination, originDestinationPairs[Arrival], visitedOrigins, level + 1);
                    if (ret is not null)
                    {
                        if (level == 0)
                            flightNumberForJourney.Add($"{FlightNum},{ret}");
                        else
                            return $"{FlightNum},{ret}";
                    }
                }
            }
            return null;
        }

        private List<JourneyDTO> CreateJourney(List<string> flightNums, List<FlightAvailableDTO> availFlights, string origin, string destination)
        {
            List<JourneyDTO> journeys = new();

            foreach (var flights in flightNums)
            {
                JourneyDTO journey = new();
                List<Flight> flightList = new();

                foreach (var flight in flights.Split(","))
                {
                    FlightAvailableDTO? f = availFlights.Single(x => x.FlightNumber == flight);

                    journey.Price += f.Price;
                    flightList.Add(new Flight
                    {
                        Destination = f.ArrivalStation,
                        Origin = f.DepartureStation,
                        Price = f.Price,
                        Transport = new Transport() { FlightCarrier = f.FlightCarrier, FlightNumber = f.FlightNumber },
                    });
                }

                journey.Origin = origin;
                journey.Destination = destination;
                journey.Flights = flightList;
                journeys.Add(journey);
            }

            return journeys;
        }

        private void ValidateInput(JourneyGetDTO journey)
        {
            if (journey.Destination.Length != 3)
                throw new ArgumentException("El parámetro 'Destino' debe contener estrictamente 3 caracteres.");
            if (journey.Origin.Length != 3)
                throw new ArgumentException("El parámetro 'Origen' debe contener estrictamente 3 caracteres.");
            if (journey.Origin == journey.Destination)
                throw new ArgumentException("El Origen debe ser diferente al Destino.");
        }
    }
}

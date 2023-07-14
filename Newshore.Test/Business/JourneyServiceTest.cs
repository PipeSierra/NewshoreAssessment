using Moq;

using Newshore.Business.Contracts;
using Newshore.Business.DTOs;
using Newshore.Business.Services;

namespace Newshore.Test.Business
{
    internal class JourneyServiceTest
    {
        [Test]
        public async Task GetMatchingFlightsAsyncReturnOK()
        {
            //Arrange
            Mock<IFlightService> flightService = new();
            JourneyService sut = new(flightService.Object);
            //JourneyDTO journey = new JourneyDTO() { Destination = "MEX", Origin = "BOG", Price = 30, Flights = new List<Flight>() { } };

            List<FlightAvailableDTO> availflights = new() { new FlightAvailableDTO { ArrivalStation = "MEX", DepartureStation = "BOG", FlightCarrier = "CV", FlightNumber = "50", Price = 30 } };

            flightService.Setup(x => x.GetAvailableFlightsAsync()).ReturnsAsync(availflights);
            //Act
            var res = await sut.GetMatchingFlightsAsync(new JourneyGetDTO { Destination = "MEX", Origin = "BOG" });
            //Assert

            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.EqualTo(1));
            Assert.That(res[0].Flights, Is.Not.Null);
            Assert.That(res[0].Flights.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetMatchingFlightsAsyncReturnEmpty()
        {
            //Arrange
            Mock<IFlightService> flightService = new();
            JourneyService sut = new(flightService.Object);

            List<FlightAvailableDTO> availflights = new();
            flightService.Setup(x => x.GetAvailableFlightsAsync()).ReturnsAsync(availflights);

            //Act
            var res = await sut.GetMatchingFlightsAsync(new JourneyGetDTO { Destination = "MEX", Origin = "BOG" });

            //Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetMatchingFlightsAsyncReturnEmpty2()
        {
            //Arrange
            Mock<IFlightService> flightService = new();
            JourneyService sut = new(flightService.Object);
            flightService.Setup(x => x.GetAvailableFlightsAsync()).ReturnsAsync(() => null);

            //Act
            var res = await sut.GetMatchingFlightsAsync(new JourneyGetDTO { Destination = "MEX", Origin = "BOG" });

            //Assert
            Assert.That(res, Is.Not.Null);
            Assert.That(res.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetMatchingFlightsAsyncReturnError()
        {
            //Arrange
            Mock<IFlightService> flightService = new();
            JourneyService sut = new(flightService.Object);


            List<FlightAvailableDTO> availflights = new() { new FlightAvailableDTO { ArrivalStation = "MZL", DepartureStation = "CTG", FlightCarrier = "CV", FlightNumber = "50", Price = 30 } };
            flightService.Setup(x => x.GetAvailableFlightsAsync()).ReturnsAsync(availflights);

            //Act
            var res = sut.GetMatchingFlightsAsync(new JourneyGetDTO { Destination = "MEX", Origin = "BOG" });

            //Assert
            var ex = Assert.ThrowsAsync<Exception>(() => res);
            Assert.That(ex.Message, Is.EqualTo("No se puede calcular ruta a partir los parámetros seleccionados."));
        }

        [Test]
        public void GetMatchingFlightsAsyncReturnErrorDueToOrigin()
        {
            //Arrange
            Mock<IFlightService> flightService = new();
            JourneyService sut = new(flightService.Object);


            List<FlightAvailableDTO> availflights = new() { new FlightAvailableDTO { ArrivalStation = "MZL", DepartureStation = "CTG", FlightCarrier = "CV", FlightNumber = "50", Price = 30 } };
            flightService.Setup(x => x.GetAvailableFlightsAsync()).ReturnsAsync(availflights);

            //Act
            var res = sut.GetMatchingFlightsAsync(new JourneyGetDTO { Destination = "MEX", Origin = "BOGTA" });

            //Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => res);
            Assert.That(ex.Message, Is.EqualTo("El parámetro 'Origen' debe contener estrictamente 3 caracteres."));
        }

        [Test]
        public void GetMatchingFlightsAsyncReturnErrorDueToDestination()
        {
            //Arrange
            Mock<IFlightService> flightService = new();
            JourneyService sut = new(flightService.Object);


            List<FlightAvailableDTO> availflights = new() { new FlightAvailableDTO { ArrivalStation = "MZL", DepartureStation = "CTG", FlightCarrier = "CV", FlightNumber = "50", Price = 30 } };
            flightService.Setup(x => x.GetAvailableFlightsAsync()).ReturnsAsync(availflights);

            //Act
            var res = sut.GetMatchingFlightsAsync(new JourneyGetDTO { Destination = "MEXCO", Origin = "BOG" });

            //Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => res);
            Assert.That(ex.Message, Is.EqualTo("El parámetro 'Destino' debe contener estrictamente 3 caracteres."));
        }

        [Test]
        public void GetMatchingFlightsAsyncReturnErrorDueToParams()
        {
            //Arrange
            Mock<IFlightService> flightService = new();
            JourneyService sut = new(flightService.Object);


            List<FlightAvailableDTO> availflights = new() { new FlightAvailableDTO { ArrivalStation = "MZL", DepartureStation = "CTG", FlightCarrier = "CV", FlightNumber = "50", Price = 30 } };
            flightService.Setup(x => x.GetAvailableFlightsAsync()).ReturnsAsync(availflights);

            //Act
            var res = sut.GetMatchingFlightsAsync(new JourneyGetDTO { Destination = "BOG", Origin = "BOG" });

            //Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => res);
            Assert.That(ex.Message, Is.EqualTo("El Origen debe ser diferente al Destino."));
        }
    }
}

using Newshore.Business.DTOs;
using Newshore.Business.Models;
using Newshore.Business.Services;

using DataAccess;

using Microsoft.Extensions.Options;

using Moq;

namespace Newshore.Test.Business
{
    internal class FlightServiceTest
    {
        [Test]
        public async Task GetAvailableFlightsAsync_ReturnList()
        {
            //Arrage
            Mock<IOptions<Settings>> settings = new();
            Mock<IHttpClientService> http = new();
            Mock<HttpResponseMessage> httpResponse = new();

            settings.Setup(x => x.Value).Returns(new Settings { FlightsAPI = "" });
            http.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.Accepted, Content = new StringContent("""[{"departureStation": "MZL", "arrivalStation": "MDE", "flightCarrier": "CO", "flightNumber": "8001", "price": 200}]""") });
            FlightService service = new(http.Object, settings.Object);

            //Act
            var flights = await service.GetAvailableFlightsAsync();

            //Assert
            Assert.IsInstanceOf<List<FlightAvailableDTO>>(flights);
            Assert.That(flights, Is.Not.Null);
            Assert.That(flights[0].DepartureStation, Is.EqualTo("MZL"));
        }

        [Test]
        public async Task GetAvailableFlightsAsync_ReturnEmpty()
        {
            //Arrage
            Mock<IOptions<Settings>> settings = new();
            Mock<IHttpClientService> http = new();
            Mock<HttpResponseMessage> httpResponse = new();

            settings.Setup(x => x.Value).Returns(new Settings { FlightsAPI = "" });
            http.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.Accepted, Content = new StringContent("[]") });
            FlightService service = new(http.Object, settings.Object);

            //Act
            var flights = await service.GetAvailableFlightsAsync();

            //Assert
            Assert.IsInstanceOf<List<FlightAvailableDTO>>(flights);
            Assert.That(flights, Is.Empty);            
        }

    }
}

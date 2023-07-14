using Newshore.Business.Contracts;
using Newshore.Business.DTOs;

using Moq;

using Newshore.API.Controllers;

namespace Newshore.Test.API
{
    internal class JourneyContollerTest
    {

        [Test]
        public void JourneyController_Get_ReturnOK()
        {
            //Arrage
            Mock<IJourneyService> service = new();
            service.Setup(x => x.GetMatchingFlightsAsync(new JourneyGetDTO())).ReturnsAsync(new List<JourneyDTO> { new JourneyDTO { Destination = "" } });
            JourneyController controller = new(service.Object);
            
            //Act
            var result = controller.Get(new JourneyGetDTO { Destination = "", Origin = "" });

            //Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}

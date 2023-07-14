
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using Moq;

using Newshore.API.Controllers;


namespace XUnitTests.API.Controllers
{
    public class ErrorControllerTests
    {
        [Test]
        public void Error_ReturnsOkRequestResult()
        {
            var webHostEnvironment = new Mock<IWebHostEnvironment>();
            webHostEnvironment.Setup(x => x.EnvironmentName).Returns("Development");

            IExceptionHandlerFeature exceptionHandlerFeature = new ExceptionHandlerFeature { Error = new Exception() };

            IFeatureCollection features = new FeatureCollection();
            features.Set(exceptionHandlerFeature);


            ErrorController ProfileController = new();
            ControllerContext controllerContext = new();
            controllerContext.HttpContext = new DefaultHttpContext(features);
            ProfileController.ControllerContext = controllerContext;

            ProfileController.Problem();

            var result = ProfileController.HandleErrorDevelopment(webHostEnvironment.Object);

            Assert.NotNull(result);

            result = ProfileController.HandleError();

            Assert.NotNull(result);
        }

        [Test]
        public void Error_ReturnsThrow()
        {
            var webHostEnvironment = new Mock<IWebHostEnvironment>();
            webHostEnvironment.Setup(x => x.EnvironmentName).Returns("Production");

            IExceptionHandlerFeature exceptionHandlerFeature = new ExceptionHandlerFeature { Error = new Exception() };

            IFeatureCollection features = new FeatureCollection();
            features.Set(exceptionHandlerFeature);

            var ProfileController = new ErrorController();
            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = new DefaultHttpContext(features);
            ProfileController.ControllerContext = controllerContext;

            ProfileController.Problem();
            try
            {
                var result = ProfileController.HandleErrorDevelopment(webHostEnvironment.Object);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<InvalidOperationException>(ex);
            }



        }
    }
}

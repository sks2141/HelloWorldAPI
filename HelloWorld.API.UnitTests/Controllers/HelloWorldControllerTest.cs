using HelloWorld.API.Controllers;
using HelloWorld.Services;
using HelloWorld.ViewModel;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HelloWorld.API.UnitTests
{
    public class HelloWorldControllerTest
    {
        private HelloWorldController controller;
        private Mock<IService> service;
        private Mock<ILogger<HelloWorldController>> logger;

        public HelloWorldControllerTest()
        {
            this.service = new Mock<IService>(MockBehavior.Strict);
            this.logger = LoggerUtils.LoggerMock<HelloWorldController>();
            
            this.controller = new HelloWorldController(this.service.Object, this.logger.Object);
        }

        // Simulate an exception thrown: The Get method will throw an exception, 
        // and will handled handled by ExceptionHandlingMiddleware, configured via Startup.cs
        [Fact]
        public void Get_ThrowsException()
        {
            this.service.Setup(i => i.GetResponses())
                .Throws<Exception>()
                .Verifiable();

            Assert.Throws<Exception>(() => this.controller.Get());
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("abc")]
        [InlineData(">")]
        [InlineData("<")]
        [InlineData("<>")]
        public void Get_With_InvalidParamValue(string paramValue)
        {
            var response = this.controller.Get(paramValue);

            Assert.Equal(string.Format("Did not find the message, linked to id:{0}", paramValue), response);
            this.service.Verify(i => i.GetResponses(), Times.Never);
        }

        [Fact]
        public void Get_With_NotFoundParamValue()
        {
            var responses = this.GetValidResponses();
            var notFoundId = (responses.Last().ResponseId + 100).ToString();
            this.service.Setup(i => i.GetResponses())
                .Returns(responses)
                .Verifiable();
            
            var response = this.controller.Get(notFoundId);

            Assert.Equal(string.Format("Did not find the message, linked to id:{0}", notFoundId), response);
            this.service.Verify(i => i.GetResponses(), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Get_With_ValidParamValue(string paramValue)
        {
            var responses = this.GetValidResponses();
            this.service.Setup(i => i.GetResponses())
                .Returns(responses)
                .Verifiable();

            var response = this.controller.Get(paramValue);

            Assert.NotEqual(string.Format("Did not find the message, linked to id:{0}", paramValue), response);
            Assert.Equal(responses.First().Message, response);
            this.service.Verify(i => i.GetResponses(), Times.Once);
        }

        [Fact]
        public void Get_With_FoundParamValue()
        {
            var responses = this.GetValidResponses();
            var foundId = (responses.Last().ResponseId).ToString();
            this.service.Setup(i => i.GetResponses())
                .Returns(responses)
                .Verifiable();
            
            var response = this.controller.Get(foundId);

            Assert.NotEqual(string.Format("Did not find the message, linked to id:{0}", foundId), response);
            Assert.Equal(responses.Last().Message, response);
            this.service.Verify(i => i.GetResponses(), Times.Once);
        }

        private IEnumerable<ResponseStatsViewModel> GetValidResponses()
        {
            return new List<ResponseStatsViewModel>
            {
                new ResponseStatsViewModel
                {
                    ResponseId = 0,
                    Message = "Hello World"
                },
                new ResponseStatsViewModel
                {
                    ResponseId = 1,
                    Message = "Hello Earth"
                },
                new ResponseStatsViewModel
                {
                    ResponseId = 2,
                    Message = "Aloha Hawaii"
                }
            };
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using swap_book.Controllers;
using swap_book.Models;
using swap_book.Services;
using Xunit;

namespace swap_book.Tests.Controllers
{
    public class MailControllerTests
    {
        [Fact]
        public void Feedback_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var emailSenderMock = new Mock<IEmailSender>();
            var controller = new MailController(emailSenderMock.Object, null);

            var validModel = new Mail
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Msg = "Test message"
            };

            // Act
            var result = controller.Feedback(validModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);

            // Verify that SendMail was called with the correct parameters
            emailSenderMock.Verify(x => x.SendMail(validModel.FirstName, validModel.LastName, validModel.Email, validModel.Msg), Times.Once);
        }

        [Fact]
        public void Feedback_InvalidModel_ReturnsErrorView()
        {
            // Arrange
            var emailSenderMock = new Mock<IEmailSender>();
            var controller = new MailController(emailSenderMock.Object, null);

            var invalidModel = new Mail();

            // Act
            controller.ModelState.AddModelError("Email", "Email is required");
            var result = controller.Feedback(invalidModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error", result.ViewName);
        }

        [Fact]
        public void Feedback_ExceptionThrown_LogsError()
        {
            // Arrange
            var emailSenderMock = new Mock<IEmailSender>();
            emailSenderMock.Setup(sender => sender.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Simulated exception"));

            var loggerMock = new Mock<ILogger<MailController>>();
            var mailController = new MailController(emailSenderMock.Object, loggerMock.Object);

            var validModel = new Mail
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Msg = "This is a valid message."
            };

            // Act
            var result = mailController.Feedback(validModel) as RedirectToActionResult;

            // Assert
            loggerMock.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<object[]>()), Times.AtLeastOnce);

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }



        [Fact]
        public void Feedback_NullModel_ReturnsErrorView()
        {
            // Arrange
            var emailSenderMock = new Mock<IEmailSender>();
            var loggerMock = new Mock<ILogger<MailController>>();
            var mailController = new MailController(emailSenderMock.Object, loggerMock.Object);

    
            var result = mailController.Feedback(null) as ViewResult;


            Assert.NotNull(result);
            Assert.Equal("Error", result.ViewName);
        }

    }
}

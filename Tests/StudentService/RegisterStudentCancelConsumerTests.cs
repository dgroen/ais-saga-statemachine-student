
using Events.StudentEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using StudentService.Consumers;
using StudentService.Services;

namespace Tests.StudentService
{
    public class RegisterStudentCancelConsumerTests
    {
        [Fact]
        public async Task Consume_ShouldLogSuccess_WhenStudentIsDeleted()
        {
            // Arrange
            var studentServicesMock = new Mock<IStudentServices>();
            var loggerMock = new Mock<ILogger<RegisterStudentCancelConsumer>>();
            var consumer = new RegisterStudentCancelConsumer(studentServicesMock.Object, loggerMock.Object);
            var contextMock = new Mock<ConsumeContext<ICancelRegisterStudentEvent>>();
            var eventMock = new Mock<ICancelRegisterStudentEvent>();
            eventMock.Setup(e => e.StudentId).Returns(new Guid("b3d85158-abf5-11ef-b762-5b5092244161"));
            contextMock.Setup(c => c.Message).Returns(eventMock.Object);
            studentServicesMock.Setup(s => s.DeleteStudent("b3d85158-abf5-11ef-b762-5b5092244161")).Returns(true);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            loggerMock.Verify(l => l.Log(
                LogLevel.Information, 
                It.IsAny<EventId>(), 
                It.Is<It.IsAnyType>((o, t) => o.ToString() == "The Student has been removed successfully"), 
                It.IsAny<Exception>(), 
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Consume_ShouldLogFailure_WhenStudentDeletionFails()
        {
            // Arrange
            var studentServicesMock = new Mock<IStudentServices>();
            var loggerMock = new Mock<ILogger<RegisterStudentCancelConsumer>>();
            var consumer = new RegisterStudentCancelConsumer(studentServicesMock.Object, loggerMock.Object);
            var contextMock = new Mock<ConsumeContext<ICancelRegisterStudentEvent>>();
            var eventMock = new Mock<ICancelRegisterStudentEvent>();
            eventMock.Setup(e => e.StudentId).Returns(new Guid("b3d85158-abf5-11ef-b762-5b5092244161"));
            contextMock.Setup(c => c.Message).Returns(eventMock.Object);
            studentServicesMock.Setup(s => s.DeleteStudent("5c515898-abf6-11ef-b116-dfd0e3a75dd8")).Returns(false);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            loggerMock.Verify(l => l.Log(
                LogLevel.Information, 
                It.IsAny<EventId>(), 
                It.Is<It.IsAnyType>((o, t) => o.ToString() == "Failed!!!"), 
                It.IsAny<Exception>(), 
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Consume_ShouldNotLog_WhenMessageIsNull()
        {
            // Arrange
            var studentServicesMock = new Mock<IStudentServices>();
            var loggerMock = new Mock<ILogger<RegisterStudentCancelConsumer>>();
            var consumer = new RegisterStudentCancelConsumer(studentServicesMock.Object, loggerMock.Object);
            var contextMock = new Mock<ConsumeContext<ICancelRegisterStudentEvent>>();
            contextMock.Setup(c => c.Message).Returns((ICancelRegisterStudentEvent)null);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            loggerMock.Verify(l => l.Log(
                LogLevel.Information, 
                It.IsAny<EventId>(), 
                It.Is<It.IsAnyType>((o, t) => o.ToString() == string.Empty), 
                It.IsAny<Exception>(), 
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
                Times.Never);
        }
    }
}

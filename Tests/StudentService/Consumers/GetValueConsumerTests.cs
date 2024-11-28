using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Events.StudentEvents;

namespace StudentService.Consumers.Tests
{
    public class GetValueConsumerTests
    {
        /// <summary>
        /// Tests the Consume method with a valid message.
        /// 
        /// It will verify that the Consume method publishes the IAddStudentEvent message
        /// to the message broker and logs an information message indicating that a
        /// message has been received.
        /// </summary>
        [Fact]
        public async Task Consume_ValidMessage_LogsInformationAndPublishesEvent()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetValueConsumer>>();
            var consumer = new GetValueConsumer(loggerMock.Object);
            var contextMock = new Mock<ConsumeContext<IGETValueEvent>>();
            var messageMock = new Mock<IGETValueEvent>();
            messageMock.SetupGet(m => m.StudentId).Returns(new Guid());
            messageMock.SetupGet(m => m.Title).Returns("Mr.");
            messageMock.SetupGet(m => m.Email).Returns("test@example.com");
            messageMock.SetupGet(m => m.RequireDate).Returns(DateTime.Now);
            messageMock.SetupGet(m => m.Age).Returns(20);
            messageMock.SetupGet(m => m.Location).Returns("New York");
            contextMock.SetupGet(c => c.Message).Returns(messageMock.Object);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            contextMock.Verify(c => c.Publish<IAddStudentEvent>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
            loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == "a message has been received"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Tests the Consume method with a null message.
        /// 
        /// It will verify that the Consume method does not publish the IAddStudentEvent
        /// message to the message broker and does not log an information message.
        /// </summary>
        [Fact]
        public async Task Consume_NullMessage_DoesNotLogOrPublish()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetValueConsumer>>();
            var consumer = new GetValueConsumer(loggerMock.Object);
            var contextMock = new Mock<ConsumeContext<IGETValueEvent>>();
            contextMock.SetupGet(c => c.Message).Returns((IGETValueEvent)null);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            contextMock.Verify(c => c.Publish<IAddStudentEvent>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);

            loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == string.Empty),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);
        }

        /// <summary>
        /// Tests the Consume method with a message that has empty fields.
        /// 
        /// It verifies that the Consume method publishes the IAddStudentEvent message
        /// to the message broker and logs an information message indicating that a
        /// message has been received, even when the message fields are empty.
        /// </summary>
        [Fact]
        public async Task Consume_MessageWithEmptyFields_LogsInformationAndPublishesEvent()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetValueConsumer>>();
            var consumer = new GetValueConsumer(loggerMock.Object);
            var contextMock = new Mock<ConsumeContext<IGETValueEvent>>();
            var messageMock = new Mock<IGETValueEvent>();
            messageMock.SetupGet(m => m.StudentId).Returns(new Guid());
            messageMock.SetupGet(m => m.Title).Returns(string.Empty);
            messageMock.SetupGet(m => m.Email).Returns(string.Empty);
            messageMock.SetupGet(m => m.RequireDate).Returns(DateTime.Now);
            messageMock.SetupGet(m => m.Age).Returns(0);
            messageMock.SetupGet(m => m.Location).Returns(string.Empty);
            contextMock.SetupGet(c => c.Message).Returns(messageMock.Object);

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            contextMock.Verify(c => c.Publish<IAddStudentEvent>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);

            loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == "a message has been received"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }
    }
}

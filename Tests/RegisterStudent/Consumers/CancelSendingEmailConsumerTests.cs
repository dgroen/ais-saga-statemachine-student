using Moq;
using MassTransit;
using Microsoft.Extensions.Logging;
using RegisterStudent.Services;
using Events.SendEmailEvents;
using Events.StudentEvents;

namespace RegisterStudent.Consumers.Tests;
public class CancelSendingEmailConsumerTests
{
    private readonly Mock<IStudentInfoService> _studentInfoServiceMock;
    private readonly Mock<ILogger<CancelSendingEmailConsumer>> _loggerMock;
    private readonly CancelSendingEmailConsumer _consumer;

    /// <summary>
    /// Tests the CancelSendingEmailConsumer class.
    /// </summary>
    public CancelSendingEmailConsumerTests()
    {
        _studentInfoServiceMock = new Mock<IStudentInfoService>();
        _loggerMock = new Mock<ILogger<CancelSendingEmailConsumer>>();
        _consumer = new CancelSendingEmailConsumer(_studentInfoServiceMock.Object, _loggerMock.Object);
    }

    /// <summary>
    /// Verifies that the <see cref="CancelSendingEmailConsumer.Consume"/> method
    /// successfully publishes the ICancelRegisterStudentEvent when the student's
    /// information is removed successfully from the service.
    /// 
    /// It checks that the <see cref="ICancelRegisterStudentEvent"/> is published
    /// once and logs the appropriate information message indicating the event has been sent.
    /// </summary>
    [Fact]
    public async Task Consume_ShouldPublishCancelRegisterStudentEvent_WhenStudentInfoRemovedSuccessfully()
    {
        // Arrange
        var contextMock = new Mock<ConsumeContext<ICancelSendEmailEvent>>();
        var messageMock = new Mock<ICancelSendEmailEvent>();
        messageMock.SetupGet(m => m.StudentId).Returns(new Guid());
        messageMock.SetupGet(m => m.Title).Returns("Mr.");
        messageMock.SetupGet(m => m.Email).Returns("test@example.com");
        messageMock.SetupGet(m => m.RequireDate).Returns(DateTime.Now);
        messageMock.SetupGet(m => m.Age).Returns(20);
        messageMock.SetupGet(m => m.Location).Returns("Test Location");
        contextMock.SetupGet(c => c.Message).Returns(messageMock.Object);

        _studentInfoServiceMock.Setup(s => s.RemoveStudentInfo(It.IsAny<string>())).Returns(true);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert
        contextMock.Verify(c => c.Publish<ICancelRegisterStudentEvent>(It.IsAny<object>(), default), Times.Once);

        _loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString() == "The message has been sent to the ICancelRegisterStudentEvent in the StudentService"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies that the <see cref="CancelSendingEmailConsumer.Consume"/> method does not
    /// publish the <see cref="ICancelRegisterStudentEvent"/> when the student's
    /// information removal fails from the service.
    /// 
    /// It checks that the <see cref="ICancelRegisterStudentEvent"/> is not published at all
    /// and logs the appropriate information message indicating the event has not been sent.
    /// </summary>
    [Fact]
    public async Task Consume_ShouldNotPublishEvent_WhenStudentInfoRemovalFails()
    {
        // Arrange
        var contextMock = new Mock<ConsumeContext<ICancelSendEmailEvent>>();
        var messageMock = new Mock<ICancelSendEmailEvent>();
        messageMock.SetupGet(m => m.StudentId).Returns(new Guid());
        contextMock.SetupGet(c => c.Message).Returns(messageMock.Object);

        _studentInfoServiceMock.Setup(s => s.RemoveStudentInfo(It.IsAny<string>())).Returns(false);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert
        contextMock.Verify(c => c.Publish<ICancelRegisterStudentEvent>(It.IsAny<object>(), default), Times.Never);
        _loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString() == "Failed!!!"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies that the <see cref="CancelSendingEmailConsumer.Consume"/> method does not
    /// publish the <see cref="ICancelRegisterStudentEvent"/> when the message to be consumed
    /// is null.
    /// 
    /// It checks that the <see cref="ICancelRegisterStudentEvent"/> is not published at all
    /// and logs no information message indicating the event has not been sent.
    /// </summary>
    [Fact]
    public async Task Consume_ShouldNotPublishEvent_WhenMessageIsNull()
    {
        // Arrange
        var contextMock = new Mock<ConsumeContext<ICancelSendEmailEvent>>();
        contextMock.SetupGet(c => c.Message).Returns((ICancelSendEmailEvent)null);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert
        contextMock.Verify(c => c.Publish<ICancelRegisterStudentEvent>(It.IsAny<object>(), default), Times.Never);

        _loggerMock.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) => o.ToString() == It.IsAny<string>()),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }
}
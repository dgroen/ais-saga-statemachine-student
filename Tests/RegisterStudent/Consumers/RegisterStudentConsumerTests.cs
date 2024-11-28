using AutoMapper;
using Events.SendEmailEvents;
using Events.StudentEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using RegisterStudent.Mapping;
using RegisterStudent.Models;
using RegisterStudent.Services;

namespace RegisterStudent.Consumers.Tests
{
    public class RegisterStudentConsumerTests
    {
        private readonly Mock<IStudentInfoService> _studentInfoServiceMock;
        private readonly Mock<ILogger<RegisterStudentConsumer>> _loggerMock;
        private readonly Mock<IRegisterStudentEvent> _studentEventMock;
        private readonly Mock<ConsumeContext<IRegisterStudentEvent>> _contextMock;
        private readonly RegisterStudentConsumer _consumer;
        private readonly Mock<AppDbContext> _dbContextMock;
        private readonly StudentInfoService _studentInfoService;
        private readonly IMapper _mapper;


        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterStudentConsumerTests"/> class.
        /// Sets up the necessary mocks and dependencies required for testing the
        /// <see cref="RegisterStudentConsumer"/> class.
        /// </summary>
        /// <remarks>
        /// The constructor initializes mocks for <see cref="IStudentInfoService"/>,
        /// <see cref="ILogger{T}"/>, and <see cref="IRegisterStudentEvent"/>.
        /// It also sets up a mock <see cref="ConsumeContext{T}"/> to simulate message consumption
        /// and configures AutoMapper with the <see cref="StudentInfoMapping"/> profile.
        /// </remarks>
        public RegisterStudentConsumerTests()
        {
            _dbContextMock = new Mock<AppDbContext>();
            _studentInfoService = new StudentInfoService(_dbContextMock.Object);

            _studentInfoServiceMock = new Mock<IStudentInfoService>();
            _loggerMock = new Mock<ILogger<RegisterStudentConsumer>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<StudentInfoMapping>());
            _mapper = config.CreateMapper();
            _consumer = new RegisterStudentConsumer(_studentInfoServiceMock.Object,
                            _loggerMock.Object,
                            _mapper);
            _studentEventMock = new Mock<IRegisterStudentEvent>();
            _studentEventMock.SetupGet(e => e.Age).Returns(25);
            _studentEventMock.SetupGet(e => e.Email).Returns("valid@example.com");
            _studentEventMock.SetupGet(e => e.StudentId).Returns(new Guid("c34f1f4e-acb4-11ef-bb81-3333bc6b0202"));
            _studentEventMock.SetupGet(e => e.Title).Returns("Mr.");
            _studentEventMock.SetupGet(e => e.RequireDate).Returns(System.DateTime.Now);
            _studentEventMock.SetupGet(e => e.Location).Returns("Location");

            _contextMock = new Mock<ConsumeContext<IRegisterStudentEvent>>();
            _contextMock.SetupGet(c => c.Message).Returns(_studentEventMock.Object);
        }

        /// <summary>
        /// Tests the Consume method with valid data to verify that it publishes a
        /// <see cref="ISendEmailEvent"/> and logs an information message.
        /// </summary>
        /// <remarks>
        /// The test sets up a mock <see cref="ConsumeContext{T}"/> with a valid
        /// <see cref="IRegisterStudentEvent"/> and configures the
        /// <see cref="IStudentInfoService"/> to return a new <see cref="StudentInfo"/>
        /// with a valid <c>StudentNumber</c>.
        /// 
        /// It then calls the Consume method and verifies that the
        /// <see cref="ISendEmailEvent"/> is published and an information message is logged.
        /// </remarks>
        [Fact]
        public async Task Consume_ValidData_PublishesSendEmailEvent()
        {
            // Arrange
            _contextMock.SetupGet(c => c.Message).Returns(_studentEventMock.Object);
            var studentInfo = new StudentInfo();
            _studentInfoServiceMock.Setup(s => s.AddStudentInfo(It.IsAny<StudentInfo>()))
                .ReturnsAsync(new StudentInfo { StudentNumber = "c34f1f4e-acb4-11ef-bb81-3333bc6b0202" });

            // Act
            await _consumer.Consume(_contextMock.Object);

            // Assert
            _contextMock.Verify(c => c.Publish<ISendEmailEvent>(
                It.IsAny<object>(),
                default),
                Times.Once);

            _loggerMock.Verify(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception,
                string>)It.IsAny<object>()),
                Times.Once
            );
        }

        /// <summary>
        /// Verifies that the <see cref="RegisterStudentConsumer.Consume"/> method
        /// publishes the <see cref="ICancelRegisterStudentEvent"/> when the age
        /// in the <see cref="IRegisterStudentEvent"/> is greater than 80.
        /// 
        /// It checks that the <see cref="ICancelRegisterStudentEvent"/> is published
        /// once and logs the appropriate information message indicating the event has been sent.
        /// </summary>
        [Fact]
        public async Task Consume_ShouldPublishCancelRegisterStudentEvent_WhenAgeIsGreaterThan80()
        {
            // Arrange
            _studentEventMock.SetupGet(e => e.Age).Returns(81);
            _contextMock.SetupGet(c => c.Message).Returns(_studentEventMock.Object);

            // Act
            await _consumer.Consume(_contextMock.Object);

            // Assert
            _contextMock.Verify(c => c.Publish<ICancelRegisterStudentEvent>(It.IsAny<object>(), default), Times.Once);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == $"Message cancelled== StudentId is c34f1f4e-acb4-11ef-bb81-3333bc6b0202"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that the <see cref="RegisterStudentConsumer.Consume"/> method
        /// publishes the <see cref="ISendEmailEvent"/> when the age in the
        /// <see cref="IRegisterStudentEvent"/> is 80.
        /// 
        /// It checks that the <see cref="ISendEmailEvent"/> is published once
        /// and logs the appropriate information message indicating the event has been sent.
        /// </summary>
        [Fact]
        public async Task Consume_ShouldPublishSendMailEvent_WhenAgeIs80()
        {
            // Arrange
            _studentEventMock.SetupGet(e => e.Age).Returns(80);
            _studentEventMock.SetupGet(e => e.StudentNumber).Returns("c34f1f4e-acb4-11ef-bb81-3333bc6b0202");
            _contextMock.SetupGet(c => c.Message).Returns(_studentEventMock.Object);
            var studentInfo = new StudentInfo();
            _studentInfoServiceMock.Setup(s => s.AddStudentInfo(It.IsAny<StudentInfo>()))
                .ReturnsAsync(new StudentInfo
                {
                    StudentId = "c34f1f4e-acb4-11ef-bb81-3333bc6b0202"
                });

            // Act
            await _consumer.Consume(_contextMock.Object);

            // Assert
            _contextMock.Verify(c => c.Publish<ISendEmailEvent>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == $"Message sent == StudentId is c34f1f4e-acb4-11ef-bb81-3333bc6b0202"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that the <see cref="RegisterStudentConsumer.Consume"/> method
        /// publishes the <see cref="ICancelRegisterStudentEvent"/> when the email
        /// in the <see cref="IRegisterStudentEvent"/> is invalid.
        /// 
        /// It checks that the <see cref="ICancelRegisterStudentEvent"/> is published
        /// once and logs the appropriate information message indicating the event has been sent.
        /// </summary>
        [Fact]
        public async Task Consume_ShouldPublishCancelRegisterStudentEvent_WhenEmailIsInvalid()
        {
            // Arrange
            _studentEventMock.SetupGet(e => e.Email).Returns("invalid@example");
            _contextMock.SetupGet(c => c.Message).Returns(_studentEventMock.Object);
            var studentInfo = new StudentInfo();
            _studentInfoServiceMock.Setup(s => s.AddStudentInfo(It.IsAny<StudentInfo>()))
                .ReturnsAsync(new StudentInfo
                {
                    StudentId = "c34f1f4e-acb4-11ef-bb81-3333bc6b0202"
                });

            // Act
            await _consumer.Consume(_contextMock.Object);

            // Assert
            _contextMock.Verify(c => c.Publish<ICancelRegisterStudentEvent>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == $"Message cancelled== StudentId is c34f1f4e-acb4-11ef-bb81-3333bc6b0202"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that the <see cref="RegisterStudentConsumer.Consume"/> method
        /// publishes the <see cref="ICancelRegisterStudentEvent"/> when the
        /// <see cref="IStudentInfoService.AddStudentInfo"/> method fails.
        /// 
        /// It checks that the <see cref="ICancelRegisterStudentEvent"/> is published
        /// once and logs the appropriate information message indicating the event has been sent.
        /// </summary>
        [Fact]
        public async Task Consume_ShouldPublishCancelRegisterStudentEvent_WhenAddStudentInfoFails()
        {
            // Arrange

            _contextMock.SetupGet(c => c.Message).Returns(_studentEventMock.Object);

            _studentInfoServiceMock.Setup(s => s.AddStudentInfo(It.IsAny<StudentInfo>())).ReturnsAsync((StudentInfo)null);

            // Act
            await _consumer.Consume(_contextMock.Object);

            // Assert
            _contextMock.Verify(c => c.Publish<ICancelRegisterStudentEvent>(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == $"Message cancelled== StudentId is c34f1f4e-acb4-11ef-bb81-3333bc6b0202"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that the <see cref="RegisterStudentConsumer.Consume"/> method logs an error message when the message to be consumed is null.
        /// 
        /// It checks that an information log with the message "The message is null" is created once.
        /// </summary>
        [Fact]
        public async Task Consume_ShouldLogError_WhenMessageIsNull()
        {
            // Arrange
            _contextMock.SetupGet(c => c.Message).Returns((IRegisterStudentEvent)null);

            // Act
            await _consumer.Consume(_contextMock.Object);

            // Assert
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == "The message is null"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that the <see cref="RegisterStudentConsumer.Consume"/> method logs an error message
        /// when an exception is thrown during the process of adding student information.
        /// 
        /// It checks that an information log with the appropriate error message is created once,
        /// indicating that an exception occurred while calling the StudentInfoService.
        /// </summary>
        [Fact]
        public async Task Consume_ShouldLogError_WhenExceptionIsThrown()
        {
            // Arrange
            _studentInfoServiceMock.Setup(s => s.AddStudentInfo(It.IsAny<StudentInfo>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            await _consumer.Consume(_contextMock.Object);

            // Assert
            _loggerMock.Verify(l => l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString() == $"Exception while calling the StudentInfoService: StudentId is c34f1f4e-acb4-11ef-bb81-3333bc6b0202"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}

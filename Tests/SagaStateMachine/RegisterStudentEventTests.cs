
namespace SagaStateMachine.Tests
{
    public class RegisterStudentEventTests
    {
        [Fact]
        public void Constructor_SetsStudentStateDataFieldCorrectly()
        {
            // Arrange
            var studentStateData = new StudentStateData();

            // Act
            var registerStudentEvent = new RegisterStudentEvent(studentStateData);

            // Assert
            Assert.Equal(studentStateData, registerStudentEvent.StudentStateData);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenStudentStateDataIsNull()
        {
            // Act and Assert
            Assert.Throws<ArgumentNullException>(() => new RegisterStudentEvent(null));
        }
    }
}
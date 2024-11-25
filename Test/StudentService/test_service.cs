using Xunit;
using Moq;
using System.Threading.Tasks;
using AisSagaStateMachine_Student.Services;
using AisSagaStateMachine_Student.Controllers;
using AisSagaStateMachine_Student.Models;
using Microsoft.AspNetCore.Mvc;

public class StudentControllerTests
{
    private readonly Mock<IStudentServices> _mockStudentServices;
    private readonly StudentController _studentController;

    public StudentControllerTests()
    {
        _mockStudentServices = new Mock<IStudentServices>();
        _studentController = new StudentController(_mockStudentServices.Object);
    }

    [Fact]
    public async Task AddStudent_ShouldReturnOk_WhenStudentIsAddedSuccessfully()
    {
        // Arrange
        var studentDto = new AddStudentDTO { /* populate with valid data */ };
        

        var student = new Student { /* populate with valid data */ };
        _mockStudentServices.Setup(s => s.AddStudent(It.IsAny<Student>())).ReturnsAsync(student);

        // Act
        var result = await _studentController.AddStudent(studentDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(student, okResult.Value);
    }

    [Fact]
    public async Task AddStudent_ShouldReturnBadRequest_WhenStudentServiceThrowsException()
    {
        // Arrange
        var studentDto = new AddStudentDTO { /* populate with valid data */ };
        _mockStudentServices.Setup(s => s.AddStudent(It.IsAny<Student>())).ThrowsAsync(new Exception("Error"));

        // Act
        var result = await _studentController.AddStudent(studentDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task AddStudent_ShouldReturnBadRequest_WhenStudentDtoIsInvalid()
    {
        // Arrange
        var studentDto = new AddStudentDTO { /* populate with invalid data */ };

        // Act
        var result = await _studentController.AddStudent(studentDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteStudent_ShouldReturnTrue_WhenStudentIsDeletedSuccessfully()
    {
        // Arrange
        var studentId = "valid-student-id";
        _mockStudentServices.Setup(s => s.DeleteStudent(studentId)).Returns(true);

        // Act
        var result = _studentController.DeleteStudent(studentId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteStudent_ShouldReturnFalse_WhenStudentDoesNotExist()
    {
        // Arrange
        var studentId = "non-existent-student-id";
        _mockStudentServices.Setup(s => s.DeleteStudent(studentId)).Returns(false);

        // Act
        var result = _studentController.DeleteStudent(studentId);

        // Assert
        Assert.False(result);
    }
}

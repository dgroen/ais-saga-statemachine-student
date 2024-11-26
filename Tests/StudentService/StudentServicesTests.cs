using Xunit;
using Moq;
using StudentService.Services;
using StudentService.Models;
using AutoFixture;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tests.Helpers;

namespace StudentService.Tests.Services;
public class StudentServicesTests
{
    private  Mock<AppDbContext> _dbContextMock;
    private StudentServices _studentServices;
    private Student _newStudent;

    public StudentServicesTests()
    {
        _dbContextMock = new Mock<AppDbContext>();
        _studentServices = new StudentServices(_dbContextMock.Object);

        var fixture = new Fixture();

        _newStudent = fixture.Build<Student>()
        .With(x => x.StudentId, "f016ae22-ac03-11ef-b12b-c3258b7903c6")
        .With(x => x.Age, 20)
        .With(x => x.Location,"Utrecht")
        .With(x => x.Email, "OgB4a@example.com")
        .Create(); 
    }

    [Fact]
    public async Task AddStudent_ShouldAddStudent_WhenStudentIsValid()
    {
        // Arrange
        _dbContextMock.Setup(x => x.Student)
        .ReturnsDbSet(StudentTestDataHelper.GetFakeStudent(_newStudent));
        
        // Act
        var result = await _studentServices.AddStudent(_newStudent);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Student>(result);
        Assert.Equal(_newStudent, result);
       
    }


    [Fact]
    public async Task AddStudent_ShouldThrowArgumentNullException_WhenStudentIsNull()
    {
        // Arrange
        Student student = null;
        var studentServices = new StudentServices(new Mock<AppDbContext>().Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => studentServices.AddStudent(student));
    }

    [Fact]
    public async Task AddStudent_ShouldThrowApplicationException_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        _dbContextMock.Setup(x => x.Student.AddAsync(_newStudent, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Microsoft.EntityFrameworkCore.DbUpdateException());

      // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _studentServices.AddStudent(_newStudent));
    }

    [Fact]
    public async Task AddStudent_ShouldThrowApplicationException_WhenOtherExceptionOccurs()
    {
        // Arrange
        _dbContextMock.Setup(x => x.Student.AddAsync(_newStudent, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _studentServices.AddStudent(_newStudent));
    }




    // [Fact]
    // public void DeleteStudent_ShouldDeleteStudent_WhenStudentExists()
    // {
    //     // Arrange
    //     var studentId = "f016ae22-ac03-11ef-b12b-c3258b7903c6";
    //     // _studentRepositoryMock.Setup(repo => repo.DeleteStudent(studentId)).Returns(true);

    //     // Act
    //     var result = _studentServices.DeleteStudent(studentId);

    //     // Assert
    //     Assert.True(result);
    //     // _studentRepositoryMock.Verify(repo => repo.DeleteStudent(studentId), Times.Once);
    // }

//     [Fact]
//     public void DeleteStudent_ShouldReturnFalse_WhenStudentDoesNotExist()
//     {
//         // Arrange
//         var studentId = 2;
//         _studentRepositoryMock.Setup(repo => repo.DeleteStudent(studentId)).Returns(false);

//         // Act
//         var result = _studentServices.DeleteStudent(studentId);

//         // Assert
//         Assert.False(result);
//     }
}


internal interface IStudentRepository
{
    void AddStudent(Student newStudent);
}


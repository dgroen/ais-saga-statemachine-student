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

    public StudentServicesTests()
    {
        _dbContextMock = new Mock<AppDbContext>();
        _studentServices = new StudentServices(_dbContextMock.Object);
    }

    [Fact]
    public async void AddStudent_ShouldAddStudent_WhenStudentIsValid()
    {
        // Arrange
        var fixture = new Fixture();
        var newStudent = fixture.Build<Student>()
        .With(x => x.StudentId, "f016ae22-ac03-11ef-b12b-c3258b7903c6")
        .With(x => x.Age, 20)
        .With(x => x.Location,"Utrecht")
        .With(x => x.Email, "OgB4a@example.com")
        .Create(); 

        _dbContextMock.Setup(x => x.Student)
        .ReturnsDbSet(StudentTestDataHelper.GetFakeStudent(newStudent));
        
        // Act
        var result = await _studentServices.AddStudent(newStudent);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Student>(result);
        Assert.Equal(newStudent, result);
       
    }

//     [Fact]
//     public void AddStudent_ShouldReturnFalse_WhenStudentIsNull()
//     {
//         // Act
//         var result = _studentServices.AddStudent(null);

//         // Assert
//         Assert.False(result);
//     }


//     [Fact]
//     public void DeleteStudent_ShouldDeleteStudent_WhenStudentExists()
//     {
//         // Arrange
//         var studentId = 1;
//         _studentRepositoryMock.Setup(repo => repo.DeleteStudent(studentId)).Returns(true);

//         // Act
//         var result = _studentServices.DeleteStudent(studentId);

//         // Assert
//         Assert.True(result);
//         _studentRepositoryMock.Verify(repo => repo.DeleteStudent(studentId), Times.Once);
//     }

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


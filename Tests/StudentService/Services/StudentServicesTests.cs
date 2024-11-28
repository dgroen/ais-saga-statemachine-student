using Moq;
using StudentService.Models;
using AutoFixture;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tests.Helpers;

namespace StudentService.Services.Tests;
public class StudentServicesTests
{
    private Mock<AppDbContext> _dbContextMock;
    private StudentServices _studentServices;
    private Student _newStudent;

    /// <summary>
    /// Tests the StudentServices class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The constructor sets up a mock of the AppDbContext and creates an instance of StudentServices
    /// with it. It then uses AutoFixture to create a new Student object with values for Age, Location,
    /// and Email.
    /// </para>
    /// </remarks>
    public StudentServicesTests()
    {
        _dbContextMock = new Mock<AppDbContext>();
        _studentServices = new StudentServices(_dbContextMock.Object);

        var fixture = new Fixture();

        _newStudent = fixture.Build<Student>()
        .With(x => x.StudentId, "f016ae22-ac03-11ef-b12b-c3258b7903c6")
        .With(x => x.Age, 20)
        .With(x => x.Location, "Utrecht")
        .With(x => x.Email, "OgB4a@example.com")
        .Create();
    }

    /// <summary>
    /// Tests the AddStudent method.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The test sets up a mock of the AppDbContext and creates a fake Student object with
    /// values for Age, Location, and Email. It then calls the AddStudent method with the
    /// fake Student and checks that it returns a Student that matches the input.
    /// </para>
    /// </remarks>
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

    /// <summary>
    /// Tests that AddStudent throws an ArgumentNullException when the Student is null.
    /// </summary>
    [Fact]
    public async Task AddStudent_ShouldThrowArgumentNullException_WhenStudentIsNull()
    {
        // Arrange
        Student student = null;
        var studentServices = new StudentServices(new Mock<AppDbContext>().Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => studentServices.AddStudent(student));
    }

    /// <summary>
    /// Verifies that the <see cref="AddStudent"/> method throws an <see cref="ApplicationException"/> 
    /// when a <see cref="Microsoft.EntityFrameworkCore.DbUpdateException"/> occurs during the database update.
    /// </summary>
    [Fact]
    public async Task AddStudent_ShouldThrowApplicationException_WhenDbUpdateExceptionOccurs()
    {
        // Arrange
        _dbContextMock.Setup(x => x.Student.AddAsync(_newStudent, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Microsoft.EntityFrameworkCore.DbUpdateException());

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _studentServices.AddStudent(_newStudent));
    }

    /// <summary>
    /// Verifies that the <see cref="AddStudent"/> method throws an <see cref="ApplicationException"/> 
    /// when an unexpected <see cref="Exception"/> occurs during the database operation.
    /// </summary>
    [Fact]
    public async Task AddStudent_ShouldThrowApplicationException_WhenOtherExceptionOccurs()
    {
        // Arrange
        _dbContextMock.Setup(x => x.Student.AddAsync(_newStudent, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _studentServices.AddStudent(_newStudent));
    }


    /// <summary>
    /// Verifies that the <see cref="DeleteStudent"/> method deletes a student when the student exists in the database.
    /// </summary>
    [Fact]
    public async Task DeleteStudent_ShouldDeleteStudent_WhenStudentExists()
    {
        // Arrange
        var studentData = new List<Student> { _newStudent }.AsQueryable();
        var mockDbSet = new Mock<DbSet<Student>>();
        mockDbSet.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(studentData.Provider);
        mockDbSet.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(studentData.Expression);
        mockDbSet.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(studentData.ElementType);
        mockDbSet.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(() => studentData.GetEnumerator());
        _dbContextMock.Setup(x => x.Student).Returns(mockDbSet.Object);

        // Act
        var result = _studentServices.DeleteStudent(_newStudent.StudentId);

        // Assert
        _dbContextMock.Verify(x => x.Student.Remove(It.IsAny<Student>()), Times.Once);
        _dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        Assert.True(result);
    }

    /// <summary>
    /// Verifies that the <see cref="DeleteStudent"/> method returns false when the student does not exist in the database.
    /// </summary>
    [Fact]
    public async Task DeleteStudent_ShouldReturnFalse_WhenStudentDoesNotExist()
    {
        // Arrange
        var studentData = new List<Student> { _newStudent }.AsQueryable();
        var mockDbSet = new Mock<DbSet<Student>>();
        mockDbSet.As<IQueryable<Student>>().Setup(m => m.Provider).Returns(studentData.Provider);
        mockDbSet.As<IQueryable<Student>>().Setup(m => m.Expression).Returns(studentData.Expression);
        mockDbSet.As<IQueryable<Student>>().Setup(m => m.ElementType).Returns(studentData.ElementType);
        mockDbSet.As<IQueryable<Student>>().Setup(m => m.GetEnumerator()).Returns(() => studentData.GetEnumerator());
        _dbContextMock.Setup(x => x.Student).Returns(mockDbSet.Object);

        // Act
        var result = _studentServices.DeleteStudent("689ddf06-ac3a-11ef-82fc-2b7754d90f17");
        // Assert
        _dbContextMock.Verify(x => x.Student.Remove(It.IsAny<Student>()), Times.Never);
        _dbContextMock.Verify(x => x.SaveChanges(), Times.Never);
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that the <see cref="DeleteStudent"/> method throws <see cref="ArgumentException"/> when the StudentId is null.
    /// </summary>
    [Fact]
    public void DeleteStudent_ShouldThrowArgumentException_WhenStudentIdIsNull()
    {
        // Arrange
        string StudentId = null;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _studentServices.DeleteStudent(StudentId));
    }

    /// <summary>
    /// Verifies that the <see cref="DeleteStudent"/> method throws <see cref="ArgumentException"/> when the StudentId is empty.
    /// </summary>
    [Fact]
    public void DeleteStudent_ShouldThrowArgumentException_WhenStudentIdIsEmpty()
    {
        // Arrange
        string emptyStudentId = string.Empty;
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _studentServices.DeleteStudent(emptyStudentId));
    }
}

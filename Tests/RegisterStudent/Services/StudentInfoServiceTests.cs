using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using RegisterStudent.Models;
using Tests.Helpers;

namespace RegisterStudent.Services.Tests
{
    public class StudentInfoServiceTests
    {
        private readonly Mock<AppDbContext> _dbContextMock;
        private readonly StudentInfoService _studentInfoService;
        private readonly StudentInfo _newStudentInfo;

        /// <summary>
        /// Tests for the <see cref="StudentInfoService"/> class.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The tests in this class check that the <see cref="StudentInfoService"/> class
        /// behaves correctly. The tests use a mock of the <see cref="AppDbContext"/> to
        /// isolate the <see cref="StudentInfoService"/> class from the database.
        /// </para>
        /// </remarks>
        public StudentInfoServiceTests()
        {
            _dbContextMock = new Mock<AppDbContext>();
            _studentInfoService = new StudentInfoService(_dbContextMock.Object);
            var fixture = new Fixture();
            _newStudentInfo = fixture.Build<StudentInfo>()
                .With(x => x.StudentId, "f016ae22-ac03-11ef-b12b-c3258b7903c6")
                .With(x => x.StudentNumber, "ABCDEFGHIJ")
                .With(x => x.Email, "OgB4a@example.com")
                .Create();
        }

        /// <summary>
        /// Verifies that the <see cref="StudentInfoService"/> constructor
        /// correctly sets the <c>_dbContext</c> field to the provided
        /// <see cref="AppDbContext"/> instance.
        /// </summary>
        /// <remarks>
        /// This test ensures that the constructor assigns the <c>_dbContext</c>
        /// field to the same instance of <see cref="AppDbContext"/> passed
        /// during the instantiation of <see cref="StudentInfoService"/>.
        /// </remarks>
        [Fact]
        public void Constructor_Sets_DbContext_Field_Correctly()
        {
            // Arrange
            _dbContextMock.Setup(x => x.StudentInfo)
             .ReturnsDbSet(StudentInfoTestDataHelper.GetFakeStudentInfo(_newStudentInfo));

            // Act
            var studentInfoService = new StudentInfoService(_dbContextMock.Object);
            // Assert
            Assert.Same(_dbContextMock.Object, studentInfoService.DbContext);
        }

        /// <summary>
        /// Verifies that the <see cref="StudentInfoService.AddStudentInfo"/> method
        /// correctly adds a new <see cref="StudentInfo"/> to the database and
        /// returns the added <see cref="StudentInfo"/>.
        /// </summary>
        /// <remarks>
        /// This test ensures that the <see cref="StudentInfoService.AddStudentInfo"/> method
        /// correctly adds a new <see cref="StudentInfo"/> to the database and returns the
        /// added <see cref="StudentInfo"/>. The test uses a mock of the
        /// <see cref="AppDbContext"/> to isolate the <see cref="StudentInfoService"/> class
        /// from the database.
        /// </remarks>
        [Fact]
        public async Task AddStudentInfo_ValidStudentInfo_ReturnsStudentInfo()
        {
            // Arrange
            _dbContextMock.Setup(x => x.StudentInfo)
             .ReturnsDbSet(StudentInfoTestDataHelper.GetFakeStudentInfo(_newStudentInfo));

            // Act
            var result = await _studentInfoService.AddStudentInfo(_newStudentInfo);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<StudentInfo>(result);
        }

        /// <summary>
        /// Verifies that the <see cref="StudentInfoService.AddStudentInfo"/> method
        /// correctly generates a student number when a valid <see cref="StudentInfo"/> is added.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This test ensures that the <see cref="StudentInfoService.AddStudentInfo"/> method
        /// assigns a non-empty student number to the added <see cref="StudentInfo"/> object.
        /// It uses a mock of the <see cref="AppDbContext"/> to isolate the service from the actual database.
        /// </para>
        /// </remarks>
        [Fact]
        public async Task AddStudentInfo_ValidStudentInfo_GeneratesStudentNumber()
        {
            // Arrange
            _dbContextMock.Setup(x => x.StudentInfo)
             .ReturnsDbSet(StudentInfoTestDataHelper.GetFakeStudentInfo(_newStudentInfo));
            var studentInfo = new StudentInfo();

            // Act
            var result = await _studentInfoService.AddStudentInfo(studentInfo);
            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.StudentNumber);
        }


        /// <summary>
        /// Verifies that the <see cref="StudentInfoService.AddStudentInfo"/> method adds the
        /// <see cref="StudentInfo"/> to the database.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This test ensures that the <see cref="StudentInfoService.AddStudentInfo"/> method
        /// correctly adds the added <see cref="StudentInfo"/> to the database. It uses a mock
        /// of the <see cref="AppDbContext"/> to isolate the service from the actual database.
        /// </para>
        /// </remarks>
        [Fact]
        public async Task AddStudentInfo_ValidStudentInfo_AddsToDatabase()
        {
            // Arrange
            _dbContextMock.Setup(x => x.StudentInfo)
                 .ReturnsDbSet(StudentInfoTestDataHelper.GetFakeStudentInfo(_newStudentInfo));

            // Act
            var result = await _studentInfoService.AddStudentInfo(_newStudentInfo);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<StudentInfo>(result);
            Assert.Equal(_newStudentInfo, result);
        }

        /// <summary>
        /// Verifies that the <see cref="StudentInfoService.AddStudentInfo"/> method
        /// throws an <see cref="ArgumentNullException"/> when a null input is provided.
        /// </summary>
        /// <remarks>
        /// This test ensures that the method handles null input by throwing the appropriate 
        /// exception, thereby preventing null references and maintaining data integrity.
        /// </remarks>
        [Fact]
        public async Task AddStudentInfo_NullInput_ThrowsArgumentNullException()
        {
            // Arrange
            var service = new StudentInfoService(new Mock<AppDbContext>().Object);
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddStudentInfo(null));
        }


        /// <summary>
        /// Verifies that the <see cref="StudentInfoService.AddStudentInfo"/> method
        /// throws an <see cref="InvalidOperationException"/> when the database save
        /// operation fails.
        /// </summary>
        /// <remarks>
        /// This test ensures that the method handles exceptions thrown during the
        /// database save operation by catching the exception and throwing an
        /// <see cref="InvalidOperationException"/>, thus maintaining the integrity of
        /// the operation and providing a consistent failure response.
        /// </remarks>
        [Fact]
        public async Task AddStudentInfo_DatabaseSaveFailure_ThrowsInvalidOperationException()
        {
            // Arrange

            _dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Throws(new Exception());
            var service = new StudentInfoService(_dbContextMock.Object);
            // Act and Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddStudentInfo(_newStudentInfo));
        }



        /// <summary>
        /// Verifies that the <see cref="StudentInfoService.RemoveStudentInfo"/> method removes the
        /// <see cref="StudentInfo"/> from the database.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This test ensures that the <see cref="StudentInfoService.RemoveStudentInfo"/> method
        /// correctly removes the specified <see cref="StudentInfo"/> from the database. It uses a
        /// mock of the <see cref="AppDbContext"/> to isolate the service from the actual database.
        /// </para>
        /// </remarks>
        [Fact]
        public async Task RemoveStudentInfo_ValidStudentId_RemovesStudent()
        {
            // Arrange
            _dbContextMock.Setup(x => x.StudentInfo)
            .ReturnsDbSet(StudentInfoTestDataHelper.GetFakeStudentInfo(_newStudentInfo));
            // Act
            var result = _studentInfoService.RemoveStudentInfo(_newStudentInfo.StudentId);
            // Assert
            Assert.True(result);
            _dbContextMock.Verify(x => x.StudentInfo.Remove(_newStudentInfo), Times.Once);
            _dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        /// <summary>
        /// Verifies that the <see cref="StudentInfoService.RemoveStudentInfo"/> method throws
        /// <see cref="ArgumentException"/> when the <paramref name="StudentId"/> is null or empty.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This test ensures that the <see cref="StudentInfoService.RemoveStudentInfo"/> method
        /// correctly handles null or empty <paramref name="StudentId"/> input by throwing
        /// <see cref="ArgumentException"/>. It uses a mock of the <see cref="AppDbContext"/> to
        /// isolate the service from the actual database.
        /// </para>
        /// </remarks>
        [Fact]
        public async Task RemoveStudentInfo_NullOrEmptyStudentId_ThrowsArgumentException()
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(() => _studentInfoService.RemoveStudentInfo(null));
            Assert.Throws<ArgumentException>(() => _studentInfoService.RemoveStudentInfo(string.Empty));
        }


        /// <summary>
        /// Verifies that the <see cref="StudentInfoService.RemoveStudentInfo"/> method returns
        /// false when the student with the specified <paramref name="StudentId"/> does not exist.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This test ensures that the <see cref="StudentInfoService.RemoveStudentInfo"/> method
        /// correctly handles the case where the student does not exist in the database. It uses
        /// a mock of the <see cref="AppDbContext"/> to isolate the service from the actual database.
        /// </para>
        /// </remarks>
        [Fact]
        public async Task RemoveStudentInfo_StudentDoesNotExist_ReturnsFalse()
        {
            // Arrange

            var studentInfoData = new List<StudentInfo> { _newStudentInfo }.AsQueryable();
            var mockDbSet = new Mock<DbSet<StudentInfo>>();
            mockDbSet.As<IQueryable<StudentInfo>>().Setup(m => m.Provider).Returns(studentInfoData.Provider);
            mockDbSet.As<IQueryable<StudentInfo>>().Setup(m => m.Expression).Returns(studentInfoData.Expression);
            mockDbSet.As<IQueryable<StudentInfo>>().Setup(m => m.ElementType).Returns(studentInfoData.ElementType);
            mockDbSet.As<IQueryable<StudentInfo>>().Setup(m => m.GetEnumerator()).Returns(() => studentInfoData.GetEnumerator());
            _dbContextMock.Setup(x => x.StudentInfo).Returns(mockDbSet.Object);

            var studentId = "12345";
            // Act
            var result = _studentInfoService.RemoveStudentInfo(studentId);
            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// Verifies that the <see cref="StudentInfoService.RemoveStudentInfo"/> method
        /// throws an <see cref="ApplicationException"/> when an exception occurs
        /// while removing the student information from the database.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This test ensures that the <see cref="StudentInfoService.RemoveStudentInfo"/> method
        /// correctly handles the case where an exception occurs while removing the student
        /// information from the database. It uses a mock of the
        /// <see cref="AppDbContext"/> to isolate the service from the actual database.
        /// </para>
        /// </remarks>
        [Fact]
        public async Task RemoveStudentInfo_ExceptionOccurs_ThrowsApplicationException()
        {
            // Arrange
            var studentId = "12345";
            var exception = new Exception("Test exception");
            _dbContextMock.Setup(x => x.StudentInfo.Remove(It.IsAny<StudentInfo>())).Throws(exception);
            // Act and Assert
            Assert.Throws<ApplicationException>(() => _studentInfoService.RemoveStudentInfo(studentId));
        }
    }
}

using AutoMapper;
using StudentService.Common.Mapping;
using StudentService.DTO;
using StudentService.Models;
using AutoFixture;

namespace StudentService.Tests.Common.Mapping
{
    public class StudentMappingTests
    {
        private readonly IMapper _mapper;

        public StudentMappingTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<StudentMapping>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_AddStudentDTO_To_Student_Correctly()
        {
            // Arrange
            var fixture = new Fixture();

            var addStudentDto = fixture.Build<AddStudentDTO>().Create();
 
            // Act
            var student = _mapper.Map<Student>(addStudentDto);

            // Assert
            Assert.NotNull(student);
            Assert.Equal(addStudentDto.StudentId, student.StudentId);
            Assert.Equal(addStudentDto.Age, student.Age);
            Assert.Equal(addStudentDto.Email, student.Email);
            Assert.Equal(addStudentDto.Title, student.Title);
            Assert.Equal(addStudentDto.RequireDate, student.RequireDate);
            Assert.Equal(addStudentDto.Location, student.Location);
        }

        [Fact]
        public void Should_Map_Student_To_ResponseStudentDTO_Correctly()
        {
            // Arrange
            var fixture = new Fixture();
            var addStudentDto = fixture.Build<AddStudentDTO>().Create();
 
            // Act
            var student = _mapper.Map<Student>(addStudentDto);
            var responseStudentDto = _mapper.Map<ResponseStudentDTO>(student);

            // Assert
            Assert.NotNull(responseStudentDto);
            Assert.Equal(addStudentDto.StudentId, responseStudentDto.StudentId);
            Assert.Equal(addStudentDto.Age, responseStudentDto.Age);
            Assert.Equal(addStudentDto.Email, responseStudentDto.Email);
            Assert.Equal(addStudentDto.Title, responseStudentDto.Title);
            Assert.Equal(addStudentDto.RequireDate, responseStudentDto.RequireDate);
            Assert.Equal(addStudentDto.Location, responseStudentDto.Location);
        }

        [Fact]
        public void Should_Handle_Null_AddStudentDTO()
        {
            // Act
            var student = _mapper.Map<Student>((AddStudentDTO)null);

            // Assert
            Assert.Null(student);
        }

        [Fact]
        public void Should_Handle_Null_Student()
        {
            // Act
            var responseStudentDto = _mapper.Map<ResponseStudentDTO>((Student)null);

            // Assert
            Assert.Null(responseStudentDto);
        }

        [Fact]
        public void Should_Handle_Empty_AddStudentDTO()
        {
            // Arrange
            var addStudentDto = new AddStudentDTO();

            // Act
            var student = _mapper.Map<Student>(addStudentDto);

            // Assert
            Assert.NotNull(student);
            Assert.NotNull(student.StudentId);
            Assert.Equal(0, student.Age);
            Assert.Null(student.Email);
            Assert.Null(student.Title);
            Assert.Equal(DateTime.MinValue, student.RequireDate);
            Assert.Null(student.Location);
        }

        [Fact]
        public void Should_Handle_Empty_Student()
        {
            // Arrange
            var student = new Student();

            // Act
            var responseStudentDto = _mapper.Map<ResponseStudentDTO>(student);

            // Assert
            Assert.NotNull(responseStudentDto);
            Assert.Equal(responseStudentDto.StudentId, responseStudentDto.StudentId);
            Assert.Null(responseStudentDto.Title);
            Assert.Equal(0, responseStudentDto.Age);
            Assert.Null(responseStudentDto.Email);
            Assert.Equal(DateTime.MinValue, responseStudentDto.RequireDate);
            Assert.Null(responseStudentDto.Location);

        }
    }
}

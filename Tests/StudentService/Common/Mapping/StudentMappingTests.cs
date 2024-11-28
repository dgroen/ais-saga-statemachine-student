using AutoMapper;
using StudentService.DTO;
using StudentService.Models;
using AutoFixture;

namespace StudentService.Common.Mapping.Tests
{
    public class StudentMappingTests
    {
        private readonly IMapper _mapper;

        /// <summary>
        /// Tests the mapping from <see cref="AddStudentDTO"/> to <see cref="Student"/>.
        /// </summary>
        public StudentMappingTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<StudentMapping>());
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Tests the mapping from <see cref="AddStudentDTO"/> to <see cref="Student"/> to ensure that all properties are correctly mapped.
        /// </summary>
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

        /// <summary>
        /// Tests the mapping from <see cref="Student"/> to <see cref="ResponseStudentDTO"/> to ensure that all properties are correctly mapped.
        /// </summary>
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

        /// <summary>
        /// Tests that passing a null <see cref="AddStudentDTO"/> to the mapper returns null.
        /// </summary>
        [Fact]
        public void Should_Handle_Null_AddStudentDTO()
        {
            // Act
            var student = _mapper.Map<Student>((AddStudentDTO)null);

            // Assert
            Assert.Null(student);
        }

        /// <summary>
        /// Tests that passing a null <see cref="Student"/> to the mapper returns null.
        /// </summary>
        [Fact]
        public void Should_Handle_Null_Student()
        {
            // Act
            var responseStudentDto = _mapper.Map<ResponseStudentDTO>((Student)null);

            // Assert
            Assert.Null(responseStudentDto);
        }

        /// <summary>
        /// Tests that passing an empty <see cref="AddStudentDTO"/> to the mapper returns a new <see cref="Student"/> with default values.
        /// </summary>
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

        /// <summary>
        /// Tests that passing an empty <see cref="Student"/> to the mapper returns a new <see cref="ResponseStudentDTO"/> with default values.
        /// </summary>
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

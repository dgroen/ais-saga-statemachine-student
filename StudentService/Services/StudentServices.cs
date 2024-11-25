using StudentService.Models;

namespace StudentService.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly AppDbContext _dbContext;

        public StudentServices(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Student> AddStudent(Student student)
        {
            if (student is not null)
            {
                await _dbContext.Student.AddAsync(student);
                await _dbContext.SaveChangesAsync();
            }
            return student;
        }

        public bool DeleteStudent(string StudentId)
        {
            var studentObj = _dbContext.Student.FirstOrDefault(t => t.StudentId == StudentId);
            if (studentObj is not null)
            {
                _dbContext.Student.Remove(studentObj);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}

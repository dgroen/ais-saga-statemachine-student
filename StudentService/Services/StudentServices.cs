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
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            try
            {
                await _dbContext.Student.AddAsync(student);
                await _dbContext.SaveChangesAsync();
                return student;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                // Handle database update exceptions
                // Log the exception or handle it as per your logging strategy
                throw new ApplicationException("An error occurred while adding the student to the database.", ex);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                // Log the exception or handle it as per your logging strategy
                throw new ApplicationException("An unexpected error occurred while adding the student.", ex);
            }
        }

        public bool DeleteStudent(string StudentId)
        {
            if (string.IsNullOrEmpty(StudentId))
            {
                throw new ArgumentException("StudentId cannot be null or empty.", nameof(StudentId));
            }

            try
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
            catch (Exception ex)
            {
                // Handle exceptions, such as logging the error
                // Log the exception or handle it as per your logging strategy
                throw new ApplicationException("An error occurred while deleting the student.", ex);
            }
        }
    }
}

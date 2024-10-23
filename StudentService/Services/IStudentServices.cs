using StudentService.Models;

namespace StudentService.Services
{
    public interface IStudentServices
    {
        Task<Student> AddStudent(Student student);
        bool DeleteStudent(string StudentId);

        // Other methods like Update could be implemented 
    }
}

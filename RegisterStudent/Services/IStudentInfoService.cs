using RegisterStudent.Models;

namespace RegisterStudent.Services
{
    public interface IStudentInfoService
    {
        Task<StudentInfo> AddStudentInfo(StudentInfo studentInfo);
        bool RemoveStudentInfo(string StudentId);
    }
}

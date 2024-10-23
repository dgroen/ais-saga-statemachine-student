using RegisterStudent.Common;
using RegisterStudent.Models;

namespace RegisterStudent.Services
{
    public class StudentInfoService : IStudentInfoService
    {
        private readonly AppDbContext _dbContext;

        public StudentInfoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<StudentInfo> AddStudentInfo(StudentInfo studentInfo)
        {
            if(studentInfo is not null)
            {
                studentInfo.StudentNumber = StringGenerator.Generate();
                await _dbContext.StudentInfo.AddAsync(studentInfo);
                await _dbContext.SaveChangesAsync();
            }
            return studentInfo;
        }

        public bool RemoveStudentInfo(string StudentId)
        {
            var studentInfoObj = _dbContext.StudentInfo.FirstOrDefault(t => t.StudentId == StudentId);
            if(studentInfoObj is not null)
            {
                _dbContext.StudentInfo.Remove(studentInfoObj);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}

using RegisterStudent.Models;

namespace Tests.Helpers
{
    public class StudentInfoTestDataHelper
    {
        internal static IEnumerable<StudentInfo> GetFakeStudentInfo(StudentInfo newStudentInfo)
        {
            return [newStudentInfo];
        }
    }
}
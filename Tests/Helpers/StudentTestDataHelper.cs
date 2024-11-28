using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentService.Models;

namespace Tests.Helpers
{
    public class StudentTestDataHelper
    {
        internal static IEnumerable<Student> GetFakeStudent(Student newStudent)
        {
            return new Student[] { newStudent };
        }
    }
}
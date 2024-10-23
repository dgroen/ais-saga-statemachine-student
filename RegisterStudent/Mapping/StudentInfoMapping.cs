using AutoMapper;
using Events.StudentEvents;
using RegisterStudent.Models;

namespace RegisterStudent.Mapping
{
    public class StudentInfoMapping : Profile
    {
        public StudentInfoMapping()
        {
            CreateMap<IRegisterStudentEvent, StudentInfo>();
        }
    }
}

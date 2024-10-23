using AutoMapper;
using StudentService.DTO;
using StudentService.Models;

namespace StudentService.Common.Mapping
{
    public class StudentMapping : Profile
    {
        public StudentMapping()
        {
            CreateMap<AddStudentDTO, Student>();
            CreateMap<Student, ResponseStudentDTO>();
        }
    }
}

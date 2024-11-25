using AutoMapper;
using StudentService.DTO;
using StudentService.Models;

namespace StudentService.Common.Mapping
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class StudentMapping : Profile
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Mapping profiles for Student and DTOs
        /// </summary>
        public StudentMapping()
        {
            CreateMap<AddStudentDTO, Student>();
            CreateMap<Student, ResponseStudentDTO>();
        }
    }
}

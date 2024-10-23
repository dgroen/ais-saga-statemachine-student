using System.ComponentModel.DataAnnotations;

namespace RegisterStudent.Models
{
    public class StudentInfo
    {
        [Key]
        public string StudentId { get; set; }
        public string Email { get; set; }
        public string StudentNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

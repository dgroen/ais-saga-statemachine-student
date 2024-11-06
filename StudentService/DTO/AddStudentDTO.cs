using System.ComponentModel.DataAnnotations;

namespace StudentService.DTO
{
    /// <summary>
    /// Add a new student
    /// </summary>
    public class AddStudentDTO
    {
        /// <summary>
        /// The unique identifier for the student
        /// </summary>
        [Required]
        public string StudentId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The student title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The student email
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The date the student was required
        /// </summary>
        public DateTime RequireDate { get; set; }

        /// <summary>
        /// The student age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// The student location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The date the student was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

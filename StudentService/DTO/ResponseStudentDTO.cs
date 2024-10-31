using System.ComponentModel.DataAnnotations;

namespace StudentService.DTO
{
    /// <summary>
    /// Response object for the added student
    /// </summary>
    public class ResponseStudentDTO
    {
        /// <summary>
        /// Unique identifier for the student
        /// </summary>
        [Key]
        [Required]
        [StringLength(450)]
        public string StudentId { get; set; }

        /// <summary>
        /// The title of the student
        /// </summary>
        [Required]
        [StringLength(450)]
        public string Title { get; set; }

        /// <summary>
        /// The email of the student
        /// </summary>
        [Required]
        [StringLength(450)]
        public string Email { get; set; }

        /// <summary>
        /// The date that the student is required
        /// </summary>
        [Required]
        public DateTime RequireDate { get; set; }

        /// <summary>
        /// The age of the student
        /// </summary>
        [Required]
        public int Age { get; set; }

        /// <summary>
        /// The location of the student
        /// </summary>
        [Required]
        [StringLength(450)]
        public string Location { get; set; }

        /// <summary>
        /// The student number of the student
        /// </summary>
        [Required]
        [StringLength(450)]
        public string StudentNumber { get; set; }

        /// <summary>
        /// The date that the student was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}

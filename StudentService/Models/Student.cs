namespace StudentService.Models
{
    /// <summary>
    /// Student model
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Student identifier
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Student title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Student email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Student required date
        /// </summary>
        public DateTime RequireDate { get; set; }

        /// <summary>
        /// Student age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Student location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Student number
        /// </summary>
        public string StudentNumber { get; set; }

        /// <summary>
        /// Student created date
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}

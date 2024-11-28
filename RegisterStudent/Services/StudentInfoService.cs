using RegisterStudent.Common;
using RegisterStudent.Models;

namespace RegisterStudent.Services
{
    public class StudentInfoService : IStudentInfoService
    {
        private readonly AppDbContext _dbContext;
        public AppDbContext DbContext => _dbContext;



        public StudentInfoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Adds a new <see cref="StudentInfo"/> to the database.
        /// </summary>
        /// <param name="studentInfo">The student information to add.</param>
        /// <returns>The added <see cref="StudentInfo"/> with a generated student number, or the original object if it was null.</returns>
        /// <remarks>
        /// If the provided <paramref name="studentInfo"/> is not null, this method assigns a generated student number to it,
        /// adds it to the database, and saves the changes. If the input is null, the method returns null.
        /// </remarks>
        public async Task<StudentInfo> AddStudentInfo(StudentInfo studentInfo)
        {
            if (studentInfo is null)
            {
                throw new ArgumentNullException(nameof(studentInfo), "Student information cannot be null.");
            }
            
            studentInfo.StudentNumber = StudentNumberGenerator.Generate();
            try
            {
                await _dbContext.StudentInfo.AddAsync(studentInfo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _dbContext.Dispose();
                throw new InvalidOperationException("Failed to save student information.", ex);
            }
            return studentInfo;
        }

        /// <summary>
        /// Removes the student information with the specified <paramref name="StudentId"/> from the database.
        /// </summary>
        /// <param name="StudentId">The student ID of the student whose information should be removed.</param>
        /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// <para>
        /// If the provided <paramref name="StudentId"/> is not null or empty, this method attempts to remove
        /// the student information with the specified ID from the database, and saves the changes.
        /// </para>
        /// <para>
        /// If the student information is found and removed, the method returns <c>true</c>. If the student information
        /// is not found or the operation fails, the method returns <c>false</c>.
        /// </para>
        /// </remarks>
        public bool RemoveStudentInfo(string StudentId)
        {
            if (string.IsNullOrEmpty(StudentId))
            {
                throw new ArgumentException("StudentId cannot be null or empty.", nameof(StudentId));
            }
            try
            {

                var studentInfoObj = _dbContext.StudentInfo.FirstOrDefault(t => t.StudentId == StudentId);
                if (studentInfoObj != null)
                {
                    _dbContext.StudentInfo.Remove(studentInfoObj);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Handle exceptions, such as logging the error
                // Log the exception or handle it as per your logging strategy
                throw new ApplicationException("An error occurred while removing the student's information.", ex);
            }
        }
    }
}

using Events.StudentEvents;
using MassTransit;
using StudentService.Services;

namespace StudentService.Consumers
{
    public class RegisterStudentCancelConsumer : IConsumer<ICancelRegisterStudentEvent>
    {
        private readonly IStudentServices _studentServices;
        private readonly ILogger<RegisterStudentCancelConsumer> _logger;

        /// <summary>
        /// Constructor for RegisterStudentCancelConsumer.
        /// </summary>
        /// <param name="studentServices">IStudentServices object</param>
        /// <param name="logger">ILogger object</param>
        public RegisterStudentCancelConsumer(IStudentServices studentServices, ILogger<RegisterStudentCancelConsumer> logger)
        {
            _studentServices = studentServices;
            _logger = logger;
        }
        /// <summary>
        /// Consumes the ICancelRegisterStudentEvent to delete a student record.
        /// </summary>
        /// <param name="context">The consume context containing the ICancelRegisterStudentEvent message.</param>
        /// <remarks>
        /// This method attempts to delete the student with the provided StudentId from the database.
        /// If successful, it logs the success message. Otherwise, it logs a failure message.
        /// </remarks>
        public async Task Consume(ConsumeContext<ICancelRegisterStudentEvent> context)
        {
            var data = context.Message;
            if (data is not null)
            {
                var res = _studentServices.DeleteStudent(data.StudentId.ToString());
                if (res is true)
                {
                    _logger.LogInformation("The Student has been removed successfully");
                }
                else
                {
                    _logger.LogInformation("Failed!!!");
                }
            }
        }
    }
}

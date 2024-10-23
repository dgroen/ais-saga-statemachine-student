using Events.SendEmailEvents;
using Events.StudentEvents;
using RegisterStudent.Services;
using MassTransit;

namespace RegisterStudent.Consumers
{
    public class CancelSendingEmailConsumer : IConsumer<ICancelSendEmailEvent>
    {
        private readonly IStudentInfoService _studentInfoService;
        private readonly ILogger<CancelSendingEmailConsumer> _logger;

        public CancelSendingEmailConsumer(IStudentInfoService studentInfoService, ILogger<CancelSendingEmailConsumer> logger)
        {
            _studentInfoService = studentInfoService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<ICancelSendEmailEvent> context)
        {
            var data = context.Message;
            if(data is not null)
            {
                var res = _studentInfoService.RemoveStudentInfo(data.StudentId.ToString());
                if(res is true)
                {
                    await context.Publish<ICancelRegisterStudentEvent>(new
                    {
                        StudentId = data.StudentId,
                        Title = data.Title,
                        Email = data.Email,
                        RequireDate = data.RequireDate,
                        Age = data.Age,
                        Location = data.Location
                    });
                    _logger.LogInformation("The message has been sent to the ICancelRegisterStudentEvent in the StudentService");
                }

                else
                {
                    _logger.LogInformation("Failed!!!");

                }
            }
        }
    }
}

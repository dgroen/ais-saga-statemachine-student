using Events.StudentEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Formatters;
using StudentService.Services;

namespace StudentService.Consumers
{
    public class RegisterStudentCancelConsumer : IConsumer<ICancelRegisterStudentEvent>
    {
        private readonly IStudentServices _studentServices;
        private readonly ILogger<RegisterStudentCancelConsumer> _logger;

        public RegisterStudentCancelConsumer(IStudentServices studentServices, ILogger<RegisterStudentCancelConsumer> logger)
        {
            _studentServices = studentServices;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<ICancelRegisterStudentEvent> context)
        {
            var data = context.Message;
            if(data is not null)
            {
                var res = _studentServices.DeleteStudent(data.StudentId.ToString());
                if(res is true)
                {
                    _logger.LogInformation("The Student has been removed successufully");
                }
                else
                {
                    _logger.LogInformation("Failed!!!");
                }
            }
        }
    }
}

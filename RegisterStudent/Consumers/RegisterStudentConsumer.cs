using AutoMapper;
using Events.SendEmailEvents;
using Events.StudentEvents;
using RegisterStudent.Models;
using RegisterStudent.Services;
using MassTransit;

namespace RegisterStudent.Consumers
{
    public class RegisterStudentConsumer : IConsumer<IRegisterStudentEvent>
    {
        // As shown, this consumer is listening to the IRegisterStudentEvent
        // But, Student Service publishes its message to the IAddStudentEvent
        // Here State machine will transform IAddStudentEvent to the IRegisterStudentEvent 
        private readonly IStudentInfoService _studentInfoService;
        private readonly ILogger<RegisterStudentConsumer> _logger;
        private readonly IMapper _mapper;

        public RegisterStudentConsumer(IStudentInfoService studentInfoService, ILogger<RegisterStudentConsumer> logger, IMapper mapper)
        {
            _studentInfoService = studentInfoService;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<IRegisterStudentEvent> context)
        {
            var data = context.Message;

            if (data is not null)
            {
                // Check if Age is 80 or less
                if (data.Age < 80)
                {
                    // Store message
                    // Use Mapper or use a studentinfo object directly
                    var mapModel = _mapper.Map<StudentInfo>(data);


                    var res = await _studentInfoService.AddStudentInfo(mapModel);
                    if (res is not null)
                    {

                        await context.Publish<ISendEmailEvent>(new
                        {
                            StudentId = data.StudentId,
                            Title = data.Title,
                            Email = data.Email,
                            RequireDate = data.RequireDate,
                            Age = data.Age,
                            Location = data.Location,
                            StudentNumber = res.StudentNumber
                        });
                        _logger.LogInformation($"Message sent == StudentId is {data.StudentId}");
                    }
                }
                else
                {
                    // This section will return the message to the Cancel Event
                    await context.Publish<ICancelRegisterStudentEvent>(new
                    {
                        StudentId = data.StudentId,
                        Title = data.Title,
                        Email = data.Email,
                        RequireDate = data.RequireDate,
                        Age = data.Age,
                        Location = data.Location
                    });
                    _logger.LogInformation($"Message canceled== StudentId is {data.StudentId}");
                }
            }
        }
    }
}

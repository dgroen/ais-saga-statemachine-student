using AutoMapper;
using Events.SendEmailEvents;
using Events.StudentEvents;
using RegisterStudent.Models;
using RegisterStudent.Services;
using MassTransit;
using System.Text.RegularExpressions;

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

        private static readonly Regex EmailValidationRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled
        );


        public RegisterStudentConsumer(IStudentInfoService studentInfoService, ILogger<RegisterStudentConsumer> logger, IMapper mapper)
        {
            _studentInfoService = studentInfoService;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<IRegisterStudentEvent> context)
        {
            var data = context.Message;
            if (data is null)
            {
                _logger.LogInformation("The message is null");
            }

            if (data is not null)
            {
                // Check if Age is 80 or less
                if (data.Age < 81 && isValidEmail(data.Email))
                {
                    // Store message
                    // Use Mapper or use a studentinfo object directly
                    var mapModel = _mapper.Map<StudentInfo>(data);

                    try
                    {
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
                            return;
                        }
                    }
                    catch (Exception)
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
                        _logger.LogInformation($"Exception while calling the StudentInfoService: StudentId is {data.StudentId}");
                        return;
                    }
                }

                // This section will return the message to the Cancel Event if all above wos not ok
                await context.Publish<ICancelRegisterStudentEvent>(new
                {
                    StudentId = data.StudentId,
                    Title = data.Title,
                    Email = data.Email,
                    RequireDate = data.RequireDate,
                    Age = data.Age,
                    Location = data.Location
                });
                _logger.LogInformation($"Message cancelled== StudentId is {data.StudentId}");

            }
        }
        private Boolean isValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return (addr.Address == email && EmailValidationRegex.IsMatch(email));
            }
            catch
            {
                return false;
            }
        }
    }
}

using Events.StudentEvents;
using MassTransit;

namespace StudentService.Consumers
{
    public class GetValueConsumer : IConsumer<IGETValueEvent>
    {
        private readonly ILogger<GetValueConsumer> _logger;

        public GetValueConsumer(ILogger<GetValueConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IGETValueEvent> context)
        {
            var data = context.Message;
            if (data is not null)
            {
                // This section will publish message to the IAddStudentEvent although the RegisterStudent service has a consumer
                // that it will be listened on the IAddStudentEvent
                await context.Publish<IAddStudentEvent>(new
                {
                    StudentId = data.StudentId,
                    Title = data.Title,
                    Email = data.Email,
                    RequireDate = data.RequireDate,
                    Age = data.Age,
                    Location = data.Location
                });
                _logger.LogInformation("a message has been received");
            }
        }
    }
}

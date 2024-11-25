using Events.StudentEvents;
using MassTransit;

namespace StudentService.Consumers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class GetValueConsumer : IConsumer<IGETValueEvent>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private readonly ILogger<GetValueConsumer> _logger;

/// <summary>
/// Initializes a new instance of the <see cref="GetValueConsumer"/> class.
/// </summary>
/// <param name="logger">An instance of <see cref="ILogger{GetValueConsumer}"/> used for logging information.</param>
        public GetValueConsumer(ILogger<GetValueConsumer> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Consumes the IGETValueEvent message and publishes IAddStudentEvent to the message broker.
        /// </summary>
        /// <param name="context">The consume context containing the IGETValueEvent message.</param>
        /// <remarks>
        /// Although the RegisterStudent service has a consumer that it will be listened on the IAddStudentEvent,
        /// this method will publish the IAddStudentEvent to the message broker.
        /// </remarks>
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

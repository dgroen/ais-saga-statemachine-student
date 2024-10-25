using MassTransit;

namespace SagaStateMachine
{
    public class StudentStateData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public DateTime StudentCreatedDate { get; set; }
        public DateTime StudentCancelDate { get; set; }
        public Guid StudentId { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public int Age { get; set; }
        public  string StudentNumber { get; set; }
    }
}

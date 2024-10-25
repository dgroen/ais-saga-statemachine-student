using Events.StudentEvents;

namespace SagaStateMachine
{
    // This class is responsible to pass the context messages to the another event which here is IRegisterStudentEvent
    public class RegisterStudentEvent : IRegisterStudentEvent
    {
        private readonly StudentStateData _studentStateData;

        public RegisterStudentEvent(StudentStateData studentStateData)
        {
            _studentStateData = studentStateData;
        }
        public Guid StudentId => _studentStateData.StudentId;

        public string Title => _studentStateData.Title;

        public string Email => _studentStateData.Email;

        public DateTime RequireDate => _studentStateData.StudentCreatedDate;

        public int Age => _studentStateData.Age;

        public string Location => _studentStateData.Location;

        public string StudentNumber => _studentStateData.StudentNumber;
    }
}

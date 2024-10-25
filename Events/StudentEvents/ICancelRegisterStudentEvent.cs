namespace Events.StudentEvents
{
    public interface ICancelRegisterStudentEvent
    {
        public Guid StudentId { get; }
        public string Title { get; }
        public string Email { get; }
        public DateTime RequireDate { get; }
        public int Age { get; }
        public string Location { get; }
        public string StudentNumber { get; }
    }
}

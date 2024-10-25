namespace Events.StudentEvents
{
    // This event is not going to be used in the State machine 
    // It will be used in the first service here which is StudentService
    public interface IGETValueEvent
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

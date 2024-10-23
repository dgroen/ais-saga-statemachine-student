using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.StudentEvents
{
    // This event is not going to use in the State machine 
    // It will used in the first service which here is StudentService
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

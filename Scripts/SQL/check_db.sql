select s.StudentId, 
    s.Email, 
    s.Title, 
    s.Age, 
    s.Location, 
    ssd.StudentNumber, 
    ssd.CurrentState,
    si.CreatedDate
from Student s
    left JOIN StudentStateData ssd ON s.StudentId = ssd.StudentId
    left join StudentInfo si on s.StudentId = si.StudentId



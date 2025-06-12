using System.ComponentModel.DataAnnotations;

namespace AttendanceManagementApi.Models.Entities;

public class Attendance
{
    public int Id { get; set; }
    public int LessonId { get; set; }
    public Lesson Lesson { get; set; }
    public int TeacherId { get; set; }
    public User Teacher { get; set; }
    
    public ICollection<AttendanceDetail>  AttendanceDetails { get; set; }
    
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
}   
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagementApi.Models.Entities;

public class Lesson
{
    public int Id { get; set; }
    [MaxLength(150)]
    public string Name { get; set; }
    public int SortOrder { get; set; }
    
    public ICollection<Attendance> Attendances { get; set; }
    
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
}
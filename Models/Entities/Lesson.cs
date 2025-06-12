using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementApi.Models.Entities;

[Index(nameof(Name),  IsUnique = true)]
[Index(nameof(SortOrder),  IsUnique = true)]
public class Lesson
{
    public int Id { get; set; }
    [MaxLength(150)]
    public string Name { get; set; }
    public int SortOrder { get; set; }
    
    public ICollection<Attendance> Attendances { get; set; }
    public ICollection<Classroom> Classrooms { get; set; }
    
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace AttendanceManagementApi.Models.Entities;

public class Classroom
{
    public int Id { get; set; }
    [MaxLength(100), Required]
    public string Name { get; set; }
    
    public ICollection<User> Users { get; set; }
    public ICollection<Lesson> Lessons { get; set; }
    
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
}
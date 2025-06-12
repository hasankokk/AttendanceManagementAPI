using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementApi.Models.Entities;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    [MaxLength(50), Required]
    public string Name { get; set; }
    [MaxLength(50), Required]
    public string Surname { get; set; }    
    [MaxLength(50), Required]
    public string Email { get; set; }
    [MaxLength(50), Required]
    public string PhoneNumber { get; set; }
   
    public Role Role { get; set; } = Role.Student;
    
    public ICollection<Classroom> Classrooms { get; set; }
    
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
}

public enum Role
{
    Student,
    Teacher
}
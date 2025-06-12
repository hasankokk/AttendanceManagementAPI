using System.ComponentModel.DataAnnotations;
using AttendanceManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementApi.Models.DTOs.User;
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class UserUpdateDto
{
    [MaxLength(50)]
    public string? Name { get; set; }
    [MaxLength(50)]
    public string? Surname { get; set; }    
    [MaxLength(50), EmailAddress]
    public string? Email { get; set; }
    [MaxLength(50)]
    public string? PhoneNumber { get; set; }
    
    public int ClassroomId { get; set; }
    public bool DeleteClassroom { get; set; } = false;

    public Role Role { get; set; } = Role.Student;
}
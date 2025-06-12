using System.ComponentModel.DataAnnotations;
using AttendanceManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementApi.Models.DTOs.User;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class UserCreateDto
{
    [MaxLength(50), Required]
    public string Name { get; set; }
    [MaxLength(50), Required]
    public string Surname { get; set; }    
    [MaxLength(50), Required, EmailAddress]
    public string Email { get; set; }
    [MaxLength(50), Required]
    public string PhoneNumber { get; set; }
   
    public Role Role { get; set; } = Role.Student;
}
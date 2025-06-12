using AttendanceManagementApi.Models.Entities;

namespace AttendanceManagementApi.Models.DTOs.User;

public class UserWithRoleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Role Role { get; set; }
}
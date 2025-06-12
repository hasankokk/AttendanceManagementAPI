using System.ComponentModel.DataAnnotations;

namespace AttendanceManagementApi.Models.Entities;

public class AttendanceDetail
{
    public int Id { get; set; }
    public int AttendanceId { get; set; }
    public Attendance Attendance { get; set; }
    public int StudentId { get; set; }
    public User Student { get; set; }
    public int AttendanceStatusId { get; set; }
    public AttendanceStatus AttendanceStatus{ get; set; }
    
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Updated { get; set; }
}
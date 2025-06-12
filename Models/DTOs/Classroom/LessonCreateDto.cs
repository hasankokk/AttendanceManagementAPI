namespace AttendanceManagementApi.Models.DTOs.Classroom;

public class LessonCreateDto
{
    public required string Name { get; set; }
    public required int SortOrder { get; set; }
}
using AttendanceManagementApi.Data;
using AttendanceManagementApi.Models.DTOs.Classroom;
using AttendanceManagementApi.Models.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementApi.Endpoints;

public static class LessonEndpoints
{
    public static void MapLessonEndpoints(this WebApplication app)
    {
        var lessonGroup = app.MapGroup("/lesson").WithTags("Ders İşlemleri")
            .WithSummary("Ders için Endpointler")
            .WithSummary("Ders ile ilgili işlemleri burada yapıyoruz.");

        lessonGroup.MapPost("/", (int classroomId ,LessonCreateDto dto, AppDbContext context) =>
        {
            var classroom = context.Classrooms
                .Include(c => c.Lessons)
                .FirstOrDefault(c => c.Id == classroomId);

            if (classroom == null)
                return Results.NotFound("Sınıf bulunamadı!");
            
            var existingLesson = context.Lessons.FirstOrDefault(l => l.Name == dto.Name);
            if (existingLesson != null)
            {
                if (classroom.Lessons.Any(c => c.Name == dto.Name))
                    return Results.BadRequest("Ders sınıfta mevcut!");
                classroom.Lessons.Add(existingLesson);
                context.SaveChanges();
                return Results.Ok(new { message = "Yeni ders eklenmedi. Ders sınıfa eklendi."});
            }
            else
            {
                if (context.Lessons.Any(l => l.SortOrder == dto.SortOrder))
                    return Results.BadRequest("Ders sıralama indisi unique olmalı!");
                var lesson = new Lesson
                {
                    Name = dto.Name,
                    SortOrder = dto.SortOrder,
                };
                classroom.Lessons.Add(lesson);
                context.Lessons.Add(lesson);
            }
            
            context.SaveChanges();
            return Results.Created($"/lesson/{dto.Name}", dto);
        })
        .WithSummary("Ders ekleme")
        .WithDescription("Ders ekleme işlemi burada yapılıyor.");

        lessonGroup.MapGet("/lessons", (AppDbContext context) => context.Lessons
                    .Select(l => new
                    {
                        l.Name,
                        l.SortOrder,
                    }).OrderBy(l => l.SortOrder).ToList()
            ).WithSummary("Dersleri önem sırasına göre listeler");
    }
}
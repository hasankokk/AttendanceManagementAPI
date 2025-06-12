using AttendanceManagementApi.Data;
using AttendanceManagementApi.Models.DTOs.Classroom;
using AttendanceManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementApi.Endpoints;

public static class ClassroomEndpoints
{
    public static void MapClassroomEndpoints(this WebApplication app)
    {
        var classroomGroup = app.MapGroup("/classroom").WithTags("Sınıf İşlemleri")
            .WithSummary("Sınıf için Endpointler")
            .WithDescription("Sınıf için gerekli işlemleri burada yapıyoruz.");
        
        classroomGroup.MapPost("/", (ClassroomCreateDto dto, AppDbContext context) =>
        {
            if (context.Classrooms.Any(c => c.Name == dto.Name))
               return Results.BadRequest("Bu isimde bir sınıf bulunuyor!");
            var classroom = new Classroom
            {
                Name = dto.Name,
            };
            context.Classrooms.Add(classroom);
            context.SaveChanges();
            return Results.Created($"/classroom/{classroom.Name}", classroom);
        })
        .WithSummary("Yeni bir sınıf oluşturur")
        .WithDescription("Eğer aynı isimde bir sınıf yoksa yeni sınıf oluşturma işlemi başarılı olur");

        classroomGroup.MapGet("/classrooms", (AppDbContext context) =>
            context.Classrooms
                .Select(c => new
                {
                    c.Name,
                    userCount = c.Users.Count(),
                    lessonCount = c.Lessons.Count()
                })
                .ToList())
            .WithSummary("Sınıfları listeme")
            .WithDescription("Sınıflarda kaç kişi var sayısını gösterir.");

        classroomGroup.MapGet("/classroom", (AppDbContext context, string classroomName) =>
        {
            var classroom = context.Classrooms
                .Where(c => c.Name == classroomName)
                .Include(c => c.Users)
                .FirstOrDefault();
            if (classroom == null)
                return Results.NotFound("Sınıf bulunamadı!");
            return Results.Ok(new
            {
                classroom.Name,
                Users = classroom.Users
                    .OrderByDescending(u => u.Role == Role.Teacher)
                    .Select(u => new
                    {
                        u.Name,
                        u.Surname,
                        Role = u.Role.ToString(),
                    }).ToList(),
            });
        })
        .WithSummary("Sınıf ile kullanıcı listeleme")
        .WithDescription("Girilen isimle eşleşen sınıfı içerisindeki kullanıcılarla listeler.");
        
        classroomGroup.MapPut("/{id:int}", (int id, ClassroomUpdateDto? dto, AppDbContext context) =>
            {
                var classroom = context.Classrooms
                    .Include(c => c.Users)
                    .FirstOrDefault(c => c.Id == id);
                if (classroom == null)
                    return Results.NotFound("Sınıf bulunamadı!");
                classroom.Name = dto.Name == null ? classroom.Name : dto.Name;
                classroom.Updated = DateTime.Now;
                if (dto.Email != null)
                {
                    var findUser = classroom.Users.FirstOrDefault(u => u.Email == dto.Email);
                    if (findUser == null)
                        return Results.BadRequest("Kullanıcı sınıfta bulunamadı!");
                    classroom.Users.Remove(findUser);

                }
                context.SaveChanges();
                return Results.Ok(new { message = "İşlem başarılı!"});
            })
        .WithSummary("Sınıf ismini günceller, sınıftan kullanıcı çıkarır.");

        classroomGroup.MapDelete("/{id:int}", (int id, AppDbContext context) =>
        {
            var classroom = context.Classrooms.Find(id);
            if (classroom == null)
                return Results.BadRequest("Sınıf bulunamadı!");
            context.Classrooms.Remove(classroom);
            context.SaveChanges();
            return Results.Ok(new { message = "Sınıf silindi!"});
        })
        .WithSummary("Sınıf silme işlemi burada yapılır.");
    }
}
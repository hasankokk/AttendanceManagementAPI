using AttendanceManagementApi.Data;
using AttendanceManagementApi.Models.DTOs;
using AttendanceManagementApi.Models.DTOs.User;
using AttendanceManagementApi.Models.Entities;
using Mapster;

namespace AttendanceManagementApi.Endpoints;

public static class TeacherEndpoints
{
    public static void MapTeacherEndpoints(this WebApplication app)
    {
            var teacherGroup = app.MapGroup("/teacher").WithTags("Öğretmen İşlemleri")
            .WithSummary("Öğretmen için Endpointler")
            .WithDescription("Öğretmen düzenleme ve listeleme işlemleri burada yapılır.");
        

        teacherGroup.MapGet("/teachers", (AppDbContext context, int skip = 0, int limit = 10, bool showAll = false) =>
        {
            var total = context.Users.Count(u => u.Role == Role.Teacher);
            if (showAll)
                limit = total;
            var teacher = context.Users.Where(u => u.Role == Role.Teacher)
                .Skip(skip)
                .Take(limit)
                .ToArray();
            return new
            {
                Teacher = teacher.Adapt<UserWithRoleDto[]>(),
                skip,
                limit,
                total
            };
        })
        .WithSummary("Öğretmenleri listeleme işlemi")
        .WithDescription("Kullanıcı veritabanından teacher rolüne göre listelemek burada yapılır.");

        teacherGroup.MapGet("/{id:int}", (int id, AppDbContext context) =>
        {
            var teacher = context.Users.Find(id);
            if (teacher == null)
                return Results.NotFound();
            if (teacher.Role != Role.Teacher)
                return Results.BadRequest("Kullanıcı öğretmen değil!");
            return Results.Ok(teacher);
        })
        .WithSummary("Tek öğretmen listeleme işlemi burada yapılır")
        .WithDescription("Öğretmen ID'sine göre tek öğretmen listeleme burada yapılır.")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        teacherGroup.MapPut("/{id:int}", (int id, UserUpdateDto? dto, AppDbContext context) =>
            {
                var teacher = context.Users.Find(id);
                if (teacher == null)
                    return Results.NotFound("Öğretmen bulunamadı!");
                if (teacher.Role != Role.Teacher)
                    return Results.BadRequest("Kullanıcı öğretmen değil!");
                teacher.Name = dto.Name == null ? teacher.Name : dto.Name;
                teacher.Surname = dto.Surname == null ? teacher.Surname : dto.Surname;
                teacher.Email = dto.Email == null ? teacher.Email : dto.Email;
                teacher.PhoneNumber = dto.PhoneNumber == null ? teacher.PhoneNumber : dto.PhoneNumber;
                teacher.Role = Role.Teacher;
                teacher.Updated = DateTime.Now;
                context.SaveChanges();
                return Results.Ok(new { message = "İşlem başarılı" });
            })
            .WithSummary("Öğretmen güncelleme işlemi burada yapılır.")
            .WithDescription("Öğretmen ID'sine göre Öğretmen bilgileri güncellenir eğer " +
                             "UserUpdateDto'dan boş property gelirse var olan değer değişmez.");
    }
}
using AttendanceManagementApi.Data;
using AttendanceManagementApi.Models.DTOs;
using AttendanceManagementApi.Models.DTOs.User;
using AttendanceManagementApi.Models.Entities;
using Mapster;

namespace AttendanceManagementApi.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this WebApplication app)
    {
        var studentGroup = app.MapGroup("/student").WithTags("Öğrenci İşlemleri")
            .WithSummary("Öğrenci için Endpointler")
            .WithDescription("Öğrenci  düzenleme ve listeleme işlemleri burada yapılır.");

        studentGroup.MapGet("/students", (AppDbContext context, int skip = 0, int limit = 10, bool showAll = false) =>
        {
            var total = context.Users.Count(u => u.Role == Role.Student);
            if (showAll)
                limit = total;
            var students = context.Users.Where(u => u.Role == Role.Student)
                .Skip(skip)
                .Take(limit)
                .ToArray();
            return new
            {
                Students = students.Adapt<UserWithRoleDto[]>(),
                skip,
                limit,
                total
            };
        })
        .WithSummary("Öğrencileri listeleme işlemi")
        .WithDescription("Kullanıcı veritabanından student rolüne göre listelemek burada yapılır.");

        studentGroup.MapGet("/{id:int}", (int id, AppDbContext context) =>
        {
            var student = context.Users.Find(id);
            if (student == null)
                return Results.NotFound();
            if (student.Role != Role.Student)
                return Results.BadRequest("Kullanıcı öğrenci değil!");
            return Results.Ok(student);
        })
        .WithSummary("Tek öğrenci listeleme işlemi burada yapılır")
        .WithDescription("Öğrenci ID'sine göre tek öğrenci listeleme burada yapılır.")
        .Produces<User>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        studentGroup.MapPut("/{id:int}", (int id, UserUpdateDto? dto, AppDbContext context) =>
            {
                var student = context.Users.Find(id);
                if (student == null)
                    return Results.NotFound("Öğrenci bulunamadı!");
                if (dto.Role != null && student.Role != Role.Student)
                    return Results.BadRequest("Kullanıcı öğrenci değil!");
                student.Name = dto.Name == null ? student.Name : dto.Name;
                student.Surname = dto.Surname == null ? student.Surname : dto.Surname;
                student.Email = dto.Email == null ? student.Email : dto.Email;
                student.PhoneNumber = dto.PhoneNumber == null ? student.PhoneNumber : dto.PhoneNumber;
                student.Role = dto.Role == null ? student.Role : dto.Role;
                student.Updated = DateTime.Now;
                context.SaveChanges();
                return Results.Ok(new { message = "İşlem başarılı" });
            })
            .WithSummary("Öğrenci güncelleme işlemi burada yapılır.")
            .WithDescription("Öğrenci ID'sine göre öğrenci bilgileri güncellenir eğer " +
                             "UserUpdateDto'dan boş property gelirse var olan değer değişmez.");
    }
    
}
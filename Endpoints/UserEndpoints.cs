using AttendanceManagementApi.Data;
using AttendanceManagementApi.Models.DTOs;
using AttendanceManagementApi.Models.DTOs.User;
using AttendanceManagementApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementApi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/user").WithTags("Kullanıcı İşlemleri")
            .WithSummary("Kullanıcı Ekleme ve Silme işlemleri burada yapılır.");
        
        userGroup.MapPost("/", (int classroomId, UserCreateDto? dto, AppDbContext context) => {
                if (dto == null)
                    return Results.BadRequest("Öğrenci bilgisi eksik");
                if (context.Users.Any(u => u.Email == dto.Email))
                    return Results.BadRequest("E-Posta adresi kullanılıyor!");
                var user = new User
                {
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Role = dto.Role,
                };
                context.Users.Add(user);
                var classroom = context.Classrooms
                    .Include(u => u.Users)
                    .FirstOrDefault(c => c.Id == classroomId);
                if (classroom != null)
                    classroom.Users.Add(user);
                context.SaveChanges();
                var resultDto = new UserCreateDto
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                };
                return Results.Created($"/user/{user.Id}", resultDto); 
            })
            .WithSummary("Kullanıcı ekleme işlemi")
            .WithDescription("Kullanıcıyı veritabanına ekleme işlemi burada yapılır.");
        
        userGroup.MapDelete("/{id:int}", (int id, AppDbContext context) =>
            {
                var user = context.Users.Find(id);
                if (user == null)
                    return Results.NotFound("Kullanıcı bulunamadı!");
                context.Users.Remove(user);
                context.SaveChanges();
                return Results.Ok(user);
            })
            .WithSummary("Kullanıcı silme işlemi burada yapılır.")
            .WithDescription("Kullanıcının ID si ile silme işlemi yapılır. Başarılı durumda kullanıcıya mesaj dönülür.")
            .Produces<User>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
using AttendanceManagementApi.Data;
using AttendanceManagementApi.Models.DTOs;
using AttendanceManagementApi.Models.DTOs.User;
using AttendanceManagementApi.Models.Entities;

namespace AttendanceManagementApi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/user").WithTags("Kullanıcı İşlemleri")
            .WithSummary("Kullanıcı Ekleme ve Silme işlemleri burada yapılır.");
        
        userGroup.MapPost("/", (UserCreateDto? dto, AppDbContext context) => {
                if (dto == null)
                    return Results.BadRequest("Öğrenci bilgisi eksik");
                if (dto.Email == context.Users.FirstOrDefault(x => x.Email == dto.Email)?.Email)
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
                context.SaveChanges();
                return Results.Created($"/user/{user.Id}", user); 
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
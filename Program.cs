using AttendanceManagementApi.Data;
using AttendanceManagementApi.Endpoints;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapUserEndpoints();
app.MapStudentEndpoints();
app.MapTeacherEndpoints();
app.MapClassroomEndpoints();
app.MapLessonEndpoints();
//app.UseHttpsRedirection();

app.Run();
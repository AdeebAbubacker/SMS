using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolManagementSystem.Application.Contracts.IServices;
using SchoolManagementSystem.Application.Contracts.Service;
using SchoolManagementSystem.Application.DTOs.Auth;
using SchoolManagementSystem.Application.DTOs.Department;
using SchoolManagementSystem.Application.Interfaces;
using SchoolManagementSystem.Application.Validators;
using SchoolManagementSystem.Domain.Context;
using SchoolManagementSystem.Domain.Interfaces;
using SchoolManagementSystem.Domain.IRepositories;
using SchoolManagementSystem.Domain.Models;
using SchoolManagementSystem.Domain.Repositories;
using SchoolManagementSystem.Infrastructure.Middlewares;
using SchoolManagementSystem.Infrastructure.Repositories;
using SchoolManagementSystem.Infrastructure.Services;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(configuration.GetConnectionString("ConnStr"))
);

var jwtOptions = configuration.GetSection("JWT");
builder.Services.Configure<JWTOptions>(jwtOptions);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>(); 
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IValidator<CreateDepartmentDTO>, CreateDepartmentValidator>(); 
builder.Services.AddScoped<IValidator<RegisterRequestDTO>, RegisterRequestDTOValidator>(); 



//builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options =>
 {
     options.SaveToken = true;
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidAudience = jwtOptions["ValidAudience"],
         ValidIssuer = jwtOptions["ValidIssuer"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions["Secret"] ?? string.Empty)),
         ClockSkew = TimeSpan.Zero
     };
 });
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:8080") // Replace with your Seq server URL
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SMS API",
        Version = "v1",
        Description = "School Management System API"
    });

    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddControllers();
var app = builder.Build();
app.UseMiddleware<LoggingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "School API v1");
});
app.MapControllers();

app.Run();

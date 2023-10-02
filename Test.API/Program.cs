using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Test.Dominio.Entities;
using Test.Infraestructura;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Test.API")));

builder.Services
    .AddDbContext<TenantDbContext>(options => options.UseSqlServer(b => b.MigrationsAssembly("Test.API")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddHttpContextAccessor()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API", Version = "v1" });
});

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "tenantRoute",
        pattern: "{tenant}/{controller=Home}/{action=Index}/{id?}",
        constraints: new { tenant = @"^[a-zA-Z0-9]+$" });
});

app.UseMiddleware<TenantMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

RunMigrations();

app.Run();


void RunMigrations()
{
    try
    {
        // Configura el servicio de Entity Framework Core y el DbContext
        var serviceProvider = new ServiceCollection()
            .AddDbContext<TenantDbContext>(options =>
            {
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Test.API"));
            })
            .BuildServiceProvider();

        // Crea una instancia del contexto de base de datos
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TenantDbContext>();

            // Obtiene las migraciones aplicadas y pendientes
            var appliedMigrations = context.Database.GetAppliedMigrations().ToList();
            var pendingMigrations = context.Database.GetPendingMigrations().ToList();

            // Aplica las migraciones pendientes si las hay
            if (pendingMigrations.Any())
            {
                context.Database.Migrate();
                Console.WriteLine("Migraciones pendientes aplicadas exitosamente.");
            }
            else
            {
                Console.WriteLine("No hay migraciones pendientes.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al aplicar migraciones: {ex.Message}");
    }
}
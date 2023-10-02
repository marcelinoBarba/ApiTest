using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Test.Infraestructura
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TenantMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string tenantIdentifier = GetTenantIdentifierFromRequest(context);
            string connectionString = _configuration.GetConnectionString(tenantIdentifier);

            if (connectionString != null)
            {
                using (var dbContext = new TenantDbContext(connectionString))
                {
                    context.Items["DbContext"] = dbContext;
                    await _next(context);
                }
            }
            else
            {
                connectionString = _configuration.GetConnectionString("Default");
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                 .Options;

                using (var dbContext = new ApplicationDbContext(options))
                {
                    context.Items["DbContext"] = dbContext;
                    await _next(context);
                }
            }
        }

        private string GetTenantIdentifierFromRequest(HttpContext context)
        {
            return context.Request.Path.Value.Split('/')[1];
        }

    }
}

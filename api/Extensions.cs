using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.SqlServer;
using HRApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

public static class Extensions
{
    public static IHost MigrateDatabase(this IHost webHost)
    {
        // Manually run any pending migrations if configured to do so.
        // var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        // if (env == "Production")
        // {
            var serviceScopeFactory = (IServiceScopeFactory)webHost.Services.GetService(typeof(IServiceScopeFactory));
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<UserContext>();
                dbContext.Database.Migrate();
            }
        // }
        return webHost;
    }
}
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace UI;

/// <summary>
/// Фабрика создания контекста для Dependency Injection.
/// </summary>
public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public static ApplicationContext CreateDbContext()
    {
        return new ApplicationContext(
            new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(GetConnectionString()).Options);
    }


    internal static string GetConnectionString()
    {
        ConfigurationBuilder builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        IConfigurationRoot config = builder.Build();
        return config.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// Создание экземпляра ApplicationContext.
    /// </summary>
    public ApplicationContext CreateDbContext(string[] args) => CreateDbContext();
}
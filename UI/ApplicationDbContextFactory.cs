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
    public static ApplicationContext CreateDbContext() =>
        new ApplicationContext(
            new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlite(GetConnectionString()).Options);

    internal static string GetConnectionString()
    {
        ConfigurationBuilder builder = new ConfigurationBuilder();
        // установка пути к текущему каталогу
        builder.SetBasePath(Directory.GetCurrentDirectory());
        // получаем конфигурацию из файла appsettings.json
        builder.AddJsonFile("appsettings.json");
        // создаем конфигурацию
        IConfigurationRoot config = builder.Build();
        // получаем строку подключения
        return config.GetConnectionString("DefaultConnection");
    }

    /// <summary>
    /// Создание экземпляра ApplicationContext.
    /// </summary>
    public ApplicationContext CreateDbContext(string[] args) => CreateDbContext();
}
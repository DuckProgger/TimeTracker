using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Database;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite(GetConnectionString());
    }

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
}   

/// <summary>
/// Фабрика создания контекста для Dependency Injection.
/// </summary>
public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public static ApplicationContext CreateDbContext() =>
        new ApplicationContext(
            new DbContextOptionsBuilder<ApplicationContext>()
            .UseSqlite(ApplicationContext.GetConnectionString()).Options);

    /// <summary>
    /// Создание экземпляра ApplicationContext.
    /// </summary>
    public ApplicationContext CreateDbContext(string[] args) => CreateDbContext();
}

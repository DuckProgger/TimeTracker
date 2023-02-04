using Database;
using Prism.Ioc;
using Services;

namespace UI.DependencyInjection;

/// <summary>
/// Класс для регистрации базы данных в контейнере IoC.
/// </summary>
internal static class DbRegistrator
{
    public static IContainerRegistry AddDatabase(this IContainerRegistry container) => container
        .RegisterScoped<ApplicationContext>(ApplicationContextFactory.CreateDbContext)
        .RegisterScoped(typeof(IRepository<>), typeof(DbRepository<>));
}

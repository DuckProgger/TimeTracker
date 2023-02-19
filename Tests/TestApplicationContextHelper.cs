using Database;
using Entity;
using Entity.Base;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Tests;

internal class TestApplicationContextHelper
{
    public static ApplicationContext Create()
    {
        return new ApplicationContext(
             new DbContextOptionsBuilder<ApplicationContext>()
                 .UseSqlite("Data Source=.\\testDb.db3").Options);
    }

    public static void CleanDatabase()
    {
        using var db = Create();
        db.Database.EnsureDeleted();
        db.Database.Migrate();
    }

    public static void WrapInContext(Action<ApplicationContext> action)
    {
        using var context = Create();
        action(context);
    }
}
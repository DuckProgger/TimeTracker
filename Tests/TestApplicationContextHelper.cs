using Database;
using Entity;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Tests;

internal class TestApplicationContextHelper
{
    public static ApplicationContext Create()
    {
        return new ApplicationContext(
             new DbContextOptionsBuilder<ApplicationContext>()
                 .UseSqlite("testDb.db3").Options);
    }

    public static void CleanDatabase()
    {
        using var db = Create();
        db.Database.EnsureDeleted();
    }

    public static void WrapInContext(Action action)
    {
        using(var context = Create())
        {
            var workdayRepository = new DbRepository<Workday>(context);

        }
    }
}
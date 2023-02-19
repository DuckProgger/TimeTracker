using Entity;
using Services;

namespace Tests;

[CollectionDefinition(nameof(WorkdayService), DisableParallelization = true)]
public class WorkdayServiceTest
{
    [Fact]
    public async Task Add_work()
    {
        TestApplicationContextHelper.CleanDatabase();
        var workdayDate = DateOnly.Parse("01.01.2000");
        var workName = "testWork";

        await Execute(wds => wds.AddWork(workdayDate, workName));

        var createdWorkday = await Execute(wds => wds.GetByDate(workdayDate));

        Assert.NotNull(createdWorkday);
        Assert.Equal(1, createdWorkday.Works.Count);
        Assert.Equal(workdayDate, createdWorkday.Date);
        Assert.Collection(createdWorkday.Works,
            item => Assert.Equal(workName, item.Name));
    }

    private static T Execute<T>(Func<WorkdayService, T> action) where T : class
    {
        T? result = null;
        TestApplicationContextHelper.WrapInContext(context =>
        {
            var workdayRepository = new DbRepository<Workday>(context);
            var workRepository = new DbRepository<Work>(context);
            var workdayService = new WorkdayService(workdayRepository, workRepository);
            result = action(workdayService);
        });
        return result;
    }
}
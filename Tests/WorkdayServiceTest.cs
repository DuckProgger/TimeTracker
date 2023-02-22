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
        const string workName = "testWork";

        await Execute(wds => wds.AddWork(workdayDate, workName));

        var createdWorkday = await Execute(wds => wds.GetByDate(workdayDate));
        Assert.NotNull(createdWorkday);
        Assert.Equal(1, createdWorkday.Works.Count);
        Assert.Equal(workdayDate, createdWorkday.Date);
        Assert.Collection(createdWorkday.Works,
            item => Assert.Equal(workName, item.Name));
    }

    [Fact]
    public async Task Add_workload_to_work()
    {
        TestApplicationContextHelper.CleanDatabase();
        var workdayDate = DateOnly.Parse("01.01.2000");
        const string workName = "testWork";
        var createdWork = await Execute(wds => wds.AddWork(workdayDate, workName));
        var workload = TimeSpan.FromHours(1) + TimeSpan.FromMinutes(2) + TimeSpan.FromSeconds(3);

        await Execute(wds => wds.AddWorkload(createdWork.Id, workload));

        var createdWorkday = await Execute(wds => wds.GetByDate(workdayDate));
        var createdWorkAfterAddWorkload = createdWorkday.Works.Single(w => w.Name == workName);
        Assert.Equal(workload, createdWorkAfterAddWorkload.Workload);
    }

    [Fact]
    public async Task Remove_work()
    {
        TestApplicationContextHelper.CleanDatabase();
        var workdayDate = DateOnly.Parse("01.01.2000");
        const string workName = "testWork";
        var createdWork = await Execute(wds => wds.AddWork(workdayDate, workName));

        await Execute(wds => wds.RemoveWork(createdWork.Id));

        var createdWorkday = await Execute(wds => wds.GetByDate(workdayDate));
        Assert.Equal(0, createdWorkday.Works.Count);
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
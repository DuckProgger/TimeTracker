using Entity;
using Services;

namespace Tests;

public class WorkdayServiceTest
{
    //public WorkdayServiceTest()
    //{
    //   TestApplicationContextFactory.CleanDatabase();
    //}

    [Fact]
    public void Add_work()
    {
        TestApplicationContextFactory.CleanDatabase();
        using(var context = TestApplicationContextFactory.Create())
        {
            var workdayRepository = new DbRepository<Workday>(context);

        }
    }
}
using System.Reflection.Metadata.Ecma335;
using Entity;

namespace Tests;

public class WorkdayTest
{
    [Fact]
    public void Add_works_to_workday()
    {
        var workday = new Workday(DateOnly.MinValue);
        var work1 = new Work("work1");
        var work2 = new Work("work2");

        workday.AddWork(work1);
        workday.AddWork(work2);

        Assert.Equal(2, workday.Works.Count);
        Assert.Collection(workday.Works, 
            item => Assert.Equal(work1, item),
            item => Assert.Equal(work2, item));
    }

    [Fact]
    public void Error_when_add_work_with_the_same_name()
    {
        var workday = new Workday(DateOnly.MinValue);
        var work1 = new Work("work1");
        var work2 = new Work("work1");
        workday.AddWork(work1);
       
        var addWorkdaySecondTimeAction = workday.AddWork;

        Assert.Throws<Exception>(() => addWorkdaySecondTimeAction(work2));
    }

    [Fact]
    public void Error_when_start_work_recording_in_workday_contains_active_work()
    {
        var workday = new Workday(DateOnly.MinValue);
        var work1 = TestWork.Create(1, "work1");
        var work2 = TestWork.Create(2, "work2");
        workday.AddWork(work1);
        workday.AddWork(work2);
        workday.StartRecording(1, DateTime.Now);

        var startRecordingForSecondWorkAction = workday.StartRecording;

        Assert.Throws<Exception>(() => startRecordingForSecondWorkAction(2, DateTime.Now));
    }

    [Fact]
    public void Error_when_start_work_recording_for_non_existent_work()
    {
        var workday = new Workday(DateOnly.MinValue);
        var work1 = TestWork.Create(1, "work1");
        workday.AddWork(work1);

        var startRecordingAction = workday.StartRecording;

        Assert.Throws<Exception>(() => startRecordingAction(2, DateTime.Now));
    }

    [Fact]
    public void Start_work_recording()
    {
        var workday = new Workday(DateOnly.MinValue);
        var work1 = TestWork.Create(1, "work1");
        var work2 = TestWork.Create(2, "work2");
        workday.AddWork(work1);
        workday.AddWork(work2);

        workday.StartRecording(1, DateTime.Now);

        Assert.True(work1.IsActive);
        Assert.False(work2.IsActive);
    }

    [Fact]
    public void Stop_work_recording()
    {
        var workday = new Workday(DateOnly.MinValue);
        var work1 = TestWork.Create(1, "work1");
        var work2 = TestWork.Create(2, "work2");
        workday.AddWork(work1);
        workday.AddWork(work2);
        workday.StartRecording(1, DateTime.Now);

        workday.StopRecording(1, DateTime.Now);

        Assert.False(work1.IsActive);
        Assert.False(work2.IsActive);
    }

    [Fact]
    public void Total_workload_calculation()
    {
        var workday = new Workday(DateOnly.MinValue);
        var work1 = TestWork.Create(1, "work1");
        var work2 = TestWork.Create(2, "work2");
        workday.AddWork(work1);
        workday.AddWork(work2);
        var startDateTime1 = DateTime.Parse("01.01.2000 10:00:00");
        var endDateTime1 = DateTime.Parse("01.01.2000 12:34:56");
        var startDateTime2 = DateTime.Parse("01.01.2000 13:00:00");
        var endDateTime2 = DateTime.Parse("01.01.2000 14:00:00");
        var expectedTotalWorkload = TimeSpan.FromHours(3) + TimeSpan.FromMinutes(34) + TimeSpan.FromSeconds(56);
        workday.StartRecording(1, startDateTime1);
        workday.StopRecording(1, endDateTime1);
        workday.StartRecording(2, startDateTime2);
        workday.StopRecording(2, endDateTime2);

        var totalWorkload = workday.TotalWorkload;

        Assert.Equal(expectedTotalWorkload, totalWorkload);
    }

    [Fact]
    public void Zero_total_workload_when_empty_list_of_works()
    {
        var workday = new Workday(DateOnly.MinValue);

        var totalWorkload = workday.TotalWorkload;

        Assert.Equal(TimeSpan.Zero, totalWorkload);
    }

    private class TestWork : Work
    {
        public TestWork(int id, string name) : base(name)
        {
            Id = id;
        }

        public static Work Create(int id, string name) => new TestWork(id, name);
    }
}
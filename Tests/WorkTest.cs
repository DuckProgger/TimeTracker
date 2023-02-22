using Entity;

namespace Tests;

public class WorkTest
{
    [Fact]
    public void Error_create_work_with_null_name()
    {
        Assert.Throws<Exception>(() => new Work(null));
    }

    [Fact]
    public void Error_create_work_with_empty_name()
    {
        Assert.Throws<Exception>(() => new Work(string.Empty));
    }

    [Fact]
    public void Change_work_name()
    {
        var work = new Work("name1");
        const string newName = "name2";

        work.ChangeName(newName);

        Assert.Equal(newName, work.Name);
    }

    [Fact]
    public void Error_when_changing_name_is_null()
    {
        var work = new Work("name1");
        string nullName = null;

        var changeNameAction = work.ChangeName;

        Assert.Throws<Exception>(() => changeNameAction(nullName));
    }

    [Fact]
    public void Error_when_changing_name_is_empty()
    {
        var work = new Work("name1");
        string emptyName = string.Empty;

        var changeNameAction = work.ChangeName;

        Assert.Throws<Exception>(() => changeNameAction(emptyName));
    }

    [Fact]
    public void Change_workload()
    {
        var work = CreateWork();
        var oneHourWorkload = TimeSpan.FromHours(1);
        var twoHourWorkload = TimeSpan.FromHours(2);
        work.AddWorkload(oneHourWorkload);

        work.ChangeWorkload(twoHourWorkload);

        Assert.Equal(twoHourWorkload, work.Workload);
    }

    [Fact]
    public void Error_when_changing_workload_to_negative_workload()
    {
        var work = CreateWork();
        var oneHourWorkload = TimeSpan.FromHours(1);
        var negativeTwoHourWorkload = TimeSpan.FromHours(-2);
        work.AddWorkload(oneHourWorkload);

        var changeWorkloadAction = work.ChangeWorkload;

        Assert.Throws<Exception>(() => changeWorkloadAction(negativeTwoHourWorkload));
    }

    [Fact]
    public void Work_is_not_active_after_created()
    {
        var work = CreateWork();

        Assert.False(work.IsActive);
    }

    [Fact]
    public void Start_recording_not_active_work()
    {
        var work = CreateWork();

        work.StartRecording(DateTime.Now);

        Assert.True(work.IsActive);
    }

    [Fact]
    public void Error_when_start_recording_active_work()
    {
        var work = CreateWork();

        work.StartRecording(DateTime.Now);
        var startRecordingSecondTimeAction = work.StartRecording;

        Assert.Throws<Exception>(() => startRecordingSecondTimeAction(DateTime.Now));
    }

    [Fact]
    public void Stop_recording_active_work()
    {
        var work = CreateWork();
        work.StartRecording(DateTime.Now);

        work.StopRecording(DateTime.Now);

        Assert.False(work.IsActive);
    }

    [Fact]
    public void Error_when_stop_recording_not_active_work()
    {
        var work = CreateWork();

        var stopRecordingAction = work.StopRecording;

        Assert.Throws<Exception>(() => stopRecordingAction(DateTime.Now));
    }

    [Fact]
    public void Add_workload_manually()
    {
        var work = CreateWork();
        var oneHourWorkload = TimeSpan.FromHours(1);
        var twoHourWorkload = TimeSpan.FromHours(2);

        work.AddWorkload(oneHourWorkload);

        Assert.Equal(oneHourWorkload, work.Workload);

        work.AddWorkload(oneHourWorkload);

        Assert.Equal(twoHourWorkload, work.Workload);
    }

    [Fact]
    public void Error_when_manually_adding_workload_results_negative_total_workload()
    {
        var work = CreateWork();
        var oneHourWorkload = TimeSpan.FromHours(1);
        var negativeTwoHourWorkload = TimeSpan.FromHours(-2);
        work.AddWorkload(oneHourWorkload);

        var addWorkloadSecondTimeAction = work.AddWorkload;

        Assert.Throws<Exception>(() => addWorkloadSecondTimeAction(negativeTwoHourWorkload));
    }

    [Fact]
    public void Add_workload_from_timer()
    {
        var work = CreateWork();
        var startDateTime = DateTime.Parse("01.01.2000 10:00:00");
        var endDateTime = DateTime.Parse("01.01.2000 12:34:56");
        var expectedWorkload = TimeSpan.FromHours(2) + TimeSpan.FromMinutes(34) + TimeSpan.FromSeconds(56);
        work.StartRecording(startDateTime);

        work.StopRecording(endDateTime);

        Assert.Equal(expectedWorkload, work.Workload);
    }


    private static Work CreateWork(string name = "testWork") => new Work(name);
}
using Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Services;

public class WorkdayService
{
    private readonly IRepository<Workday> workdayRepository;
    private readonly IRepository<Work> workRepository;

    public WorkdayService(IRepository<Workday> workdayRepository,
        IRepository<Work> workRepository)
    {
        this.workdayRepository = workdayRepository;
        this.workRepository = workRepository;
    }

    public async Task<Workday?> GetByDate(DateOnly workdayDate)
    {
        return await GetWorkdayQueryable(workdayDate)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<Workday> AddWorkDay(DateOnly workdayDate)
    {
        var newWorkday = new Workday(workdayDate);
        return await workdayRepository.CreateAsync(newWorkday);
    }

    public async Task<Work> AddWork(DateOnly workdayDate, string name)
    {
        var newWork = new Work(name);

        // Проверить первая ли это работа за день
        var workday = await GetWorkdayQueryable(workdayDate)
            .FirstOrDefaultAsync();
        if (workday == null)
        {
            var newWorkday = new Workday(workdayDate);
            newWorkday.AddWork(newWork);
            await workdayRepository.CreateAsync(newWorkday);
        }
        else
        {
            workday.AddWork(newWork);
            await workdayRepository.EditAsync(workday);
        }

        return newWork;
    }

    public async Task<Work> AddWorkload(int workId, TimeSpan workload)
    {
        var work = await workRepository.GetAsync(workId);
        if (work == null)
            throw new Exception($"Work with Id = {workId} not found.");
        work.AddWorkload(workload);
        return await workRepository.EditAsync(work);
    }

    public async Task StartRecording(DateOnly workdayDate, int workId)
    {
        var workday = await GetWorkdayQueryable(workdayDate)
            .FirstOrDefaultAsync();
        if (workday == null)
            throw new Exception($"No work for {workdayDate}.");
        workday.StartRecording(workId, DateTime.Now);
        await workdayRepository.EditAsync(workday);
    }

    public async Task StopRecording(DateOnly workdayDate, int workId)
    {
        var workday = await GetWorkdayQueryable(workdayDate)
            .FirstOrDefaultAsync();
        if (workday == null)
            throw new Exception($"No work for {workdayDate}.");
        workday.StopRecording(workId, DateTime.Now);
        await workdayRepository.EditAsync(workday);
    }

    public async Task RemoveWork(int workId)
    {
        var work = await workRepository.GetAsync(workId);
        if (work == null)
            throw new Exception($"Work with Id = {workId} not found.");

        await workRepository.RemoveAsync(work);
    }

    public async Task EditWork(int workId, string name, TimeSpan workload)
    {
        var work = await workRepository.GetAsync(workId);
        if (work == null)
            throw new Exception($"Work with Id = {workId} not found.");

        work.ChangeName(name);
        work.ChangeWorkload(workload);
        await workRepository.EditAsync(work);
    }

    public async Task<TimeSpan> GetUnaccountedTime(DateOnly workdayDate)
    {
        var now = DateTime.Now;
        if(!IsWorkDay(workdayDate)) return TimeSpan.Zero;

        var settings = SettingsService.Read();
        var workTimeBegin = settings.WorkTimeBegin;
        var workTimeEnd = settings.WorkTimeEnd;
        var lunchDuration = settings.LunchDuration;

        var workTimeDateBegin = new DateTime(workdayDate.Year, workdayDate.Month, workdayDate.Day,
            workTimeBegin.Hours, workTimeBegin.Minutes, 0);
        var workTimeDateEnd = new DateTime(workdayDate.Year, workdayDate.Month, workdayDate.Day,
            workTimeEnd.Hours, workTimeEnd.Minutes, 0);

        if (now < workTimeDateBegin)
            return TimeSpan.Zero;

        var workday = await GetByDate(workdayDate);
        if (workday == null) return TimeSpan.Zero;
        // Если рабочий день прошел, то неучтенное время =
        // общая продолжительность рабочего дня - общие трудозатраты - продолжительность обеда
        // Если рабочий день в процессе, то неучтенное время =
        // продолжительность рабочего дня до текущего момента - общие трудозатраты - продолжительность обеда
        TimeSpan workdayTime = now > workTimeDateEnd
            ? workTimeEnd - workTimeBegin - lunchDuration
            : now - workTimeDateBegin - lunchDuration;

        return workdayTime - workday.TotalWorkload;
    }

    public static bool IsWorkTime(DateTime dateTime)
    {
        var settings = SettingsService.Read();
        var isWorkDay = IsWorkDay(new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day));
        if(!isWorkDay) return false;

        var currentTime = dateTime.TimeOfDay;
        return currentTime >= settings.WorkTimeBegin &&
               currentTime <= settings.WorkTimeEnd;
    }

    public static bool IsWorkDay(DateOnly date)
    {
        var currentDayOfWeek = date.DayOfWeek;
        var settings = SettingsService.Read();
        var isWorkDay = settings.WorkDays.Contains(currentDayOfWeek);
        return isWorkDay;
    }

    private IQueryable<Workday> GetWorkdayQueryable(DateOnly workdayDate)
    {
        return workdayRepository.Items
            .Include(wd => wd.Works)
            .ThenInclude(w => w.WorkloadTimer)
            .Where(wd => wd.Date == workdayDate);
    }
}
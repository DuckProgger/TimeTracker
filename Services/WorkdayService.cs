using Entity;
using Microsoft.EntityFrameworkCore;

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
        return await GetWorkdayQueryable(workdayDate).FirstOrDefaultAsync();
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

    private IQueryable<Workday> GetWorkdayQueryable(DateOnly workdayDate)
    {
        return workdayRepository.Items
            .Include(wd => wd.Works)
            .ThenInclude(w => w.WorkloadTimer)
            .Where(wd => wd.Date == workdayDate);
    }
}
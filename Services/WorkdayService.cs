using Entity;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class WorkdayService
{
    private readonly IRepository<Workday> workdayRepository;
    private readonly IRepository<Work> workRepository;
    private readonly IRepository<WorkloadTimer> workloadTimeRepository;

    public WorkdayService(IRepository<Workday> workdayRepository,
        IRepository<Work> workRepository,
        IRepository<WorkloadTimer> workloadTimeRepository)
    {
        this.workdayRepository = workdayRepository;
        this.workRepository = workRepository;
        this.workloadTimeRepository = workloadTimeRepository;
    }

    public async Task<IEnumerable<Work>> GetWorks(DateOnly workdayDate)
    {
        var workday = await GetWorkdayQueryable(workdayDate)
            .FirstOrDefaultAsync();
        return workday?.Works ?? Enumerable.Empty<Work>();
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

    public async Task<Work> StartRecording(int workId)
    {
        var work = await workRepository.Items
            .Include(w => w.WorkloadTimer)
            .FirstOrDefaultAsync(w => w.Id == workId);
        if (work == null)
            throw new Exception($"Work with Id = {workId} not found.");
        work.StartRecording();
        return await workRepository.EditAsync(work);
    }

    public async Task<Work> StopRecording(int workId)
    {
        var work = await workRepository.Items
            .Include(w => w.WorkloadTimer)
            .FirstOrDefaultAsync(w => w.Id == workId);
        if (work == null)
            throw new Exception($"Work with Id = {workId} not found.");
        work.StopRecording();
        return await workRepository.EditAsync(work);
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
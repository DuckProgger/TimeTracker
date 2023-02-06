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
            var newWorkday = new Workday()
            {
                Date = workdayDate,
                Works = new List<Work>() { newWork }
            };
            await workdayRepository.CreateAsync(newWorkday);
        }
        else
        {
            workday.Works.Add(newWork);
            await workdayRepository.EditAsync(workday);
        }

        return newWork;
    }

    public async Task<Work> AddWorkload(int workId, TimeSpan workload)
    {
        var work = await workRepository.GetAsync(workId);
        if (work == null)
            throw new Exception($"Work with Id = {workId} not found.");
        work.Workload += workload;
        return await workRepository.EditAsync(work);
    }

    private IQueryable<Workday> GetWorkdayQueryable(DateOnly workdayDate)
    {
        return workdayRepository.Items
            .Include(wd => wd.Works)
            .Where(wd => wd.Date == workdayDate);
    }
}
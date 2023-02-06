using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Microsoft.EntityFrameworkCore;
using Services.Infrastructure;

namespace Services;

public class WorkService
{
    private readonly IRepository<Work> workRepository;
    private readonly IRepository<Workday> workdayRepository;

    public WorkService(IRepository<Work> workRepository,
        IRepository<Workday> workdayRepository)
    {
        this.workRepository = workRepository;
        this.workdayRepository = workdayRepository;
    }

    public async Task<Work> AddWork(string name)
    {
        var newWork = new Work(name);

        // Проверить первая ли это работа за день
        var today = DateOnlyHelper.Today();
        var todayWorkday = await workdayRepository.Items.Include(wd => wd.Works).FirstOrDefaultAsync(wd => wd.Date == today);
        if (todayWorkday == null)
        {
            var newWorkday = new Workday()
            {
                Date = today,
                Works = new List<Work>() { newWork }
            };
            await workdayRepository.CreateAsync(newWorkday);
        }
        else
        {
            todayWorkday.Works.Add(newWork);
            await workdayRepository.EditAsync(todayWorkday);
        }

        return newWork;
    }
}
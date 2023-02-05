using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Services;

public class WorkService
{
    private readonly IRepository<Work> workRepository;
    private readonly WorkService workService;

    public WorkService(IRepository<Work> workRepository)
    {
        this.workRepository = workRepository;
    }

    public async Task<Work> AddWork(string name)
    {
        var newWork = new Work(name);
        return await workRepository.CreateAsync(newWork);
    }
}
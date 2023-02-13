using Entity;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ScreenshotService
{
    private readonly IRepository<Screenshot> screenshotRepository;

    public ScreenshotService(IRepository<Screenshot> screenshotRepository)
    {
        this.screenshotRepository = screenshotRepository;
    }

    public async Task SaveScreenshot(byte[] data, DateTime created)
    {
        await screenshotRepository.CreateAsync(new Screenshot(data, created));
    }

    public async Task<IEnumerable<Screenshot>> GetByDay(DateOnly date)
    {
        return await screenshotRepository.Items.Where(s => s.Created.Year == date.Year &&
                                                    s.Created.Month == date.Month &&
                                                    s.Created.Day == date.Day)
            .ToListAsync();
    }
}
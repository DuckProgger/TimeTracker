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

    public async Task<int> GetScreensotsCountByDay(DateOnly date)
    {
        return await GetScreenshotsByDayQuery(date)
            .CountAsync();
    }

    public async Task<IEnumerable<Screenshot>> GetScreenshotBunchByDay(DateOnly date, int skip, int take)
    {
        return await GetScreenshotsByDayQuery(date)
            .Skip(skip)
            .Take(take)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    private IQueryable<Screenshot> GetScreenshotsByDayQuery(DateOnly date)
    {
        return screenshotRepository.Items
                .AsNoTracking()
                .Where(s => s.Created.Year == date.Year &&
                            s.Created.Month == date.Month &&
                            s.Created.Day == date.Day);
    }
}
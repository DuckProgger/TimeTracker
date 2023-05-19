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

    public async Task RemoveScreenshotsByDay(DateOnly date)
    {
        var screenshots = await GetScreenshotsByDayQuery(date)
            .ToListAsync();
        foreach (var screenshot in screenshots)
            await screenshotRepository.RemoveAsync(screenshot);
    }

    public async Task<IEnumerable<Screenshot>> GetScreenshotBunchByDay(DateOnly date, int skip, int take)
    {
        return await GetScreenshotsByDayQuery(date)
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<DateOnly>> GetAllScreenshotDates()
    {
        return await screenshotRepository.Items
            .Select(s => s.Created)
            .GroupBy(s => new { s.Year, s.Month, s.Day })
            .Select(g => new DateOnly(g.Key.Year, g.Key.Month, g.Key.Day))
            .ToListAsync();
    }

    public async Task ClearOutdatedScreenshots()
    {
        var settings = SettingsService.Read();
        var screenshotsLifetimeFromDays = settings.ScreenshotsLifetimeFromDays;
        var dateOfOutdatedScreenshots = DateTime.Now - TimeSpan.FromDays(screenshotsLifetimeFromDays);
        var screenshotDates = await GetAllScreenshotDates();
        var outdatedScreenshotsDates = screenshotDates
            .Where(s => s.Year <= dateOfOutdatedScreenshots.Year &&
                        s.Month <= dateOfOutdatedScreenshots.Month &&
                        s.Day <= dateOfOutdatedScreenshots.Day);
        foreach (var outdatedScreenshotDate in outdatedScreenshotsDates)
            await RemoveScreenshotsByDay(outdatedScreenshotDate);
    }

    private IQueryable<Screenshot> GetScreenshotsByDayQuery(DateOnly date)
    {
        return screenshotRepository.Items
                .Where(s => s.Created.Year == date.Year &&
                            s.Created.Month == date.Month &&
                            s.Created.Day == date.Day);
    }
}
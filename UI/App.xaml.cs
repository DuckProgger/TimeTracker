using Database;
using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using Prism.Modularity;
using Services;
using System;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows;
using UI.DependencyInjection;
using UI.Infrastructure;
using UI.Modules;
using UI.Services;
using UI.Views;

namespace UI;

public partial class App
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry
            .AddDatabase()
            .AddDialogs()
            .Register<ScreenshotService>()
            .Register<WorkdayService>()
            ;
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog
            .AddModule(typeof(MainModule))
            ;
    }

    //protected override void ConfigureViewModelLocator()
    //{
    //    base.ConfigureViewModelLocator();
    //    //ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
    //    //ViewModelLocationProvider.Register<WorksView, WorksViewModel>();
    //}

    protected override Window CreateShell() => Container.Resolve<MainWindow>();

    protected override void Initialize()
    {
        base.Initialize();

        var context = ServiceLocator.GetService<ApplicationContext>();
        context.Database.Migrate();
        ClearOutdatedScreenshots();
        RunSaveScreenshotBackgroundWorker();
    }

    internal static void RunSaveScreenshotBackgroundWorker()
    {
        var worker = new RepeatableBackgroundWorker(TimeSpan.Zero, TimeSpan.FromMinutes(15));
        worker.DoWork += async (s, e) =>
        {
            if (!IsWorkTime(DateTime.Now)) return;
            var imageBytes = ScreenshotHelper.CreateScreenshot(ImageFormat.Png);
            var screenshotService = ServiceLocator.GetService<ScreenshotService>();
            await screenshotService.SaveScreenshot(imageBytes, DateTime.Now);
        };
        worker.RunWorkerAsync();
    }

    private static async void ClearOutdatedScreenshots()
    {
        var settings = SettingsService.Read();
        var screenshotsLifetimeFromDays = settings.ScreenshotsLifetimeFromDays;
        var dateOfOutdatedScreenshots = DateTime.Now - TimeSpan.FromDays(screenshotsLifetimeFromDays);
        var screenshotService = ServiceLocator.GetService<ScreenshotService>();
        var screenshotDates = await screenshotService.GetAllScreenshotDates();
        var outdatedScreenshotsDates = screenshotDates
            .Where(s => s.Year <= dateOfOutdatedScreenshots.Year &&
                        s.Month <= dateOfOutdatedScreenshots.Month &&
                        s.Day <= dateOfOutdatedScreenshots.Day);
        foreach (var outdatedScreenshotDate in outdatedScreenshotsDates)
            await screenshotService.RemoveScreenshotsByDay(outdatedScreenshotDate);
    }

    private static bool IsWorkTime(DateTime dateTime)
    {
        var settings = SettingsService.Read();
        var currentDayOfWeek = dateTime.DayOfWeek;
        var isWorkDay = settings.WorkDays.Contains(currentDayOfWeek);
        if(!isWorkDay) return false;

        var currentTime = dateTime.TimeOfDay;
        var isWorkTime = currentTime >= settings.WorkTimeBegin &&
                         currentTime <= settings.WorkTimeEnd;
        if(!isWorkTime) return false;

        return true;
    }

}
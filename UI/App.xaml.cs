using Database;
using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using Prism.Modularity;
using Services;
using System;
using System.Drawing.Imaging;
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
        var screenshotService = ServiceLocator.GetService<ScreenshotService>();
        screenshotService.ClearOutdatedScreenshots().GetAwaiter().GetResult();
        RunSaveScreenshotBackgroundWorker();
    }

    internal static void RunSaveScreenshotBackgroundWorker()
    {
        var settings = SettingsService.Read();
        var screenshotCreationPeriod = settings.ScreenshotCreationPeriodFromMinutes;
        var nowMinute = DateTime.Now.Minute;
        // Запускать воркера только во время, кратное настройке ScreenshotCreationPeriodFromMinutes
        var startWorkerOffset = screenshotCreationPeriod - nowMinute % screenshotCreationPeriod;
        var worker = new RepeatableBackgroundWorker(TimeSpan.FromMinutes(startWorkerOffset), TimeSpan.FromMinutes(screenshotCreationPeriod));
        worker.DoWork += async (s, e) =>
        {
            if (!WorkdayService.IsWorkTime(DateTime.Now)) return;
            var imageBytes = ScreenshotHelper.CreateScreenshot(ImageFormat.Png);
            var screenshotService = ServiceLocator.GetService<ScreenshotService>();
            await screenshotService.SaveScreenshot(imageBytes, DateTime.Now);
        };
        worker.RunWorkerAsync();
    }
}
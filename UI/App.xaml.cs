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
        RunSaveScreenshotBackgroundWorker(screenshotService);
    }

    internal static void RunSaveScreenshotBackgroundWorker(ScreenshotService screenshotService)
    {
        var worker = new RepeatableBackgroundWorker(TimeSpan.Zero, TimeSpan.FromMinutes(15));
        worker.DoWork += async (s, e) =>
        {
            var imageBytes = ScreenshotHelper.CreateScreenshot(ImageFormat.Png);
            await screenshotService.SaveScreenshot(imageBytes, DateTime.Now);
        };
        worker.RunWorkerAsync();
    }
}
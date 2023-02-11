using Database;
using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using UI.DependencyInjection;
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
            ;
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) {
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
    }
}
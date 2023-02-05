using Database;
using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using System.Windows;
using Prism.Modularity;
using Prism.Mvvm;
using UI.DependencyInjection;
using UI.Modules;
using UI.ViewModels;
using UI.Views;
using WorksView = UI.Views.WorksView;

namespace UI;

public partial class App
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.AddDatabase();
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
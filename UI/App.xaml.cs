using Database;
using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using System.Windows;
using UI.DependencyInjection;
using UI.View;

namespace UI;

public partial class App
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.AddDatabase();
    }

    protected override Window CreateShell() => Container.Resolve<MainWindow>();

    protected override void Initialize()
    {
        base.Initialize();

        var context = ServiceLocator.GetService<ApplicationContext>();
        context.Database.Migrate();
    }
}
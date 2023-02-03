using System.Windows;
using Database;
using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using UI.DependencyInjection;

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
        var context = ServiceLocator.GetService<ApplicationContext>();
        context.Database.Migrate();
    }
}
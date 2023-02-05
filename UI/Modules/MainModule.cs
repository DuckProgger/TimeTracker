using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UI.Infrastructure.Constants;
using UI.Views;

namespace UI.Modules;

/// <summary>
/// Представляет модуль для навигации по представлениям из главного экрана.
/// </summary>
internal class MainModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
        regionManager
             .RegisterViewWithRegion(RegionNames.Main, typeof(WorksView))
        ;

    }
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<WorksView>();
    }
}

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using UI.Infrastructure.Constants;
using UI.Views;

namespace UI.Modules;

internal class DialogModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
        regionManager
            .RegisterViewWithRegion(RegionNames.Dialog, typeof(AddWorkloadManuallyView))
            ;
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<AddWorkloadManuallyView>();

    }
}

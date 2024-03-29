﻿using Prism.Ioc;
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
             .RegisterViewWithRegion(RegionNames.Main, typeof(WorkdayView))
             .RegisterViewWithRegion(RegionNames.Main, typeof(ScreenshotsView))
             .RegisterViewWithRegion(RegionNames.Main, typeof(SettingsView))
        ;

    }
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<WorkdayView>();
        containerRegistry.RegisterForNavigation<ScreenshotsView>();
        containerRegistry.RegisterForNavigation<SettingsView>();
    }
}

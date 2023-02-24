using System.Windows.Input;
using Prism.Commands;
using Prism.Regions;
using UI.Infrastructure.Constants;

namespace UI.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private readonly IRegionManager regionManager;

    public MainWindowViewModel(IRegionManager regionManager)
    {
        this.regionManager = regionManager;
    }

    #region Command NavigateToOtherView - Команда переключиться на другое представление

    private ICommand? _NavigateToOtherViewCommand;
    /// <summary>Команда - переключиться на другое представление</summary>
    public ICommand NavigateToOtherViewCommand => _NavigateToOtherViewCommand
        ??= new DelegateCommand<string>(OnNavigateToOtherViewCommandExecuted);
    private void OnNavigateToOtherViewCommandExecuted(string viewName) =>
        regionManager.RequestNavigate(RegionNames.Main, viewName);

    #endregion
}
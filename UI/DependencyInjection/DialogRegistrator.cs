using Prism.Ioc;
using UI.Views;

namespace UI.DependencyInjection;

/// <summary>
/// Класс для регистрации диалоговых окон.
/// </summary>
internal static class DialogRegistrator
{
    public static IContainerRegistry AddDialogs(this IContainerRegistry container)
    {
        container.RegisterDialog<AddWorkloadManuallyView>();
        container.RegisterDialog<WorkView>();
        container.RegisterDialog<ScreenshotView>();
        return container;
    }
}

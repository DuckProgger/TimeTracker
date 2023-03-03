using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI.Templates;

public class TimePicker : TextBox
{
    private static readonly Regex onlyNumberRegex = new Regex("[0-9]+", RegexOptions.Compiled);
    private bool init = false;

    private void ProcessText(object sender, TextChangedEventArgs e)
    {
        if (init) return;
        init = true;
        Select(0, Text.Length);
        Focus();
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        TextChanged += ProcessText;
        PreviewTextInput += OnPreviewTextInput;
    }

    private void OnSelectionChanged(object sender, RoutedEventArgs e)
    {
        e.Handled = true;
        if (SelectionLength != 1)
            SelectionLength = 1;
    }

    private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !onlyNumberRegex.IsMatch(e.Text);
    }
}
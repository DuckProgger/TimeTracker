using System.Globalization;
using System.Windows.Controls;
using UI.Infrastructure;

namespace UI.Validation.Rules;

internal class IncorrectTimeSpanFormatValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        var correct = DateTimeUtils.CheckCorrectTimeFormat(value as string);
        return correct
            ? ValidationResult.ValidResult
            : new ValidationResult(false, "Incorrect format.");
    }
}
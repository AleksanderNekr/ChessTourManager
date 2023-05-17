using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ChessTourManager.WPF.Helpers.ValidationRules;

public class EmailValidationRule : ValidationRule
{
    /// <inheritdoc />
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        // Check email in regexp.
        Regex regex = new(@"^([a-zA-Z0-9_\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        if (regex.IsMatch(value?.ToString() ?? string.Empty))
        {
            return ValidationResult.ValidResult;
        }

        return new ValidationResult(false, "Некорректный email.");
    }
}

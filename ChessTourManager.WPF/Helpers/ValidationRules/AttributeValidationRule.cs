using System.Globalization;
using System.Windows.Controls;

namespace ChessTourManager.WPF.Helpers.ValidationRules;

public class AttributeValidationRule : ValidationRule
{
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        var input = value?.ToString();
        if (input is null)
        {
            return new ValidationResult(false, null);
        }

        return input.Length == 0 || input.Length > 3 || input.Contains(" ")
                   ? new ValidationResult(false,
                                          "Ячейка должна быть заполнена без пробелов и должна содержать не более 3 символов")
                   : new ValidationResult(true, null);
    }
}

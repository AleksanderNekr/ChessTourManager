using System.Windows.Controls;

namespace ChessTourManager.WPF.Views.ValidationRules;

public class TextValidationRule : ValidationRule
{
    public override ValidationResult Validate(object? value, System.Globalization.CultureInfo cultureInfo)
    {
        var input = value?.ToString();
        if (input is null)
        {
            return new ValidationResult(false, null);
        }

        return input.Length == 0
                   ? new ValidationResult(false, "Ячейка должна быть заполнена")
                   : new ValidationResult(true,  null);
    }
}

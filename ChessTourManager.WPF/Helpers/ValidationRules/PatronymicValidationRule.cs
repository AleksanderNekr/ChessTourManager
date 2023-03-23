using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace ChessTourManager.WPF.Helpers.ValidationRules;

public class PatronymicValidationRule : ValidationRule
{
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        string patronymic = value?.ToString() ?? string.Empty;

        if (patronymic.All(char.IsLetter))
        {
            return ValidationResult.ValidResult;
        }

        return new ValidationResult(false, "Неизвестная ошибка!");
    }
}

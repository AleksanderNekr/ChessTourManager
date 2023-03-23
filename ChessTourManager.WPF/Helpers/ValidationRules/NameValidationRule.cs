using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace ChessTourManager.WPF.Helpers.ValidationRules;

public class NameValidationRule : ValidationRule
{
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        string name           = value?.ToString() ?? string.Empty;
        bool   isNotEmpty     = !string.IsNullOrWhiteSpace(name);
        bool   areLettersOnly = name.All(char.IsLetter);

        if (isNotEmpty && areLettersOnly)
        {
            return ValidationResult.ValidResult;
        }

        if (!areLettersOnly)
        {
            return new ValidationResult(false, "Имя должно содержать только буквы!");
        }

        if (!isNotEmpty)
        {
            return new ValidationResult(false, "Имя не может быть пустым!");
        }

        return new ValidationResult(false, "Неизвестная ошибка!");
    }
}

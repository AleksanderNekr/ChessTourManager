using System;
using System.Globalization;
using System.Windows.Controls;

namespace ChessTourManager.WPF.Helpers.ValidationRules;

public class NameValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        string name = value.ToString() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(name) && name.Length > 2)
        {
            return ValidationResult.ValidResult;
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return new ValidationResult(false, "Поле не может быть пустым!");
        }

        if (name.Length <= 2)
        {
            return new ValidationResult(false, "Имя должно содержать более 2 символов!");
        }

        return new ValidationResult(false, "Неизвестная ошибка!");
    }
}

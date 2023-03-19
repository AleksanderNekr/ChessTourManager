using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ChessTourManager.WPF.Helpers.ValidationRules;

public class PasswordValidationRule : ValidationRule
{
    /// <inheritdoc />
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        string input = value?.ToString() ?? string.Empty;

        if (input.Length < 5)
        {
            return new ValidationResult(false, "Пароль должен быть не менее 5 символов.");
        }

        // Only latin letters, special characters, no whitespace symbols.
        Regex regex = new(@"^[a-zA-Z0-9!@#%^&*()_+\=\[\]':""\\|,.<>\/?]*$");

        if (regex.IsMatch(input))
        {
            return ValidationResult.ValidResult;
        }

        return new ValidationResult(false, "Пароль должен содержать только латинские буквы, цифры и специальные символы.");
    }
}

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace HunterPie.UI.Architecture.Validation;
public class UsernameValidationRule : ValidationRule
{
    private const string ERROR_RANGE_USERNAME = "VALIDATION_SHOULD_BE_WITHIN_RANGE_STRING";
    private const string ERROR_MUST_START_WITH_CHAR = "VALIDATION_INVALID_USERNAME_STRING";
    private const string USERNAME_PATTERN = @"^([^\x00-\x7F]|[\w_\ \.\+\-]){3,20}$";

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is not string username)
            throw new ArgumentException("value must be a string");

        if (string.IsNullOrEmpty(username))
            return new ValidationResult(true, ERROR_RANGE_USERNAME);

        if (username.Length is < 3 or > 20)
            return new ValidationResult(false, ERROR_RANGE_USERNAME);

        if (!Regex.IsMatch(username, USERNAME_PATTERN))
            return new ValidationResult(false, ERROR_MUST_START_WITH_CHAR);

        return new ValidationResult(true, null);
    }
}
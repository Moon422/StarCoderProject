using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Backend.Validators;

public class PasswordValidatorAttribute : ValidationAttribute
{
    private readonly int minLength;
    private readonly int maxLength;
    private readonly bool digit;
    private readonly bool uppercase;
    private readonly bool lowercase;
    private readonly bool symbol;

    public PasswordValidatorAttribute(int minLength = 8, int maxLength = 20, bool digit = true, bool uppercase = true, bool lowercase = true, bool symbol = true)
    {
        this.minLength = minLength;
        this.maxLength = maxLength;
        this.digit = digit;
        this.uppercase = uppercase;
        this.lowercase = lowercase;
        this.symbol = symbol;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string password = (string)value;

        string digitPattern = @"\d";
        string upperPattern = @"[A-Z]";
        string lowerPattern = @"[a-z]";
        string symbolPattern = @"[$#@!^&]";

        if (password.Length < minLength)
        {
            return new ValidationResult($"Password length must be greater than {minLength} characters");
        }
        else if (password.Length > maxLength)
        {
            return new ValidationResult($"Password length must be less than {maxLength} characters");
        }
        else if (digit && !new Regex(digitPattern).IsMatch(password))
        {
            return new ValidationResult($"Password must contain at least one digit");
        }
        else if (uppercase && !new Regex(upperPattern).IsMatch(password))
        {
            return new ValidationResult($"Password must contain at least one uppercase alphabet");
        }
        else if (lowercase && !new Regex(lowerPattern).IsMatch(password))
        {
            return new ValidationResult($"Password must contain at least one lowercase alphabet");
        }
        else if (symbol && !new Regex(symbolPattern).IsMatch(password))
        {
            return new ValidationResult($"Password must contain at least one symbol from the following: $, #, @, !, ^, &");
        }

        return ValidationResult.Success;
    }
}

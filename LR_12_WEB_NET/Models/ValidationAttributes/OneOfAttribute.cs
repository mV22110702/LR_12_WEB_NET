using System.ComponentModel.DataAnnotations;

namespace LR6_WEB_NET.Models.ValidationAttributes;

public class OneOfAttribute<T> : ValidationAttribute
{
    private readonly bool _canBeNull = false;
    private readonly List<T> _validValues = new();

    public OneOfAttribute(params T[] validValues)
    {
        _validValues = validValues.ToList();
    }

    protected string GetErrorMessage(object? value, string propertyName)
    {
        if (!_canBeNull && value == null)
            return $"{propertyName} cannot be null";
        return $"{propertyName} must be one of values: {string.Join(", ", _validValues)}";
    }

    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        if (_validValues.Count == 0)
            return new ValidationResult("No valid values provided");
        if (value == null) return new ValidationResult(GetErrorMessage(value, validationContext.DisplayName));
        if (!_validValues.Contains((T)value))
            return new ValidationResult(GetErrorMessage(value, validationContext.DisplayName));
        return ValidationResult.Success;
    }
}
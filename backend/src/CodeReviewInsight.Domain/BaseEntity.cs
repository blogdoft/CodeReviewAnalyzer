using FluentValidation;
using FluentValidation.Results;

namespace CodeReviewInsight.Domain;

public abstract class BaseEntity<T> : IValidatableClass
    where T : BaseEntity<T>
{
    public bool IsValid() => Validation().IsValid;

    public virtual ValidationResult Validation()
    {
        var validator = GetValidatorInstance();
        if (this is not T validationInstance)
        {
            throw new ValidationException($"The provided validator {validator.GetType().FullName} to type {this.GetType().FullName} is not valid.");
        }

        var validationResult = validator.Validate(validationInstance);

        return validationResult;
    }

    protected abstract AbstractValidator<T> GetValidatorInstance();
}

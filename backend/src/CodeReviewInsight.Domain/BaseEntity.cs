using FluentValidation;
using FluentValidation.Results;

namespace CodeReviewInsight.Domain;

public abstract class BaseEntity<T> : IValidatableClass
    where T : BaseEntity<T>
{
    private const string ValidationErrorMsg = "The provided validator {0} to type {1} is not valid.";

    public bool IsValid() => Validation().IsValid;

    public virtual ValidationResult Validation()
    {
        var validator = GetValidatorInstance();
        var instanceToValidate = this as T ??
            throw new ValidationException(
                string.Format(
                    ValidationErrorMsg,
                    validator.GetType().FullName,
                    GetType().FullName));

        var validationResult = validator.Validate(instanceToValidate);

        return validationResult;
    }

    protected abstract AbstractValidator<T> GetValidatorInstance();
}

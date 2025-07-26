using FluentValidation.Results;

namespace CodeReviewInsight.Domain;

public interface IValidatableClass
{
    bool IsValid();
    ValidationResult Validation();
}

using CodeReviewInsight.Domain.Features.Configurations.Entities;
using FluentValidation;

namespace CodeReviewInsight.Domain.Features.Configurations.Validators;

public class TenantValidator : AbstractValidator<Tenant>
{
    public TenantValidator()
    {
        RuleFor(t => t.Id).NotNull();
        RuleFor(t => t.Name).Length(1, 255);
        RuleFor(t => t.DataSource).NotNull();
    }
}

using CodeReviewInsight.Domain.Enums;

namespace CodeReviewInsight.Domain.Exceptions;

public class UnsuportedDataSourceTypeException(IntegrationType integrationType) :
    DomainException($"Integration type {integrationType.ToString()} is not supported")
{
}

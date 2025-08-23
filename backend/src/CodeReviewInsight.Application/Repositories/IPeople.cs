using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewInsight.Application.Integrations.Models;
using CodeReviewInsight.Application.Models.PagingModels;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.People;

namespace CodeReviewInsight.Application.Repositories;

public interface IPeople
{
    Task<Person> UpsertPersonAsync(TenantId tenantId, Person person);

    Task<Person> UpdatePerson(TenantId tenantId, Person person);

    Task<Person?> GetByIdAsync(TenantId tenantId, Guid personId);

    Task UpsertAsync(IntegrationPerson createdBy);

    Task<PageReturn<IEnumerable<Person>>> GetAllAsync(
        Guid tenantId,
        string? personName,
        bool? status,
        PageFilter pageFilter);
}

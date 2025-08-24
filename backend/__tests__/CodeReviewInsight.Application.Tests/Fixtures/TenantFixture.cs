using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;

namespace CodeReviewInsight.Application.Tests.Fixtures;

public static class TenantFixture
{
    private static readonly Faker _faker = BogusFixture.Get();

    public static Tenant Build() => Build(1)[0];

    public static List<Tenant> Build(int qtd) =>
        Enumerable.Range(0, qtd).Select(_ =>
            new TenantBuilder()
                .WithId(Guid.NewGuid())
                .WithName(_faker.Company.CompanyName())
                .WithDataSource(new AutoFaker<AzureDevOps>().Generate(2))
                .Build())
            .ToList();
}

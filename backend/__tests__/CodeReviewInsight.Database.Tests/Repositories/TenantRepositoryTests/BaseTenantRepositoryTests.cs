using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewInsight.Database.Repositories;
using CodeReviewInsight.Database.ResultSets;
using CodeReviewInsight.Domain.Enums;
using CodeReviewInsight.Domain.Features.Configurations;
using System.Data;
using System.Text.Json;

namespace CodeReviewInsight.Database.Tests.Repositories.TenantRepositoryTests;

public class BaseTenantRepositoryTests
{
    public BaseTenantRepositoryTests()
    {
        DatabaseFacade = Substitute.For<IDatabaseFacade>();
        Connection = Substitute.For<IDbConnection>();
        Transaction = Substitute.For<IDbTransaction>();
        DatabaseFacade
            .GetDbConnection()
            .Returns(Connection);
        Connection
            .BeginTransaction()
            .Returns(Transaction);
    }

    protected IDatabaseFacade DatabaseFacade { get; }
    protected IDbConnection Connection { get; }
    protected IDbTransaction Transaction { get; }
    protected Faker Faker { get; } = BogusFixture.Get();

    protected static T GetProp<T>(object anonymous, string name)
    {
        var prop = anonymous.GetType().GetProperty(name)!;
        return (T)prop.GetValue(anonymous)!;
    }

    protected TenantResultSet BuildTenantResultSet(TenantId id) => new()
    {
        SharedKey = id,
        Active = Faker.Random.Bool(),
        Name = Faker.Company.CompanyName(),
    };

    protected DataSourceResultSet[] BuildDataSourceResultSet(IntegrationType? integrationType = null) => [
        new DataSourceResultSet()
        {
            IntegrationType = integrationType ?? Faker.Random.Enum<IntegrationType>(),
            Name = Faker.Company.CompanyName(),
            Pat = Guid.NewGuid().ToString(),
            DevOpsUrl = Faker.Internet.Url(),
            Areas = JsonSerializer.Serialize(Faker.Random.WordsArray(3)),
            Projects = JsonSerializer.Serialize(Faker.Random.WordsArray(3)),
            Active = Faker.Random.Bool(),
        }];

    internal TenantRepository BuildTenantRepository() => new TenantRepository(DatabaseFacade);
}

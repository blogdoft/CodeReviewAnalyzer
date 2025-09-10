using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using CodeReviewInsight.Domain.Features.GitRepositories;
using System.Data;

namespace CodeReviewInsight.Domain.Tests.Features.GitRepositories;


public class GitRepositoryFactoryTests
{
    private static readonly Faker _faker = BogusFixture.Get();
    private static string FakeRepoName() =>
        _faker.Lorem.Word() + "-" + _faker.Random.AlphaNumeric(6);

    private static string FakeHttpsRepoUrl() =>
        $"https://example.com/{_faker.Internet.UserName()}/{_faker.Random.AlphaNumeric(8)}.git";

    [Fact]
    public void Should_BuildRepositoryWithProvidedFields_When_AllMandatoryFieldsAreSet()
    {
        // Given
        var id = Guid.NewGuid();
        var name = FakeRepoName();
        var url = FakeHttpsRepoUrl();
        TenantId tenantId = Guid.NewGuid();

        var factory = new GitRepositoryFactory()
            .WithId(id)
            .WithName(name)
            .WithUrl(url)
            .WithTenant(tenantId);

        // When
        var repo = factory.Build();

        // Then
        repo.Id.ShouldBe(id);
        repo.Name.ShouldBe(name);
        repo.Url.ShouldNotBeNull();
        repo.Url!.AbsoluteUri.ShouldBe(new Uri(url).AbsoluteUri);
        repo.TenantId.ShouldBe(tenantId);
    }

    [Fact]
    public void Should_RespectProvidedId_When_WithIdIsUsed()
    {
        // Given
        var id = Guid.NewGuid();
        var factory = new GitRepositoryFactory()
            .WithId(id)
            .WithName(FakeRepoName())
            .WithUrl(FakeHttpsRepoUrl())
            .WithTenant((TenantId)Guid.NewGuid());

        // When
        var repo = factory.Build();

        // Then
        repo.Id.ShouldBe(id);
    }

    [Fact]
    public void Should_AllowFluentChaining_When_MethodsAreChained()
    {
        // Given
        var id = Guid.NewGuid();
        var name = FakeRepoName();
        var url = FakeHttpsRepoUrl();
        TenantId tenantId = Guid.NewGuid();

        var factory = new GitRepositoryFactory()
            .WithId(id)
            .WithName(name)
            .WithUrl(url)
            .WithTenant(tenantId);

        // When
        var repo = factory.Build();

        // Then
        repo.Id.ShouldBe(id);
        repo.Name.ShouldBe(name);
        repo.Url!.AbsoluteUri.ShouldBe(new Uri(url).AbsoluteUri);
        repo.TenantId.ShouldBe(tenantId);
    }

    [Fact]
    public void Should_GenerateNewId_When_IdWasNotProvided()
    {
        // Given
        var factory = new GitRepositoryFactory()
            .WithName(FakeRepoName())
            .WithUrl(FakeHttpsRepoUrl())
            .WithTenant((TenantId)Guid.NewGuid());

        // When
        var repo = factory.Build();

        // Then
        repo.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void Should_GenerateDifferentIdsAcrossBuilds_When_IdWasNotProvided()
    {
        // Given
        var factory = new GitRepositoryFactory()
            .WithName(FakeRepoName())
            .WithUrl(FakeHttpsRepoUrl())
            .WithTenant((TenantId)Guid.NewGuid());

        // When
        var repo1 = factory.Build();
        var repo2 = factory.Build();

        // Then
        repo1.Id.ShouldNotBe(repo2.Id);
    }

    [Fact]
    public void Should_ThrowNoNullAllowed_When_NameWasNotProvided()
    {
        // Given
        var factory = new GitRepositoryFactory()
            .WithUrl(FakeHttpsRepoUrl())
            .WithTenant((TenantId)Guid.NewGuid());

        // When
        var ex = Should.Throw<NoNullAllowedException>(() => factory.Build());

        // Then
        ex.Message.ShouldContain("You must provide a name when creating a repository.");
    }

    [Fact]
    public void Should_ThrowNoNullAllowed_When_UrlWasNotProvided()
    {
        // Given
        var factory = new GitRepositoryFactory()
            .WithName(FakeRepoName())
            .WithTenant((TenantId)Guid.NewGuid());

        // When
        var ex = Should.Throw<NoNullAllowedException>(() => factory.Build());

        // Then
        ex.Message.ShouldContain("You must provide a URL when creating a repository.");
    }

    [Fact]
    public void Should_AcceptEmptyStringName_When_EmptyNameProvided()
    {
        // (Teste de caracterização do comportamento atual)
        // Given
        var factory = new GitRepositoryFactory()
            .WithName(string.Empty)
            .WithUrl(FakeHttpsRepoUrl())
            .WithTenant((TenantId)Guid.NewGuid());

        // When
        var repo = factory.Build();

        // Then
        repo.Name.ShouldBe(string.Empty);
    }

    [Fact]
    public void Should_ThrowArgumentNull_When_WithUrlReceivesNull()
    {
        // Given
        var factory = new GitRepositoryFactory()
            .WithName(FakeRepoName())
            .WithTenant((TenantId)Guid.NewGuid());

        // When / Assert
        Should.Throw<ArgumentNullException>(() => factory.WithUrl(null!));
    }

    [Fact]
    public void Should_ThrowUriFormat_When_WithUrlReceivesInvalidString()
    {
        // Given
        var factory = new GitRepositoryFactory()
            .WithName(FakeRepoName())
            .WithTenant((TenantId)Guid.NewGuid());

        // When / Assert
        Should.Throw<UriFormatException>(() => factory.WithUrl("not an url"));
    }

    [Fact]
    public void Should_PersistAbsoluteUrl_When_WithUrlReceivesAbsoluteUrl()
    {
        // Given
        var url = FakeHttpsRepoUrl();
        var factory = new GitRepositoryFactory()
            .WithName(FakeRepoName())
            .WithUrl(url)
            .WithTenant((TenantId)Guid.NewGuid());

        // When
        var repo = factory.Build();

        // Then
        repo.Url!.IsAbsoluteUri.ShouldBeTrue();
        repo.Url.AbsoluteUri.ShouldBe(new Uri(url).AbsoluteUri);
    }

    [Fact]
    public void Should_SetTenantId_When_WithTenantIsCalledWithTenantId()
    {
        // Given
        TenantId tenantId = Guid.NewGuid();

        var factory = new GitRepositoryFactory()
            .WithName(FakeRepoName())
            .WithUrl(FakeHttpsRepoUrl())
            .WithTenant(tenantId);

        // When
        var repo = factory.Build();

        // Then
        repo.TenantId.ShouldBe(tenantId);
    }

    [Fact]
    public void Should_SetTenantId_When_WithTenantIsCalledWithTenantEntity()
    {
        // Given
        TenantId tenantId = Guid.NewGuid();
        var tenant = new Tenant(tenantId, "Name", []);

        var factory = new GitRepositoryFactory()
            .WithName(FakeRepoName())
            .WithUrl(FakeHttpsRepoUrl())
            .WithTenant(tenant);

        // When
        var repo = factory.Build();

        // Then
        repo.TenantId.ShouldBe(tenantId);
    }

    [Fact]
    public void Should_BeOrderIndependent_When_SettingPropertiesInDifferentOrders()
    {
        // Given
        var id = Guid.NewGuid();
        var name = FakeRepoName();
        var url = FakeHttpsRepoUrl();
        TenantId tenantId = Guid.NewGuid();

        var repoA = new GitRepositoryFactory()
            .WithId(id).WithName(name).WithUrl(url).WithTenant(tenantId)
            .Build();

        var repoB = new GitRepositoryFactory()
            .WithTenant(tenantId).WithUrl(url).WithName(name).WithId(id)
            .Build();

        // Then
        repoA.Id.ShouldBe(repoB.Id);
        repoA.Name.ShouldBe(repoB.Name);
        repoA.Url!.AbsoluteUri.ShouldBe(repoB.Url!.AbsoluteUri);
        repoA.TenantId.ShouldBe(repoB.TenantId);
    }

    [Fact]
    public void Should_CreateNewIndependentInstances_OnEachBuild()
    {
        // Given
        var factory = new GitRepositoryFactory()
            .WithName(FakeRepoName())
            .WithUrl(FakeHttpsRepoUrl())
            .WithTenant((TenantId)Guid.NewGuid());

        // When
        var repo1 = factory.Build();
        var repo2 = factory.Build();

        // Then
        repo1.ShouldNotBeSameAs(repo2);
    }
}

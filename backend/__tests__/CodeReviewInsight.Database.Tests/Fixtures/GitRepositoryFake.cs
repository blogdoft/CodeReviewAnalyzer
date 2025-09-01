using CodeReviewInsight.Domain.Features.GitRepositories;

namespace CodeReviewInsight.Database.Tests.Fixtures;

public static class GitRepositoryFake
{
    private static readonly Faker _faker = new("pt_BR");

    public static GitRepository BuildGitRepository() => BuildGitRepository(1)[0];

    public static List<GitRepository> BuildGitRepository(int count) =>
        [.. Enumerable.Range(0, count).Select(_ =>
            new GitRepository()
            {
                Id = Guid.NewGuid(),
                Name = _faker.Random.Words(5),
                Url = new Uri(_faker.Internet.Url()),
                TenantId = Guid.NewGuid(),
            })];
}

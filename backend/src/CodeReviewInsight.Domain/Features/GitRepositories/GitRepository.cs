namespace CodeReviewInsight.Domain.Features.GitRepositories;

public class GitRepository
{
    internal GitRepository()
    {
    }

    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required Uri Url { get; init; }
}

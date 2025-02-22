using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Services;
using CodeReviewAnalyzer.AzureDevopsItg.Extensions;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace CodeReviewAnalyzer.AzureDevopsItg.Factories;

public class PullRequestFactory(
    Configuration configuration,
    GitHttpClient gitClient,
    WorkingHourCalculator workingHourCalculator)
{
    public async Task<PullRequest> CreateAsync(GitPullRequest pr, GitRepository repository)
    {
        var fileCount = 0;
        var threads = new List<GitPullRequestCommentThread>();

        var requests = new List<Task>
        {
            Task.Run(async () => fileCount = await CountChangedFilesAsync(gitClient, repository, pr)),
            Task.Run(async () => threads = await gitClient.GetThreadsAsync(repository.Id, pr.PullRequestId)),
        };

        await Task.WhenAll(requests);

        return ToPullRequest(pr, threads, fileCount);
    }

    private static async Task<int> CountChangedFilesAsync(
        GitHttpClient gitClient,
        GitRepository repository,
        GitPullRequest pr)
    {
        var iterations = await gitClient.GetPullRequestIterationsAsync(repository.Id, pr.PullRequestId);
        var lastIteration = iterations.LastOrDefault();
        if (lastIteration?.Id is not null)
        {
            var iterationChanges = await gitClient.GetPullRequestIterationChangesAsync(
                repository.Id.ToString(),
                pr.PullRequestId,
                lastIteration.Id.Value);
            return iterationChanges.ChangeEntries.Count();
        }

        return 0;
    }

    private static string BuildPullRequestUrl(
        Configuration configuration,
        GitPullRequest gitPullRequest) =>
        $"https://dev.azure.com/{configuration.Organization}/{configuration.ProjectName}/_git/{gitPullRequest.Repository.Name}/pullrequest/{gitPullRequest.PullRequestId}";

    private static string ExtractMergeMode(GitPullRequest gitPullRequest)
    {
        if (gitPullRequest.CompletionOptions.MergeStrategy is null)
        {
            return "Unknown";
        }

        return typeof(GitPullRequestMergeStrategy).GetEnumName(gitPullRequest.CompletionOptions.MergeStrategy) ?? "Unknown";
    }

    private static PrComments ToPrComments((GitPullRequestCommentThread Thread, Comment Comment) x)
    {
        return new PrComments
        {
            CommentIndex = x.Comment.Id,
            ThreadId = x.Thread.Id,
            CommentedBy = new User()
            {
                Id = x.Comment.Author.Id,
                Name = x.Comment.Author.DisplayName,
                Active = !x.Comment.Author.Inactive,
            },
            Comment = x.Comment.Content,
            CommentDate = x.Comment.PublishedDate,
            ResolvedDate = x.Comment.LastUpdatedDate,
        };
    }

    private PullRequest ToPullRequest(GitPullRequest gitPullRequest, List<GitPullRequestCommentThread> threads, int fileCount)
    {
        var approvals = threads.SelectMany(t => t.Comments)
            .Where(c =>
                c.CommentType == CommentType.System && (
                c.Content.Contains("approved", StringComparison.OrdinalIgnoreCase) ||
                c.Content.Contains("aprovado", StringComparison.OrdinalIgnoreCase) ||
                c.Content.Contains("voted 10", StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(c => c.PublishedDate);

        var personsComments = threads
                .Where(thread => thread.Status != CommentThreadStatus.Unknown)
                .SelectMany(t => t.Comments, (thread, comment) => (
                    Thread: thread,
                    Comment: comment))
                .Where(x => !x.Comment.IsDeleted
                    && x.Comment.CommentType != CommentType.System
                    && x.Comment.CommentType != CommentType.CodeChange
                    && x.Comment.Author != null)
                .OrderBy(x => x.Thread.PublishedDate);

        return new(workingHourCalculator)
        {
            Id = gitPullRequest.PullRequestId,
            Title = gitPullRequest.Title,
            Repository = ToRepository(gitPullRequest.Repository),
            CreationDate = gitPullRequest.CreationDate,
            ClosedDate = gitPullRequest.ClosedDate,
            Url = BuildPullRequestUrl(configuration, gitPullRequest),
            CreatedBy = gitPullRequest.CreatedBy.ToUser(),
            FileCount = fileCount,
            MergeMode = ExtractMergeMode(gitPullRequest),
            Reviewers = gitPullRequest.Reviewers
                .Aggregate(
                    new List<User>(),
                    (users, reviewer) =>
                    {
                        if (!reviewer.Inactive)
                        {
                            users.Add(reviewer.ToUser());
                        }

                        return users;
                    }),
            FirstCommentDate = personsComments.Any()
                ? personsComments.FirstOrDefault().Thread.PublishedDate
                : gitPullRequest.CreationDate,
            LastApprovalDate = approvals.FirstOrDefault()?.PublishedDate ?? gitPullRequest.ClosedDate,
            Comments = personsComments.Select(ToPrComments),
            ThreadCount = personsComments.Count(),
        };
    }

    private CodeRepository ToRepository(GitRepository repository) => new()
    {
        Name = repository.Name,
        Url = repository.Url,
        ExternalId = repository.Id.ToString(),
    };
}

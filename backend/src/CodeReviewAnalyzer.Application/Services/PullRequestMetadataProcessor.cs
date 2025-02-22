using CodeReviewAnalyzer.Application.Integrations;
using CodeReviewAnalyzer.Application.Integrations.Models;
using CodeReviewAnalyzer.Application.Models;
using CodeReviewAnalyzer.Application.Reports;
using CodeReviewAnalyzer.Application.Repositories;

namespace CodeReviewAnalyzer.Application.Services;

public class PullRequestMetadataProcessor(
    IPullRequestsClient pullRequestsClient,
    IConfigurations configurationRepository,
    ICodeRepository codeRepository,
    IDayOff dayOff,
    IUsers users,
    IPullRequests pullRequestRepository)
{
    private readonly PeriodTimeSpan _morningWorkingTime = new(TimeSpan.FromHours(8), TimeSpan.FromHours(12));
    private readonly PeriodTimeSpan _afternoonWorkingTime = new(TimeSpan.FromHours(13), TimeSpan.FromHours(17));

    public async Task ExecuteAsync(DateOnly begin, DateOnly end)
    {
        var configurations = await configurationRepository.GetAllAsync();
        var holidays = await dayOff.GetAllAsync(
            from: begin,
            to: end);
        var calculator = new WorkingHourCalculator(
            _morningWorkingTime,
            _afternoonWorkingTime,
            holidays.Select(holiday => holiday.Date));

        foreach (var configuration in configurations)
        {
            await ProcessConfigurationAsync(configuration, calculator);
        }
    }

    private async Task ProcessConfigurationAsync(
        Configuration configuration,
        WorkingHourCalculator calculator)
    {
        var pullRequests = pullRequestsClient.GetPullRequestsAsync(
            configuration,
            calculator,
            minTime: new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified),
            maxTime: DateTime.Now);

        await foreach (var pullRequest in pullRequests)
        {
            await ProcessRepositoryAsync(pullRequest);
            await ProcessUserAsync(pullRequest);
            await ProcessPullRequestAsync(pullRequest);
        }
    }

    private async Task ProcessRepositoryAsync(PullRequest pullRequest) =>
        await codeRepository.AddAsync(pullRequest.Repository);

    private async Task ProcessUserAsync(PullRequest pullRequest)
    {
        await users.Upsert(pullRequest.CreatedBy);
        foreach (var reviewer in pullRequest.Reviewers.Union(pullRequest.Comments.Select(c => c.CommentedBy)))
        {
            await users.Upsert(reviewer);
        }
    }

    private async Task ProcessPullRequestAsync(PullRequest pullRequest) =>
        await pullRequestRepository.Add(pullRequest);
}

using CodeReviewInsight.Application.Models.PullRequestReport;

namespace CodeReviewInsight.Application.Services.Crawlers;

public interface IWorkItemsCrawler
{
    Task CrawAsync(ReportFilter filter);
}

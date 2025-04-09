using CodeReviewAnalyzer.Application.Models.PullRequestReport;

namespace CodeReviewAnalyzer.Application.Services.Crawlers;

public interface IWorkItemsCrawler
{
    Task CrawAsync(ReportFilter filter);
}

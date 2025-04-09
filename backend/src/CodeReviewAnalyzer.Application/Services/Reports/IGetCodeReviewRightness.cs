using CodeReviewAnalyzer.Application.Models.PullRequestReport;
using CodeReviewAnalyzer.Application.Models.Reports;

namespace CodeReviewAnalyzer.Application.Services.Reports;

public interface IGetCodeReviewRightness
{
    Task<CodeReviewRightness> EvaluateAsync(ReportFilter filter);
}

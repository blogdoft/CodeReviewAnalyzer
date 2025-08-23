using CodeReviewInsight.Application.Models.PullRequestReport;
using CodeReviewInsight.Application.Models.Reports;

namespace CodeReviewInsight.Application.Services.Reports;

public interface IGetCodeReviewRightness
{
    Task<CodeReviewRightness> EvaluateAsync(ReportFilter filter);
}

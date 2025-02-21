import { MetricStatsOptions } from '../../../shared/components/metric-stats-panel/metric-stats-options';
import { MetricStatus } from '../../../shared/components/metric-stats-panel/metric-status.enum';
import { PullRequestTimeReport, TimeIndex } from '../../service/report/models/pull-request-report.model';

export interface PullRequestStats {
    firstAttempt: MetricStatsOptions;
    closedPullRequest: MetricStatsOptions;
    meanTimeToReview: MetricStatsOptions;
    meanTimeToApprove: MetricStatsOptions;
    meanTimeToMerge: MetricStatsOptions;
}

export class PullRequestStatsService {
    constructor(public report: PullRequestTimeReport) {}

    private emptyTimeIndex: TimeIndex = {
        periodInMinutes: -1,
        referenceDate: new Date().toString()
    };

    public build(): PullRequestStats {
        const sortedPrCount = this.getOrdered(this.report.pullRequestCount);
        const sortedNonApproval = this.getOrdered(this.report.pullRequestWithoutCommentCount);
        return {
            firstAttempt: this.buildFirstAttempt(sortedPrCount, sortedNonApproval),
            closedPullRequest: this.buildPrClosedCount(sortedPrCount),
            meanTimeToReview: this.buildMeanTimeToReview(this.report.meanTimeToStartReview),
            meanTimeToApprove: this.buildMeanTimeToApprove(this.report.meanTimeOpenToApproval),
            meanTimeToMerge: this.buildMeanTimeToMerge(this.report.meanTimeToMerge)
        };
    }

    private getOrdered(timeIndex: TimeIndex[] | null): CurrentPrevious {
        if (!timeIndex) {
            return {
                current: this.emptyTimeIndex,
                previous: this.emptyTimeIndex
            };
        }

        const sorted = timeIndex.sort((a, b) => new Date(a.referenceDate).getTime() - new Date(b.referenceDate).getTime());

        return {
            current: sorted[sorted.length - 1],
            previous: sorted[sorted.length - 2]
        };
    }

    private buildFirstAttempt(prCount: CurrentPrevious, nonApproved: CurrentPrevious): MetricStatsOptions {
        const firstAttempt = this.evalFirstAttempt(prCount, nonApproved);
        let status = MetricStatus.ok;
        const value = parseInt(firstAttempt.current.value);
        if (value < 40) status = MetricStatus.warning;
        if (value > 80) status = MetricStatus.error;
        return {
            title: 'Approved on first attempt',
            status: status,
            current: {
                value: firstAttempt.current.value,
                referenceDate: new Date(firstAttempt.current.referenceDate)
            },
            previous: {
                value: firstAttempt.previous.value.toString(),
                referenceDate: new Date(firstAttempt.previous.referenceDate)
            }
        };
    }

    private buildPrClosedCount(prCount: CurrentPrevious): MetricStatsOptions {
        return {
            title: 'PR Closed',
            status: MetricStatus.ok,
            icon: 'pi-box',
            current: {
                value: prCount.current.periodInMinutes.toString(),
                referenceDate: new Date(prCount.current.referenceDate)
            },
            previous: {
                value: prCount.previous.periodInMinutes.toString(),
                referenceDate: new Date(prCount.previous.referenceDate)
            }
        };
    }

    private buildMeanTimeToReview(meanTimeToReview: TimeIndex[] | null): MetricStatsOptions {
        const sorted = this.getCurrentPreviousHourly(meanTimeToReview);
        let status = MetricStatus.ok;
        if (sorted.current.periodInMinutes > 8) status = MetricStatus.warning;
        if (sorted.current.periodInMinutes > 13) status = MetricStatus.error;

        return {
            title: 'Mean Time to Review',
            status: status,
            current: {
                value: `${sorted.current.periodInMinutes} hours`,
                referenceDate: new Date(sorted.current.referenceDate)
            },
            previous: {
                value: `${sorted.previous.periodInMinutes} hours`,
                referenceDate: new Date(sorted.previous.referenceDate)
            }
        };
    }

    private buildMeanTimeToApprove(meanTimeToApprove: TimeIndex[] | null): MetricStatsOptions {
        const sorted = this.getCurrentPreviousHourly(meanTimeToApprove);
        let status = MetricStatus.ok;
        if (sorted.current.periodInMinutes > 48) status = MetricStatus.warning;
        if (sorted.current.periodInMinutes > 72) status = MetricStatus.error;

        return {
            title: 'Mean Time to Approve',
            status: status,
            current: {
                value: `${sorted.current.periodInMinutes} hours`,
                referenceDate: new Date(sorted.current.referenceDate)
            },
            previous: {
                value: `${sorted.previous.periodInMinutes} hours`,
                referenceDate: new Date(sorted.previous.referenceDate)
            }
        };
    }

    private buildMeanTimeToMerge(meanTimeToMerge: TimeIndex[] | null): MetricStatsOptions {
        const sorted = this.getCurrentPreviousHourly(meanTimeToMerge);
        let status = MetricStatus.ok;
        if (sorted.current.periodInMinutes > 48) status = MetricStatus.warning;
        if (sorted.current.periodInMinutes > 72) status = MetricStatus.error;

        return {
            title: 'Mean Time to Merge',
            status: status,
            current: {
                value: `${sorted.current.periodInMinutes} hours`,
                referenceDate: new Date(sorted.current.referenceDate)
            },
            previous: {
                value: `${sorted.previous.periodInMinutes} hours`,
                referenceDate: new Date(sorted.previous.referenceDate)
            }
        };
    }

    private getCurrentPreviousHourly(content: TimeIndex[] | null): CurrentPrevious {
        if (!content) {
            return {
                current: {
                    periodInMinutes: -1,
                    referenceDate: new Date().toString()
                },
                previous: {
                    periodInMinutes: -1,
                    referenceDate: new Date().toString()
                }
            };
        }

        const sorted = this.getOrdered(content);
        return {
            current: {
                ...sorted.current,
                periodInMinutes: Math.round(sorted.current.periodInMinutes / 60)
            },
            previous: {
                ...sorted.previous,
                periodInMinutes: Math.round(sorted.previous.periodInMinutes / 60)
            }
        };
    }

    private evalFirstAttempt(prCount: CurrentPrevious, prWithoutApprovalCount: CurrentPrevious) {
        const currentValue = (prWithoutApprovalCount.current.periodInMinutes * 100) / prCount.current.periodInMinutes;
        const previousValue = (prWithoutApprovalCount.previous.periodInMinutes * 100) / prCount.previous.periodInMinutes;
        return {
            current: {
                value: `${Math.round(currentValue)}%`,
                referenceDate: prWithoutApprovalCount.current.referenceDate
            },
            previous: {
                value: `${Math.round(previousValue)}%`,
                referenceDate: prWithoutApprovalCount.previous.referenceDate
            }
        };
    }
}

interface CurrentPrevious {
    current: TimeIndex;
    previous: TimeIndex;
}

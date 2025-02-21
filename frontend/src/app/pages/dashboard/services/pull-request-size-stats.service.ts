import { MetricStatsOptions } from '../../../shared/components/metric-stats-panel/metric-stats-options';
import { MetricStatus } from '../../../shared/components/metric-stats-panel/metric-status.enum';
import { PullRequestFileSize, PullRequestTimeReport } from '../../service/report/models/pull-request-report.model';

export class PullRequestSizeStatsService {
    constructor(public report: PullRequestTimeReport) {}

    private emptyFileSize: PullRequestFileSize = {
        maxFileCount: -1,
        meanFileCount: -1,
        minFileCount: -1,
        referenceDate: new Date().toString()
    };

    public build(): { meanFileCount: MetricStatsOptions; maxFileCount: MetricStatsOptions } {
        const sorted = this.getOrdered(this.report.pullRequestSize);

        let status = MetricStatus.ok;
        if (sorted.current.meanFileCount > 20) status = MetricStatus.warning;
        if (sorted.current.meanFileCount > 25) status = MetricStatus.error;

        return {
            meanFileCount: {
                title: 'Mean File count',
                status: status,
                current: {
                    value: `${sorted.current.meanFileCount} files`,
                    referenceDate: this.getDate(sorted.current.referenceDate)
                },
                previous: {
                    value: `${sorted.previous.meanFileCount} files`,
                    referenceDate: this.getDate(sorted.previous.referenceDate)
                }
            },
            maxFileCount: {
                title: 'Max File count',
                status: MetricStatus.ok,
                icon: 'pi-book',
                current: {
                    value: `${sorted.current.maxFileCount} files`,
                    referenceDate: this.getDate(sorted.current.referenceDate)
                },
                previous: {
                    value: `${sorted.previous.maxFileCount} files`,
                    referenceDate: this.getDate(sorted.previous.referenceDate)
                }
            }
        };
    }

    private getOrdered(timeIndex: PullRequestFileSize[] | null): CurrentPrevious {
        if (!timeIndex) {
            return {
                current: this.emptyFileSize,
                previous: this.emptyFileSize
            };
        }

        const sorted = timeIndex.sort((a, b) => new Date(a.referenceDate).getTime() - new Date(b.referenceDate).getTime());

        return {
            current: sorted[sorted.length - 1],
            previous: sorted[sorted.length - 2]
        };
    }

    private getDate(date: string | null): Date {
        if (!date) return new Date();

        return new Date(`${date}T00:00:00`);
    }
}

interface CurrentPrevious {
    current: PullRequestFileSize;
    previous: PullRequestFileSize;
}

import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ChartModule } from 'primeng/chart';

import { MetricStatsOptions } from '../../../shared/components/metric-stats-panel/metric-stats-options';
import { MetricStatsPanelComponent } from '../../../shared/components/metric-stats-panel/metric-stats-panel.component';
import { PullRequestTimeReport } from '../../service/report/models/pull-request-report.model';
import { PullRequestSizeStatsService } from '../services/pull-request-size-stats.service';
import { PullRequestStatsService } from '../services/pull-request-stats.service';

@Component({
    standalone: true,
    selector: 'app-stats-widget',
    imports: [CommonModule, ChartModule, MetricStatsPanelComponent],
    template: `<div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <app-metric-stats-panel [options]="meanTimeToReview" />
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <app-metric-stats-panel [options]="meanTimeToApprove" />
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <app-metric-stats-panel [options]="meanTimeToMerge" />
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <app-metric-stats-panel [options]="prApproval" />
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <app-metric-stats-panel [options]="meanFileCount" />
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <app-metric-stats-panel [options]="closedPullRequest" />
        </div>
        <div class="col-span-12 lg:col-span-6 xl:col-span-3">
            <app-metric-stats-panel [options]="maxFileCount" />
        </div>`
})
export class StatsWidget {
    @Input()
    set pullRequestTimeReport(value: PullRequestTimeReport) {
        this.report = value;
        var prStats = new PullRequestStatsService(value).build();
        var fileStats = new PullRequestSizeStatsService(value).build();
        this.prApproval = prStats.firstAttempt;
        this.closedPullRequest = prStats.closedPullRequest;
        this.meanTimeToReview = prStats.meanTimeToReview;
        this.meanTimeToApprove = prStats.meanTimeToApprove;
        this.meanTimeToMerge = prStats.meanTimeToMerge;
        this.meanFileCount = fileStats.meanFileCount;
        this.maxFileCount = fileStats.maxFileCount;
    }
    get PullRequestTimeReport(): PullRequestTimeReport {
        return this.report;
    }

    public report!: PullRequestTimeReport;

    public prApproval!: MetricStatsOptions;
    public closedPullRequest!: MetricStatsOptions;
    public meanFileCount!: MetricStatsOptions;
    public maxFileCount!: MetricStatsOptions;
    public meanTimeToReview!: MetricStatsOptions;
    public meanTimeToApprove!: MetricStatsOptions;
    public meanTimeToMerge!: MetricStatsOptions;
}

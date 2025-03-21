import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { DatePickerModule } from 'primeng/datepicker';
import { IftaLabelModule } from 'primeng/iftalabel';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';

import { MessageHandlerService } from '../../shared/services/message-handler.service';
import { CommentData } from '../service/report/models/comment-data.model';
import { Outlier } from '../service/report/models/outlier';
import { PullRequestTimeReport } from '../service/report/models/pull-request-report.model';
import { PullRequestReportService } from '../service/report/pullrequest.report.service';
import { DashboardFilter, DateRange } from './components/filter/date-range';
import { FilterComponent } from './components/filter/filter.component';
import { OutliersTableComponent } from './components/outliers-table/outliers-table.component';
import { PullRequestGraphComponent } from './components/pull-request-graph/pull-request-graph.component';
import { ReviewerDensityGraphComponent } from './components/reviewer-density-graph/reviewer-density-graph.component';
import { StatsWidget } from './components/statswidget';
import { forkJoin } from 'rxjs';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [
        AutoCompleteModule,
        DatePickerModule,
        FormsModule,
        StatsWidget,
        PullRequestGraphComponent,
        ToastModule,
        MessageModule,
        ReviewerDensityGraphComponent,
        ToolbarModule,
        IftaLabelModule,
        FilterComponent,
        OutliersTableComponent
    ],
    template: `<p-toast></p-toast>
        <div class="grid grid-cols-12 gap-8">
            <div class="col-span-12">
                <app-filter
                    [rangeDate]="rangeDates"
                    (onSearch)="onSearch($event)"
                ></app-filter>
            </div>
            <div class="col-span-12"></div>
            <app-stats-widget
                class="contents"
                [pullRequestTimeReport]="pullRequestReport"
            />
            <div class="col-span-12">
                <app-pull-request-graph
                    [pullRequestTimeReport]="pullRequestReport"
                ></app-pull-request-graph>
            </div>
            <div class="col-span-12 ">
                <app-reviewer-density-graph
                    [reviewerDensity]="reviewerDensityReport"
                ></app-reviewer-density-graph>
            </div>
            <div class="col-span-12 xl:col-span-6">
                <app-outliers-table [outliers]="outliers" />
            </div>
        </div>`,
    providers: [MessageHandlerService]
})
export class Dashboard implements OnInit {
    public rangeDates!: DateRange;

    public pullRequestReport: PullRequestTimeReport = {
        meanTimeToApprove: [],
        meanTimeToMerge: [],
        meanTimeToStartReview: [],
        pullRequestCount: [],
        pullRequestSize: [],
        approvedOnFirstAttempt: []
    };

    public reviewerDensityReport: CommentData[] = [
        {
            commentCount: -1,
            userId: 'undefined',
            userName: 'undefined',
            referenceDate: new Date().toDateString()
        }
    ];

    outliers!: Outlier[];

    constructor(
        private pullRequestReportService: PullRequestReportService,
        private messageHandler: MessageHandlerService
    ) {}

    ngOnInit(): void {
        this.initializeDateRange();
        const filter: DashboardFilter = {
            teamRepositoryId: null,
            teamUserId: null,
            dateRange: this.rangeDates
        };
        this.onSearch(filter);
    }

    initializeDateRange(): void {
        const today = new Date();
        const threeMonthsAgo = new Date();
        threeMonthsAgo.setMonth(today.getMonth() - 3, 1);
        this.rangeDates = { from: threeMonthsAgo, to: today };
    }

    onSearch($event: DashboardFilter) {
        forkJoin({
            report: this.pullRequestReportService.getReports(
                $event.dateRange.from,
                $event.dateRange.to,
                $event.teamRepositoryId,
                $event.teamUserId
            ),
            density: this.pullRequestReportService.getReviewerDensity(
                $event.dateRange.from,
                $event.dateRange.to,
                $event.teamRepositoryId,
                $event.teamUserId
            ),
            outliers: this.pullRequestReportService.getOutliers(
                $event.dateRange.from,
                $event.dateRange.to,
                $event.teamRepositoryId,
                $event.teamUserId
            )
        }).subscribe({
            next: ({ report, density, outliers }) => {
                this.pullRequestReport = report;
                this.reviewerDensityReport = density;
                this.outliers = outliers;
            },
            error: (err) =>
                this.messageHandler.handleHttpError(
                    err,
                    'Error retrieving report data'
                )
        });
    }
}

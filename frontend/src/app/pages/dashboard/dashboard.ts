import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { DatePickerModule } from 'primeng/datepicker';
import { IftaLabelModule } from 'primeng/iftalabel';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';

import { CommentData } from '../service/report/models/comment-data.model';
import { Outlier } from '../service/report/models/outlier';
import { PullRequestTimeReport } from '../service/report/models/pull-request-report.model';
import { PullRequestReportService } from '../service/report/pullrequest.report.service';
import { BestSellingWidget } from './components/bestsellingwidget';
import { DashboardFilter, DateRange } from './components/filter/date-range';
import { FilterComponent } from './components/filter/filter.component';
import { NotificationsWidget } from './components/notificationswidget';
import { OutliersTableComponent } from './components/outliers-table/outliers-table.component';
import { PullRequestGraphComponent } from './components/pull-request-graph/pull-request-graph.component';
import { RecentSalesWidget } from './components/recentsaleswidget';
import { RevenueStreamWidget } from './components/revenuestreamwidget';
import { ReviewerDensityGraphComponent } from './components/reviewer-density-graph/reviewer-density-graph.component';
import { StatsWidget } from './components/statswidget';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [
        AutoCompleteModule,
        DatePickerModule,
        FormsModule,
        StatsWidget,
        RecentSalesWidget,
        BestSellingWidget,
        RevenueStreamWidget,
        NotificationsWidget,
        PullRequestGraphComponent,
        ToastModule,
        MessageModule,
        ReviewerDensityGraphComponent,
        ToolbarModule,
        IftaLabelModule,
        FilterComponent,
        OutliersTableComponent
    ],
    template: `
        <p-toast></p-toast>
        <div class="grid grid-cols-12 gap-8">
            <div class="col-span-12">
                <app-filter
                    [rangeDate]="rangeDates"
                    (OnSearch)="OnSearch($event)"
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
            <div class="col-span-12 xl:col-span-6">
                <app-recent-sales-widget />
                <app-best-selling-widget />
            </div>
            <div class="col-span-12 xl:col-span-6">
                <app-revenue-stream-widget />
                <app-notifications-widget />
            </div>
        </div>
    `,
    providers: [MessageService]
})
export class Dashboard {
    public rangeDates!: DateRange;

    public pullRequestReport: PullRequestTimeReport = {
        meanTimeOpenToApproval: [],
        meanTimeToMerge: [],
        meanTimeToStartReview: [],
        pullRequestCount: [],
        pullRequestSize: [],
        pullRequestWithoutCommentCount: []
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
        private message: MessageService
    ) {}

    ngOnInit(): void {
        const today = new Date();
        const threeMonthsAgo = new Date();
        threeMonthsAgo.setMonth(today.getMonth() - 3, 1);
        this.rangeDates = { from: threeMonthsAgo, to: today };
        const filter: DashboardFilter = {
            teamRepositoryId: null,
            teamUserId: null,
            dateRange: this.rangeDates
        };
        this.OnSearch(filter);
    }

    OnSearch($event: DashboardFilter) {
        this.pullRequestReportService
            .getReports(
                $event.dateRange.from,
                $event.dateRange.to,
                $event.teamRepositoryId,
                $event.teamUserId
            )
            .subscribe({
                next: (data) => {
                    this.pullRequestReport = data;
                },
                error: (err) => this.showError(err)
            });

        this.pullRequestReportService
            .getReviewerDensity(
                $event.dateRange.from,
                $event.dateRange.to,
                $event.teamRepositoryId,
                $event.teamUserId
            )
            .subscribe({
                next: (data) => (this.reviewerDensityReport = data),
                error: (err) => this.showError(err)
            });
        this.pullRequestReportService
            .getOutliers(
                $event.dateRange.from,
                $event.dateRange.to,
                $event.teamRepositoryId,
                $event.teamUserId
            )
            .subscribe({
                next: (data) => (this.outliers = data),
                error: (err) => this.showError(err)
            });
    }

    private showError(err: any) {
        this.message.add({
            detail: err.message,
            summary: 'Summary',
            severity: 'error',
            life: 50000
        });
    }
}

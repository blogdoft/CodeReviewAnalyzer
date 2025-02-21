import { FloatLabelModule } from 'primeng/floatlabel';
import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';

import { CommentData } from '../service/report/models/comment-data.model';
import { PullRequestTimeReport } from '../service/report/models/pull-request-report.model';
import { PullRequestReportService } from '../service/report/pullrequest.report.service';
import { BestSellingWidget } from './components/bestsellingwidget';
import { NotificationsWidget } from './components/notificationswidget';
import { PullRequestGraphComponent } from './components/pull-request-graph/pull-request-graph.component';
import { RecentSalesWidget } from './components/recentsaleswidget';
import { RevenueStreamWidget } from './components/revenuestreamwidget';
import { ReviewerDensityGraphComponent } from './components/reviewer-density-graph/reviewer-density-graph.component';
import { StatsWidget } from './components/statswidget';
import { ToolbarModule } from 'primeng/toolbar';
import { DatePickerModule } from 'primeng/datepicker';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [
        FloatLabelModule,
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
        ToolbarModule
    ],
    template: `
        <p-toast></p-toast>
        <div class="grid grid-cols-12 gap-8">
            <div class="col-span-12">
                <p-toolbar>
                    <ng-template #start></ng-template>
                    <ng-template #center>
                        <p-floatlabel>
                            <p-datepicker [(ngModel)]="rangeDates" selectionMode="range" [readonlyInput]="true" dateFormat="dd/mm/yy" size="small" showIcon iconDisplay="input" fluid="true" [showButtonBar]="true" inputId="inputPeriod"></p-datepicker>
                            <label for="inputPeriod">Period</label></p-floatlabel
                        >
                    </ng-template>
                    <ng-template #end></ng-template>
                </p-toolbar>
            </div>
            <app-stats-widget class="contents" [pullRequestTimeReport]="pullRequestReport" />
            <div class="col-span-12">
                <app-pull-request-graph [pullRequestTimeReport]="pullRequestReport"></app-pull-request-graph>
            </div>

            <div class="col-span-12 ">
                <app-reviewer-density-graph [reviewerDensity]="reviewerDensityReport"></app-reviewer-density-graph>
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
    public pullRequestReport: PullRequestTimeReport = {
        meanTimeOpenToApproval: [],
        meanTimeToMerge: [],
        meanTimeToStartReview: [],
        pullRequestCount: [],
        pullRequestSize: [],
        pullRequestWithoutCommentCount: []
    };

    set rangeDates(value: Date[] | null) {
        this._rangeDates = value;
        if (value && value[0] != null && value[1] != null) this.searchPullRequests(value);
    }
    get rangeDates(): Date[] | null {
        return this._rangeDates;
    }

    public reviewerDensityReport: CommentData[] = [];
    private _rangeDates: Date[] | null = null;

    constructor(
        private pullRequestReportService: PullRequestReportService,
        private message: MessageService
    ) {}

    ngOnInit(): void {
        const today = new Date();
        const threeMonthsAgo = new Date();
        threeMonthsAgo.setMonth(today.getMonth() - 3, 1);
        this.rangeDates = [threeMonthsAgo, today];
    }

    public searchPullRequests(event: Date[] | null) {
        this.pullRequestReportService.getReports(event![0], event![1]).subscribe({
            next: (data) => {
                this.pullRequestReport = data;
            },
            error: (err) => this.showError(err)
        });

        this.pullRequestReportService.getReviewerDensity(event![0], event![1]).subscribe({
            next: (data) => (this.reviewerDensityReport = data),
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

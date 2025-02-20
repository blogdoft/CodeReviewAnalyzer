import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { DatePickerModule } from 'primeng/datepicker';
import { DividerModule } from 'primeng/divider';
import { FloatLabelModule } from 'primeng/floatlabel';
import { ToolbarModule } from 'primeng/toolbar';

import { PullRequestGraphComponent } from '../../components/pull-request-graph/pull-request-graph.component';
import { CommentData } from '../../services/report/models/comment-data.model';
import { PullRequestTimeReport } from '../../services/report/models/pull-request-report.model';
import { PullRequestReportService } from '../../services/report/pullrequest.report.service';
import { ReviewerDensityGraphComponent } from './../../components/reviewer-density-graph/reviewer-density-graph.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    PullRequestGraphComponent,
    ReviewerDensityGraphComponent,
    ToolbarModule,
    DatePickerModule,
    FloatLabelModule,
    FormsModule,
    DividerModule,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  public _rangeDates: Date[] | null = null;

  set rangeDates(value: Date[] | null) {
    this._rangeDates = value;
    if (value && value[0] != null && value[1] != null)
      this.searchPullRequests(value);
  }

  get rangeDates(): Date[] | null {
    return this._rangeDates;
  }
  public lineChartLegend = true;
  public pullRequestReport: PullRequestTimeReport = {
    meanTimeOpenToApproval: [],
    meanTimeToMerge: [],
    meanTimeToStartReview: [],
    pullRequestCount: [],
    pullRequestSize: [],
    pullRequestWithoutCommentCount: [],
  };
  public reviewerDensityReport: CommentData[] = [];

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
      error: (err) => this.showError(err),
    });

    this.pullRequestReportService
      .getReviewerDensity(event![0], event![1])
      .subscribe({
        next: (data) => (this.reviewerDensityReport = data),
        error: (err) => this.showError(err),
      });
  }

  private showError(err: any) {
    this.message.add({
      detail: err.message,
      summary: 'Summary',
      severity: 'error',
      life: 50000,
    });
  }
}

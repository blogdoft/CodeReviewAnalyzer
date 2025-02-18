import { Component, OnInit } from '@angular/core';
import { PullRequestReportService } from '../../services/report/pullrequest.report.service';
import { PullRequestTimeReport } from '../../services/report/models/pull-request-report.model';
import { PullRequestGraphComponent } from '../../components/pull-request-graph/pull-request-graph.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [PullRequestGraphComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  public rangeDates: Date[] | null = null;
  public lineChartLegend = true;
  public report: PullRequestTimeReport = {
    meanTimeOpenToApproval: [],
    meanTimeToMerge: [],
    meanTimeToStartReview: [],
    pullRequestCount: [],
    pullRequestSize: [],
    pullRequestWithoutCommentCount: [],
  };

  constructor(private pullRequestReportService: PullRequestReportService) {}

  ngOnInit(): void {
    const today = new Date();
    const threeMonthsAgo = new Date();
    threeMonthsAgo.setMonth(today.getMonth() - 4);
    this.rangeDates = [threeMonthsAgo, today];
    this.searchPullRequests(this.rangeDates);
  }

  public searchPullRequests(event: Date[] | null) {
    this.pullRequestReportService.getReports(event![0], event![1]).subscribe({
      next: (data) => {
        this.report = data;
        console.log(data);
      },
      error: (err) => console.error(err),
    });
  }
}

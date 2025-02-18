import { PullRequestTimeReport } from './../../services/report/models/pull-request-report.model';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ChartConfiguration, ChartOptions, ChartType } from 'chart.js';
import { TimeIndex } from '../../services/report/models/pull-request-report.model';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { FormsModule } from '@angular/forms';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: 'app-pull-request-graph',
  imports: [
    BaseChartDirective,
    CardModule,
    ButtonModule,
    DatePickerModule,
    FormsModule,
  ],
  templateUrl: './pull-request-graph.component.html',
  styleUrl: './pull-request-graph.component.scss',
})
export class PullRequestGraphComponent {
  @Input()
  set pullRequestTimeReport(value: PullRequestTimeReport) {
    this._pullRequestTimeReport = value;
    this.renderReport(value);
  }

  get pullRequestTimeReport(): PullRequestTimeReport {
    return this._pullRequestTimeReport;
  }

  @Input()
  rangeDates: Date[] | null = null;

  @Output()
  onSearchClick = new EventEmitter<Date[] | null>();

  lineChartData!: ChartConfiguration<'line'>['data'];

  lineChartOptions: ChartOptions<'line'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'bottom',
      },
    },
  };
  public lineChartLegend = true;

  private _pullRequestTimeReport: PullRequestTimeReport = {
    meanTimeOpenToApproval: [],
    meanTimeToMerge: [],
    meanTimeToStartReview: [],
    pullRequestCount: [],
    pullRequestSize: [],
    pullRequestWithoutCommentCount: [],
  };

  constructor() {}

  onSearch() {
    this.onSearchClick.emit(this.rangeDates);
  }

  private renderReport(report: PullRequestTimeReport): void {
    const labels = this.extractLabels(report);
    const meanTimeToApproval = this.createTimeDataSeries(
      report.meanTimeOpenToApproval ?? [],
      labels
    );
    const meanTimeStartReview = this.createTimeDataSeries(
      report.meanTimeToStartReview ?? [],
      labels
    );
    const meanTimeToMerge = this.createTimeDataSeries(
      report.meanTimeToMerge ?? [],
      labels
    );
    const prCounters = this.createCounterDataSeries(
      report.pullRequestCount ?? [],
      labels
    );
    const prWithoutComment = this.createCounterDataSeries(
      report.pullRequestWithoutCommentCount ?? [],
      labels
    );

    console.log(prWithoutComment);

    this.lineChartData = {
      labels: labels,
      datasets: [
        {
          data: meanTimeToApproval,
          label: 'Mean Time To Approval',
          borderColor: '#264653',
          backgroundColor: 'transparent',
          fill: false,
          tension: 0.0,
          borderWidth: 1.5,
        },
        {
          data: meanTimeStartReview,
          label: 'Mean Time To Start Review',
          borderColor: '#2A9D8F',
          backgroundColor: 'transparent',
          fill: false,
          tension: 0.0,
          borderWidth: 1.5,
        },
        {
          data: meanTimeToMerge,
          label: 'Mean Time To Merge',
          borderColor: '#E9C46A',
          backgroundColor: 'transparent',
          fill: false,
          tension: 0.0,
          borderWidth: 1.5,
        },
        {
          data: prCounters,
          label: 'Pull request count',
          borderColor: '#F4A261',
          backgroundColor: 'transparent',
          fill: false,
          tension: 0.0,
          borderWidth: 1.5,
        },
        {
          data: prWithoutComment,
          label: 'Non Commented pull requests count',
          borderColor: '#E76F51',
          backgroundColor: 'transparent',
          fill: false,
          tension: 0.0,
          borderWidth: 1.5,
        },
      ],
    };
  }

  createTimeDataSeries(timeIndices: TimeIndex[], labels: string[]): number[] {
    const monthNames = [
      'Jan',
      'Fev',
      'Mar',
      'Abr',
      'Mai',
      'Jun',
      'Jul',
      'Ago',
      'Set',
      'Out',
      'Nov',
      'Dez',
    ];
    const dataMap: { [label: string]: number } = {};
    timeIndices.forEach((item) => {
      const date = new Date(item.referenceDate);
      const label = `${monthNames[date.getMonth()]}/${date
        .getFullYear()
        .toString()
        .slice(-2)}`;
      dataMap[label] = item.periodInMinutes / 60;
    });
    return labels.map((label) => dataMap[label] ?? 0);
  }

  createCounterDataSeries(
    timeIndices: TimeIndex[],
    labels: string[]
  ): number[] {
    const monthNames = [
      'Jan',
      'Fev',
      'Mar',
      'Abr',
      'Mai',
      'Jun',
      'Jul',
      'Ago',
      'Set',
      'Out',
      'Nov',
      'Dez',
    ];
    const dataMap: { [label: string]: number } = {};
    timeIndices.forEach((item) => {
      const date = new Date(item.referenceDate);
      const label = `${monthNames[date.getMonth()]}/${date
        .getFullYear()
        .toString()
        .slice(-2)}`;
      dataMap[label] = item.periodInMinutes;
    });
    return labels.map((label) => dataMap[label] ?? 0);
  }

  private extractLabels(report: PullRequestTimeReport) {
    // Extrai todas as datas dos arrays que contenham o campo referenceDate
    const dates: string[] = [
      ...(report.meanTimeOpenToApproval ?? []).map(
        (item) => item.referenceDate
      ),
      ...(report.meanTimeToStartReview ?? []).map((item) => item.referenceDate),
      ...(report.meanTimeToMerge ?? []).map((item) => item.referenceDate),
    ];

    // Remove duplicatas e ordena as datas
    const uniqueDates = Array.from(new Set(dates)).sort();

    // Define os nomes dos meses em português
    const monthNames = [
      'Jan',
      'Fev',
      'Mar',
      'Abr',
      'Mai',
      'Jun',
      'Jul',
      'Ago',
      'Set',
      'Out',
      'Nov',
      'Dez',
    ];

    // Mapeia cada data para o formato desejado: "MMM/YY"
    const labels = uniqueDates.map((dateStr) => {
      const date = new Date(dateStr);
      const month = monthNames[date.getMonth()];
      const year = date.getFullYear().toString().slice(-2);
      return `${month}/${year}`;
    });
    console.log(labels);
    return labels;
  }

  searchPullRequests() {}
}

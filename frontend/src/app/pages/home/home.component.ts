import { Component, OnInit, ViewChild } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartOptions, ChartType } from 'chart.js';
import { PullRequestReportService } from '../../services/report/pullrequest.report.service';
import {
  PullRequestTimeReport,
  TimeIndex,
} from '../../services/report/models/pull-request-report.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [BaseChartDirective],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  public lineChartData!: ChartConfiguration<'line'>['data'];

  constructor(private pullRequestReportService: PullRequestReportService) {}

  ngOnInit(): void {
    this.pullRequestReportService
      .getReports('2024-10-01', '2025-02-28')
      .subscribe({
        next: (data) => {
          const labels = this.extractLabels(data);
          const meanTimeToApproval = this.createTimeDataSeries(
            data.meanTimeOpenToApproval ?? [],
            labels
          );
          const meanTimeStartReview = this.createTimeDataSeries(
            data.meanTimeToStartReview ?? [],
            labels
          );
          const meanTimeToMerge = this.createTimeDataSeries(
            data.meanTimeToMerge ?? [],
            labels
          );
          const prCounters = this.createCounterDataSeries(
            data.pullRequestCount ?? [],
            labels
          );
          const prWithoutComment = this.createCounterDataSeries(
            data.pullRequestWithoutCommentCount ?? [],
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

          console.log(data);
        },
        error: (err) => console.error(err),
      });
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

  createCounterDataSeries(timeIndices: TimeIndex[], labels: string[]): number[] {
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

  public lineChartOptions: ChartOptions<'line'> = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'bottom',
      }
    }
  };

  public lineChartLegend = true;

  public barChartLegend = true;
  public barChartPlugins = [];

  public barChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [
      'Jan/24',
      'Fev/24',
      'Mar/24',
      'Abr/24',
      'Mai/24',
      'Jun/24',
      'Jul/24',
      'Ago/24',
      'Set/24',
      'Out/24',
      'Nov/24',
      'Dez/24',
      'Jan/25',
    ],
    datasets: [
      {
        data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0],
        label: 'Aduan Nadalon Vieira',
      },
      {
        data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1],
        label: 'Lucas farias',
      },
      {
        data: [0, 2, 1, 2, 0, 0, 0, 0, 0, 1, 1, 0, 1],
        label: 'Danielle Baer',
      },
    ],
  };

  public barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: false,
  };
}

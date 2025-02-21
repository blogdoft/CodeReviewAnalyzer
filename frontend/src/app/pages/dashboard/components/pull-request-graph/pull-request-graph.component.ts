import { Component, Input } from '@angular/core';
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { AccordionModule } from 'primeng/accordion';
import { CardModule } from 'primeng/card';
import { ChartModule } from 'primeng/chart';
import { DividerModule } from 'primeng/divider';
import { PanelModule } from 'primeng/panel';
import { TabsModule } from 'primeng/tabs';

import { PullRequestTimeReport, TimeIndex } from '../../../service/report/models/pull-request-report.model';
import { MathjaxDirective } from '../../../uikit/directives/mathjax.directive';
import { monthNames } from '../constants/month-names';

@Component({
    selector: 'app-pull-request-graph',
    imports: [CardModule, ChartModule, AccordionModule, TabsModule, PanelModule, DividerModule, MathjaxDirective],
    templateUrl: './pull-request-graph.component.html',
    styleUrl: './pull-request-graph.component.scss'
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

    lineChartData!: ChartConfiguration<'line'>['data'];
    lineChartOptions: ChartOptions<'line'> = {
        responsive: true,
        plugins: {
            legend: {
                display: true,
                position: 'bottom'
            }
        }
    };
    public lineChartLegend = true;

    private _pullRequestTimeReport: PullRequestTimeReport = {
        meanTimeOpenToApproval: [],
        meanTimeToMerge: [],
        meanTimeToStartReview: [],
        pullRequestCount: [],
        pullRequestSize: [],
        pullRequestWithoutCommentCount: []
    };

    constructor() {}

    private renderReport(report: PullRequestTimeReport): void {
        const labels = this.extractLabels(report);
        const meanTimeToApproval = this.createTimeDataSeries(report.meanTimeOpenToApproval ?? [], labels);
        const meanTimeStartReview = this.createTimeDataSeries(report.meanTimeToStartReview ?? [], labels);
        const meanTimeToMerge = this.createTimeDataSeries(report.meanTimeToMerge ?? [], labels);
        const prCounters = this.createCounterDataSeries(report.pullRequestCount ?? [], labels);
        const prWithoutComment = this.createCounterDataSeries(report.pullRequestWithoutCommentCount ?? [], labels);

        this.lineChartData = {
            labels: labels,
            datasets: [
                {
                    data: meanTimeStartReview,
                    label: 'Mean Time to Review',
                    borderColor: '#2A9D8F',
                    backgroundColor: 'transparent',
                    fill: false,
                    tension: 0.0,
                    borderWidth: 1.5
                },
                {
                    data: meanTimeToApproval,
                    label: 'Mean Time to Approve',
                    borderColor: '#264653',
                    backgroundColor: 'transparent',
                    fill: false,
                    tension: 0.0,
                    borderWidth: 1.5
                },
                {
                    data: meanTimeToMerge,
                    label: 'Mean Time To Merge',
                    borderColor: '#E9C46A',
                    backgroundColor: 'transparent',
                    fill: false,
                    tension: 0.0,
                    borderWidth: 1.5
                },
                {
                    data: prCounters,
                    label: 'Pull request count',
                    borderColor: '#F4A261',
                    backgroundColor: 'transparent',
                    fill: false,
                    tension: 0.0,
                    borderWidth: 1.5
                },
                {
                    data: prWithoutComment,
                    label: 'Non Commented pull requests count',
                    borderColor: '#E76F51',
                    backgroundColor: 'transparent',
                    fill: false,
                    tension: 0.0,
                    borderWidth: 1.5
                }
            ]
        };
    }

    private createTimeDataSeries(timeIndices: TimeIndex[], labels: string[]): number[] {
        const dataMap: { [label: string]: number } = {};
        timeIndices.forEach((item) => {
            const date = new Date(item.referenceDate);
            const label = `${monthNames[date.getMonth()]}/${date.getFullYear().toString().slice(-2)}`;
            dataMap[label] = item.periodInMinutes / 60;
        });
        return labels.map((label) => dataMap[label] ?? 0);
    }

    private createCounterDataSeries(timeIndices: TimeIndex[], labels: string[]): number[] {
        const dataMap: { [label: string]: number } = {};
        timeIndices.forEach((item) => {
            const date = new Date(item.referenceDate);
            const label = `${monthNames[date.getMonth()]}/${date.getFullYear().toString().slice(-2)}`;
            dataMap[label] = item.periodInMinutes;
        });
        return labels.map((label) => dataMap[label] ?? 0);
    }

    private extractLabels(report: PullRequestTimeReport) {
        // Extrai todas as datas dos arrays que contenham o campo referenceDate
        const dates: string[] = [
            ...(report.meanTimeOpenToApproval ?? []).map((item) => item.referenceDate),
            ...(report.meanTimeToStartReview ?? []).map((item) => item.referenceDate),
            ...(report.meanTimeToMerge ?? []).map((item) => item.referenceDate)
        ];

        // Remove duplicatas e ordena as datas
        const uniqueDates = Array.from(new Set(dates)).sort();

        // Mapeia cada data para o formato desejado: "MMM/YY"
        const labels = uniqueDates.map((dateStr) => {
            const date = new Date(dateStr);
            const month = monthNames[date.getMonth()];
            const year = date.getFullYear().toString().slice(-2);
            return `${month}/${year}`;
        });

        return labels;
    }
}

import { Component, Input } from '@angular/core';
import { ChartConfiguration } from 'chart.js';
import { CardModule } from 'primeng/card';
import { ChartModule } from 'primeng/chart';

import { CommentData } from '../../../service/report/models/comment-data.model';
import { monthNames } from '../constants/month-names';

@Component({
    selector: 'app-reviewer-density-graph',
    imports: [CardModule, ChartModule],
    templateUrl: './reviewer-density-graph.component.html',
    styleUrl: './reviewer-density-graph.component.scss'
})
export class ReviewerDensityGraphComponent {
    @Input()
    set reviewerDensity(value: CommentData[]) {
        this._reviewerDensity = value;
        this.prepareChartData(value);
    }

    get reviewerDensity(): CommentData[] {
        return this._reviewerDensity;
    }
    public _reviewerDensity: CommentData[] = [];
    public barChartLegend = true;
    public barChartPlugins = [];
    public barChartData!: ChartConfiguration<'bar'>['data'];
    public barChartOptions: ChartConfiguration<'bar'>['options'] = {
        responsive: true,
        plugins: {
            legend: {
                display: true,
                position: 'bottom'
            }
        }
    };

    prepareChartData(apiResponse: CommentData[]): void {
        // 1. Build axis X
        const developers = Array.from(
            new Set(apiResponse.map((item) => item.userName))
        ).sort();

        // 2. Build Month list (referenceDate)
        const months = Array.from(
            new Set(apiResponse.map((item) => item.referenceDate))
        ).sort();

        // User Friendly dates: "Jan/2025"
        const formattedMonths = months.map((dateStr) => {
            const date = new Date(`${dateStr}T00:00:00`);
            const month = monthNames[date.getMonth()];
            const year = date.getFullYear();
            return `${month}/${year}`;
        });

        // 3. Create datasets for each month.
        // Each dataset will have, by developer, the months' commentCount.
        const colors = [
            'rgba(54, 162, 235, 0.7)', // Blue
            'rgba(255, 99, 132, 0.7)', // Red
            'rgba(255, 206, 86, 0.7)', // Yellow
            'rgba(75, 192, 192, 0.7)', // Aqua/Teal
            'rgba(153, 102, 255, 0.7)', // Purple
            'rgba(255, 159, 64, 0.7)', // Orange
            'rgba(0, 128, 0, 0.7)', // Dark Green
            'rgba(128, 0, 128, 0.7)', // Dark Purple
            'rgba(0, 0, 128, 0.7)', // Navy Blue
            'rgba(220, 20, 60, 0.7)', // Crimson
            'rgba(70, 130, 180, 0.7)', // Steel Blue
            'rgba(128, 128, 0, 0.7)' // Olive
        ];

        const datasets = months.map((month, idx) => {
            // Para cada desenvolvedor, busque o registro correspondente a este mês
            const data = developers.map((dev) => {
                const record = apiResponse.find(
                    (item) =>
                        item.userName === dev && item.referenceDate === month
                );
                return record ? record.commentCount : 0;
            });
            return {
                label: formattedMonths[idx],
                data: data,
                backgroundColor: colors[idx % colors.length],
                borderWidth: 1
            };
        });

        // 4. Configure os dados do gráfico: as labels serão os desenvolvedores e os datasets, cada mês.
        this.barChartData = {
            labels: developers,
            datasets: datasets
        };
    }
}

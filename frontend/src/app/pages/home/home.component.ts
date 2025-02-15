import { Component } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartOptions, ChartType } from 'chart.js';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [BaseChartDirective],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  public lineChartData: ChartConfiguration<'line'>['data'] = {
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
        label: 'Review Time: Open To approval',
        tension: 0,
        borderColor: 'red',
        borderWidth: 1,
        data: [71, 21, 48, 48, 109, 91, 34, 45, 49, 47, 32, 27, 24],
      },
      {
        label: 'Review Time: Start review mean time',
        tension: 0,
        borderColor: 'blue',
        borderWidth: 1,
        data: [12, 2, 5, 3, 2, 17, 7, 2, 12, 5, 9, 6, 3],
      },
      {
        label: 'Review Time: Mean revision time',
        tension: 0,
        borderColor: 'green',
        borderWidth: 1,
        data: [44, 18, 42, 40, 98, 68, 22, 45, 18, 39, 21, 19, 21],
      },
      {
        label: 'Pull Size: Mean File Count',
        tension: 0,
        borderColor: 'purple',
        borderWidth: 1,
        data: [4, 7, 6, 8, 8, 16, 7, 1, 14, 8, 10, 12, 8],
      },
    ],
  };

  public lineChartOptions: ChartOptions<'line'> = {
    responsive: false,
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

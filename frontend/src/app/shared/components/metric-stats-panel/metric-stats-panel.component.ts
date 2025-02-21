import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

import { MetricStatsOptions } from './metric-stats-options';
import { MetricStatus } from './metric-status.enum';

@Component({
    selector: 'app-metric-stats-panel',
    imports: [CommonModule],
    templateUrl: './metric-stats-panel.component.html',
    styleUrl: './metric-stats-panel.component.scss'
})
export class MetricStatsPanelComponent {
    @Input()
    set options(value: MetricStatsOptions) {
        this._options = {
            ...value,
            icon: `${this.baseIconClass} ${value.icon ?? this.icon[value.status]} ${this.iconColor[value.status]}`
        };
    }

    get options(): MetricStatsOptions {
        return this._options;
    }

    private iconColor = {
        [MetricStatus.ok]: 'text-green-500',
        [MetricStatus.warning]: 'text-yellow-500',
        [MetricStatus.error]: 'text-red-500'
    };

    private icon = {
        [MetricStatus.ok]: 'pi-check-circle',
        [MetricStatus.warning]: 'pi-exclamation-circle',
        [MetricStatus.error]: 'pi-times-circle'
    };

    private _options: MetricStatsOptions = {
        title: 'Untitled',
        current: { value: 'N/A', referenceDate: new Date() },
        previous: { value: 'N/A', referenceDate: new Date() },
        icon: 'pi-tag',
        status: MetricStatus.error
    };
    private baseIconClass = 'pi text-blue-500 !text-xl';

    constructor() {}
}

import { CommonModule } from '@angular/common';
import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { MultiSelectModule } from 'primeng/multiselect';
import { SelectModule } from 'primeng/select';
import { SliderModule } from 'primeng/slider';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';

import { Outlier } from '../../../service/report/models/outlier';
import { ButtonModule } from 'primeng/button';
import { PullRequestStatsDialogComponent } from '../pull-request-stats-dialog/pull-request-stats-dialog.component';

@Component({
    selector: 'app-outliers-table',
    standalone: true,
    imports: [
        ButtonModule,
        TableModule,
        TagModule,
        FormsModule,
        CommonModule,
        InputIconModule,
        IconFieldModule,
        MultiSelectModule,
        SelectModule,
        SliderModule,
        CardModule,
        PullRequestStatsDialogComponent
    ],
    templateUrl: './outliers-table.component.html',
    styleUrl: './outliers-table.component.scss'
})
export class OutliersTableComponent {
    loading: boolean = true;
    showPrStats: boolean = false;

    fieldNames: string[] = [];

    @ViewChild('filter') filter!: ElementRef;
    @ViewChild('prstats') prStatsDialog!: PullRequestStatsDialogComponent;

    @Input()
    set outliers(value: Outlier[]) {
        this._outliers = value;
        this.fieldNames = [];
        if (value) {
            this.fieldNames = [
                ...new Set(this.outliers.map((outlier) => outlier.outlierField))
            ];
        }
        this.loading = false;
    }

    get outliers(): Outlier[] {
        return this._outliers;
    }

    public selectedOutliers!: string[];

    public showPrStatsClick() {
        console.log(this.showPrStats);
        this.showPrStats = true;
    }

    public showPrStatsOf(event: string): void {
        this.prStatsDialog.showStatsFor(event);
    }

    private _outliers!: Outlier[];
}

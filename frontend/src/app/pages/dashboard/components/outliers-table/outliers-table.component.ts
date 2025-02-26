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

import { CustomerService } from '../../../service/customer.service';
import { Outlier } from '../../../service/report/models/outlier';

@Component({
    selector: 'app-outliers-table',
    standalone: true,
    imports: [
        TableModule,
        TagModule,
        FormsModule,
        CommonModule,
        InputIconModule,
        IconFieldModule,
        MultiSelectModule,
        SelectModule,
        SliderModule,
        CardModule
    ],
    templateUrl: './outliers-table.component.html',
    styleUrl: './outliers-table.component.scss',
    providers: [CustomerService]
})
export class OutliersTableComponent {
    loading: boolean = true;

    fieldNames: string[] = [];

    @ViewChild('filter') filter!: ElementRef;

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

    private _outliers!: Outlier[];
}

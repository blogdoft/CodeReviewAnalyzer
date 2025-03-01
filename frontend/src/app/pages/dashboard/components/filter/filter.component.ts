import {
    LazyQuery,
    LookupOptions,
    PageResponse
} from './../../../../shared/components/paginated-lookup/paginated-lookup.component';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
    AutoCompleteDropdownClickEvent,
    AutoCompleteLazyLoadEvent,
    AutoCompleteModule,
    AutoCompleteSelectEvent
} from 'primeng/autocomplete';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DatePickerModule } from 'primeng/datepicker';
import { IftaLabelModule } from 'primeng/iftalabel';
import { ToolbarModule } from 'primeng/toolbar';

import { PaginatedLookupComponent } from '../../../../shared/components/paginated-lookup/paginated-lookup.component';
import { PageFilter } from './../../../service/report/models/page-filter';
import { TeamsRepositoryService } from './../../../service/report/teams-repository.service';
import { DashboardFilter, DateRange } from './date-range';

@Component({
    selector: 'app-filter',
    imports: [
        CommonModule,
        FormsModule,
        ToolbarModule,
        IftaLabelModule,
        DatePickerModule,
        AutoCompleteModule,
        CardModule,
        ButtonModule,
        PaginatedLookupComponent
    ],
    templateUrl: './filter.component.html',
    styleUrl: './filter.component.scss'
})
export class FilterComponent {
    @Output()
    public OnSearch: EventEmitter<DashboardFilter> =
        new EventEmitter<DashboardFilter>();

    @Input()
    set rangeDate(value: DateRange | null) {
        this._rangeDate = value;
        this.lookupRange = [];

        if (value) {
            this.lookupRange = [value.from, value.to];
            return;
        }
    }
    get rangeDate(): DateRange | null {
        return this._rangeDate;
    }

    public autoFilteredValue!: any;
    protected lookupRange: Date[] = [];
    protected userTeamLookupOptions: LookupOptions = {
        placeholder: 'Start typing the team name',
        dataKey: 'externalId',
        optionLabel: 'name',
        inputId: 'userTeamLookup'
    };
    protected repoTeamLookupOptions: LookupOptions = {
        ...this.userTeamLookupOptions,
        inputId: 'repoTeamLookup'
    };
    protected isLoading: boolean = false;
    protected pageResponse: PageResponse = {
        currentPage: 0,
        totalItems: 0,
        totalPages: 0
    };

    protected repoTeamId: string | null = null;
    protected userTeamId: string | null = null;

    private _rangeDate!: DateRange | null;
    private pageSize: number = 50;

    constructor(private teams: TeamsRepositoryService) {}

    onSearchClick() {
        this.OnSearch.emit({
            teamRepositoryId: this.repoTeamId,
            teamUserId: this.userTeamId,
            dateRange: {
                from: this.lookupRange[0]!,
                to: this.lookupRange[1]!
            }
        });
    }

    loadTeamData(event: LazyQuery): void {
        this.isLoading = true;

        this.teams
            .getTeamsByDescription(`${event.params}*`, {
                page: event.page,
                size: this.pageSize,
                order: event.order ?? 'name'
            })
            .subscribe({
                next: (response) => {
                    this.pageResponse = {
                        totalItems: response.totalItems,
                        currentPage: response.currentPage,
                        totalPages: response.totalPages
                    };

                    if (event.lazyCall) {
                        this.autoFilteredValue = [
                            ...this.autoFilteredValue,
                            ...response.data
                        ];
                    } else {
                        this.autoFilteredValue = response.data;
                    }
                    this.isLoading = false;
                },
                error: (err) => {
                    console.log(JSON.stringify(err));
                    this.isLoading = false;
                }
            });
    }
}

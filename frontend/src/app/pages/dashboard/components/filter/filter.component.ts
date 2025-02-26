import { TeamsRepositoryService } from './../../../service/report/teams-repository.service';
import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
    AutoCompleteDropdownClickEvent,
    AutoCompleteLazyLoadEvent,
    AutoCompleteModule,
    AutoCompleteSelectEvent,
    AutoCompleteUnselectEvent
} from 'primeng/autocomplete';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DatePickerModule } from 'primeng/datepicker';
import { IftaLabelModule } from 'primeng/iftalabel';
import { ToolbarModule } from 'primeng/toolbar';

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
        ButtonModule
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

    public selectedAutoValue!: any;
    public autoFilteredValue!: any;
    protected lookupRange: Date[] = [];
    private _rangeDate!: DateRange | null;
    private repoTeamId: string | null = null;
    private userTeamId: string | null = null;

    /**
     *
     */
    constructor(private teams: TeamsRepositoryService) {}

    public filterTeamRepository($event: AutoCompleteDropdownClickEvent) {
        this.teams.getTeamsByDescription($event.query).subscribe({
            next: (data) => {
                this.autoFilteredValue = data;
            },
            error: (err) => console.log(JSON.stringify(err))
        });
    }
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

    onClearRepoTeam($event: Event | undefined) {
        this.repoTeamId = null;
    }
    onSelectRepoTeam($event: AutoCompleteSelectEvent) {
        this.repoTeamId = $event.value['externalId'];
    }

    onSelectUserTeam($event: AutoCompleteSelectEvent) {
        this.userTeamId = $event.value['externalId'];
    }

    onClearUserTeam($event: Event | undefined) {
        this.userTeamId = null;
    }
}

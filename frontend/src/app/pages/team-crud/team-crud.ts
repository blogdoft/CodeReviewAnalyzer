import { CommonModule } from '@angular/common';
import { Component, OnInit, signal, ViewChild } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AccordionModule } from 'primeng/accordion';
import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { DrawerModule } from 'primeng/drawer';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { Paginator, PaginatorModule, PaginatorState } from 'primeng/paginator';
import { Table, TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';

import { MessageHandlerService } from '../../shared/services/message-handler.service';
import { Product } from '../service/product.service';
import { Team } from '../service/teamsRepository/Models/team';
import { TeamsRepositoryService } from '../service/teamsRepository/teams-repository.service';
import { TeamDetailsComponent } from './team-details/team-details.component';

interface Column {
    field: string;
    header: string;
    customExportHeader?: string;
}

interface ExportColumn {
    title: string;
    dataKey: string;
}

@Component({
    selector: 'app-crud',
    standalone: true,
    imports: [
        AccordionModule,
        ButtonModule,
        CommonModule,
        ConfirmDialogModule,
        DialogModule,
        DrawerModule,
        IconFieldModule,
        InputIconModule,
        InputTextModule,
        PaginatorModule,
        RouterModule,
        TableModule,
        TagModule,
        ToastModule,
        ToolbarModule,
        TeamDetailsComponent
    ],
    templateUrl: './team-crud.html',
    providers: [
        ConfirmationService,
        MessageHandlerService,
        TeamsRepositoryService
    ]
})
export class TeamCrud implements OnInit {
    private readonly InitialPage: number = 1;
    activeState: boolean[] = [false, false];
    teamDialog: boolean = false;

    teams = signal<Team[]>([]);

    team!: Team;

    selectedProducts!: Product[] | null;
    selectedTeams!: Team[] | null;

    submitted: boolean = false;

    @ViewChild('dt') dt!: Table;
    @ViewChild('paginator') paginator!: Paginator;

    @ViewChild(TeamDetailsComponent)
    teamDetailsComponent!: TeamDetailsComponent;

    exportColumns!: ExportColumn[];

    cols!: Column[];

    protected tableFilter: TableFilter = {
        pageSize: 5,
        currentPage: this.InitialPage,
        totalItems: 0
    };
    protected first: number = 0;
    protected pageOptions: number[] = [5, 10, 20, 30];

    constructor(
        private teamRepository: TeamsRepositoryService,
        private confirmationService: ConfirmationService,
        private messageHandler: MessageHandlerService,
        private route: ActivatedRoute,
        private router: Router
    ) {}

    ngOnInit() {
        this.route.queryParamMap.subscribe((params) => {
            let pageSize = parseInt(params.get('pageSize') || '10');
            pageSize =
                this.pageOptions.indexOf(pageSize) === -1 ? 10 : pageSize;
            const currentPage = parseInt(
                params.get('currentPage') || this.InitialPage.toString()
            );
            this.first = (currentPage - 1) * pageSize;
            const teamName = params.get('teamName');
            this.tableFilter = {
                pageSize,
                currentPage,
                teamName
            };

            this.loadTeams();
        });
    }

    /** Filtering */

    protected onGlobalFilter(table: Table, event: Event) {
        this.tableFilter.currentPage = this.InitialPage;
        this.tableFilter.teamName = (event.target as HTMLInputElement).value;
        this.onFilterChange();
    }

    protected pageChange(event: PaginatorState) {
        this.tableFilter.currentPage = (event.page ?? 0) + this.InitialPage;
        this.tableFilter.pageSize = event.rows ?? 10;

        this.onFilterChange();
    }

    private onFilterChange(): void {
        const { totalItems, ...tableFilter } = this.tableFilter;
        this.router.navigate([], {
            relativeTo: this.route,
            queryParams: {
                ...tableFilter
            },
            onSameUrlNavigation: 'ignore'
        });
    }

    /** CRUD Events */

    protected btnNewClick() {
        this.team = {};
        this.teamDetailsComponent.showTeam(this.team);
    }

    protected btnEditClick(team: Team) {
        this.team = { ...team };
        this.teamDetailsComponent.showTeam(this.team);
    }

    protected btnDeleteClick(team: Team) {
        console.log('asdf');
        this.confirmationService.confirm({
            message: 'Are you sure you want to delete "' + team.name + '"?',
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.deleteTeam(team);
            }
        });
    }

    protected btnGroupDeleteClick() {
        if (!this.selectedTeams) return;
        this.confirmationService.confirm({
            message: 'Are you sure you want to delete the selected teams?',
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                const teams = this.selectedTeams?.map((x) => x) ?? [];
                this.selectedTeams = null;
                teams.forEach((team) => {
                    this.deleteTeam(team);
                });
                this.onFilterChange();
                this.selectedTeams = null;
            }
        });
    }

    protected onSaveTeam(event: Team) {
        this.team = event;
        this.saveTeam();
    }

    private deleteTeam(team: Team | null) {
        if (!team?.externalId) {
            this.messageHandler.showError(
                'This team has no external id defined!'
            );
            return;
        }

        this.teamRepository.deleteTeam(team.externalId).subscribe({
            next: (response) => {
                this.messageHandler.addSuccess(
                    `Team "${team.name}" was successfully deleted.`
                );
                this.loadTeams();
            },
            error: (err) => {
                this.messageHandler.handleHttpError(err);
                this.loadTeams();
            }
        });
    }

    private saveTeam() {
        this.submitted = true;
        if (this.team.name?.trim()) {
            if (this.team.externalId) {
                this.updateTeam(this.team);
            } else {
                this.createTeam(this.team);
            }

            this.teamDialog = false;
            this.team = {};
        }
    }

    private createTeam(team: Team): void {
        this.team.externalId = uuidv4();
        this.teamRepository.createTeam(team).subscribe({
            next: (response) => {
                this.messageHandler.addSuccess(
                    `Team "${team.name}" successfully created.`
                );

                this.loadTeams();
            },
            error: (err) => {
                this.messageHandler.handleHttpError(err);
                this.loadTeams();
            }
        });
    }

    private updateTeam(team: Team) {
        this.teamRepository.updateTeam(team).subscribe({
            next: (response) => {
                this.messageHandler.addSuccess(
                    `The Team "${response.name}" was updated successfully.`
                );
                this.loadTeams();
            },
            error: (err) => {
                this.messageHandler.handleHttpError(
                    err,
                    `Error while saving "${team.name}" updates`
                );
                this.loadTeams();
            }
        });
    }

    protected loadTeams() {
        this.teamRepository
            .getTeamsByDescription(`${this.tableFilter.teamName ?? ''}*`, {
                page: this.tableFilter.currentPage - 1,
                size: this.tableFilter.pageSize,
                order: 'name'
            })
            .subscribe({
                next: (response) => {
                    this.teams.set(response.data);
                    this.tableFilter.totalItems = response.totalItems;
                },
                error: (err) =>
                    this.messageHandler.handleHttpError(
                        err,
                        'Error loading teams data.'
                    )
            });

        this.cols = [
            {
                field: 'externalId',
                header: 'External Id',
                customExportHeader: 'External Id'
            },
            { field: 'name', header: 'Name' },
            { field: 'description', header: 'Description' },
            { field: 'active', header: 'Active' }
        ];

        this.exportColumns = this.cols.map((col) => ({
            title: col.header,
            dataKey: col.field
        }));
    }

    protected getStatus(status: boolean): string {
        if (status) return 'Active';
        else return 'Deactivated';
    }

    protected getSeverity(status: boolean) {
        if (status) return 'success';
        else return 'danger';
    }

    protected exportCSV() {
        this.dt.exportCSV();
    }
}

function uuidv4() {
    return '10000000-1000-4000-8000-100000000000'.replace(/[018]/g, (c) =>
        (
            +c ^
            (crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (+c / 4)))
        ).toString(16)
    );
}

interface TableFilter {
    currentPage: number;
    pageSize: number;
    totalItems?: number | null;
    teamName?: string | null;
}

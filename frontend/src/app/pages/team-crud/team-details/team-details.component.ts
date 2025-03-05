import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
    Accordion,
    AccordionModule,
    AccordionTabOpenEvent
} from 'primeng/accordion';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { DrawerModule } from 'primeng/drawer';
import { IftaLabelModule } from 'primeng/iftalabel';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';
import { TextareaModule } from 'primeng/textarea';
import { ToastModule } from 'primeng/toast';
import { ToggleSwitchModule } from 'primeng/toggleswitch';

import {
    LookupOptions,
    PageResponse
} from '../../../shared/components/paginated-lookup/paginated-lookup.component';
import { MessageHandlerService } from '../../../shared/services/message-handler.service';
import { Team } from '../../service/teamsRepository/Models/team';
import { TeamsRepositoryService } from '../../service/teamsRepository/teams-repository.service';
import { TeamUser } from './../../service/teamsRepository/Models/TeamUser';
import { TeamUserDialogComponent } from './team-user-dialog/team-user-dialog.component';

@Component({
    selector: 'app-team-details',
    imports: [
        AccordionModule,
        ButtonModule,
        CommonModule,
        ConfirmDialog,
        DialogModule,
        DrawerModule,
        FormsModule,
        InputTextModule,
        IftaLabelModule,
        SelectModule,
        TableModule,
        TextareaModule,
        ToastModule,
        ToggleSwitchModule,
        TeamUserDialogComponent
    ],
    templateUrl: './team-details.component.html',
    styleUrl: './team-details.component.scss',
    providers: [MessageService, MessageHandlerService, ConfirmationService]
})
export class TeamDetailsComponent {
    @Output()
    public onSaveTeam: EventEmitter<Team> = new EventEmitter<Team>();

    @ViewChild(TeamUserDialogComponent)
    protected teamUserDialog!: TeamUserDialogComponent;

    protected team!: Team;
    protected teamUsers: TeamUser[] = [
        {
            user: {},
            role: ''
        }
    ];
    protected submitted: boolean = false;
    protected saving: boolean = false;
    protected showDrawer: boolean = false;
    protected usersLookupOptions: LookupOptions = {
        label: 'Peek an user',
        placeholder: 'Type user name',
        dataKey: 'id',
        optionLabel: 'name',
        inputId: 'userTeamLookup'
    };
    protected autoFilteredUsers: any[] = [];
    protected isLoading: boolean = false;
    protected pageResponse: PageResponse = {
        currentPage: 0,
        totalItems: 0,
        totalPages: 0
    };

    protected selectedUserId: string | null = null;
    protected selectedUserRole: string | null = null;

    private readonly TeamUserIndex: number = 0;
    private readonly TeamRepoIndex: number = 1;

    /**
     *
     */
    constructor(
        private teamsRepository: TeamsRepositoryService,
        private messageHandler: MessageHandlerService,
        private confirmationService: ConfirmationService
    ) {}

    public showTeam(team: Team) {
        this.team = team;
        this.showDrawer = true;
    }

    public hideDialog() {
        this.showDrawer = false;
        this.saving = false;
    }

    protected onAccordionOpen(event: AccordionTabOpenEvent) {
        if (event.index == this.TeamUserIndex) this.loadTeamUsers();
        if (event.index == this.TeamRepoIndex) this.loadTeamRepos();
    }

    protected saveProduct() {
        this.saving = true;
        this.onSaveTeam.emit(this.team);
        this.hideDialog();
    }

    private loadTeamRepos() {
        this.messageHandler.showError('Not implemented yet', 'Error');
    }

    private loadTeamUsers() {
        if (!this.team.externalId) return;
        this.teamsRepository.getUsersFromTeam(this.team.externalId).subscribe({
            next: (response) => (this.teamUsers = response),
            error: (err) => {
                this.messageHandler.handleHttpError(
                    err,
                    `Error requesting ${this.team.name + ' '}teams' users`
                );
            }
        });
    }

    removeUser(teamUser: TeamUser): void {
        this.confirmationService.confirm({
            message: `Are you sure you want to remove <strong>${teamUser.user?.name}</strong> from ${this.team.name}?`,
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                if (!this.team.externalId) return;
                if (!teamUser.user?.id) return;
                this.teamsRepository
                    .deleteUserFromTeam(this.team.externalId, teamUser.user.id)
                    .subscribe({
                        next: (response) => {
                            this.teamUsers = response;
                            this.messageHandler.addSuccess(
                                `User ${teamUser.user?.name} removed successfully from ${this.team.name}`
                            );
                        },
                        error: (err) =>
                            this.messageHandler.handleHttpError(
                                err,
                                `Error while removing ${teamUser.user?.name ?? 'user'} from team.`
                            )
                    });
            }
        });
    }

    showAddUserDialog() {
        this.teamUserDialog.showDialog(this.team);
    }

    addUser(teamUser: TeamUser) {
        if (!this.team.externalId) {
            this.messageHandler.showError(
                'A team was not selected early.',
                'Invalid data'
            );

            return;
        }

        if (!teamUser.user?.id) {
            this.messageHandler.showError(
                'A user was not selected.\nYou need to peek one before continue.',
                'Invalid data'
            );

            return;
        }

        this.teamsRepository
            .addUserToTeam(
                this.team.externalId,
                teamUser.user?.id,
                teamUser.role ?? 'Developer'
            )
            .subscribe({
                next: (response) => {
                    this.teamUsers = response;
                    this.messageHandler.addSuccess(
                        `User ${teamUser.user?.name} added successfully to ${this.team.name}.`
                    );
                },
                error: (err) =>
                    this.messageHandler.handleHttpError(
                        err,
                        `Error while try add ${teamUser.user?.name ?? 'a new user'}`
                    )
            });
    }
}

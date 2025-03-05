import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { IftaLabelModule } from 'primeng/iftalabel';
import { InputTextModule } from 'primeng/inputtext';

import {
    LazyQuery,
    LookupOptions,
    PageResponse,
    PaginatedLookupComponent
} from '../../../../shared/components/paginated-lookup/paginated-lookup.component';
import { MessageHandlerService } from '../../../../shared/services/message-handler.service';
import { Team } from '../../../service/teamsRepository/Models/team';
import { TeamUser } from '../../../service/teamsRepository/Models/TeamUser';
import { User } from '../../../service/users-repository/Models/User';
import { UserRepositoryService } from '../../../service/users-repository/user-repository.service';

@Component({
    selector: 'app-team-user-dialog',
    imports: [
        ButtonModule,
        CommonModule,
        DialogModule,
        FormsModule,
        IftaLabelModule,
        InputTextModule,
        PaginatedLookupComponent
    ],
    templateUrl: './team-user-dialog.component.html',
    styleUrl: './team-user-dialog.component.scss'
})
export class TeamUserDialogComponent {
    private readonly PageSize = 50;
    public visible: boolean = false;
    public teamUser: TeamUser = {};

    @Output()
    public onAdd: EventEmitter<TeamUser> = new EventEmitter<TeamUser>();

    @ViewChild(PaginatedLookupComponent)
    private paginatedLookupComponent!: PaginatedLookupComponent;

    protected team: Team = {};
    protected isLoading: boolean = false;
    protected pageResponse: PageResponse = {
        currentPage: 0,
        totalItems: 0,
        totalPages: 0
    };
    protected selectedUserId: string | null = null;
    protected autoFilteredUsers: User[] = [];

    protected usersLookupOptions: LookupOptions = {
        label: 'Pick a user',
        placeholder: 'Type user name',
        dataKey: 'id',
        optionLabel: 'name',
        inputId: 'userTeamLookup'
    };

    /**
     *
     */
    constructor(
        private userRepository: UserRepositoryService,
        private messageHandler: MessageHandlerService
    ) {}

    showDialog(team: Team) {
        this.team = team;
        this.teamUser = {};
        this.paginatedLookupComponent.clear();
        this.autoFilteredUsers = [];
        this.selectedUserId = null;
        this.visible = true;
        this.pageResponse = {
            currentPage: 0,
            totalItems: 0,
            totalPages: 0
        };

        this.isLoading = false;
        setTimeout(() => this.paginatedLookupComponent.applyFocus(), 200);
    }

    protected loadUserData(event: LazyQuery) {
        this.isLoading = true;

        this.userRepository
            .getAll(`${event.params}*`, true, {
                page: event.page,
                size: this.PageSize,
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
                        console.log('lazyCall');
                        this.autoFilteredUsers = [
                            ...this.autoFilteredUsers,
                            ...response.data
                        ];
                        this.isLoading = false;
                    } else {
                        this.autoFilteredUsers = response.data;
                    }
                },
                error: (err) => {
                    this.isLoading = false;
                    this.messageHandler.handleHttpError(err);
                }
            });
    }

    protected saveClick(event: MouseEvent) {
        var selectedUser = this.autoFilteredUsers.find(
            (user) => user.id == this.selectedUserId
        );

        this.teamUser.user = selectedUser;

        if (!this.teamUser.user?.id) {
            console.log('Choose a user first and then try add again.');
            return;
        }

        this.onAdd.emit(this.teamUser);
        this.visible = false;
    }
}

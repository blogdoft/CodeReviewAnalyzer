import { Component } from '@angular/core';
import { DialogModule } from 'primeng/dialog';
import { FieldsetModule } from 'primeng/fieldset';
import { InputTextModule } from 'primeng/inputtext';

import { PullRequestStats } from '../../../service/pull-request-repository/models/pull-request-stats';
import { PullRequestRepositoryService } from '../../../service/pull-request-repository/pull-request-repository.service';
import { CommonModule } from '@angular/common';
import { MinutesToHoursPipe } from '../../../../shared/directives/minutes-to-hours.pipe';

@Component({
    standalone: true,
    selector: 'app-pull-request-stats-dialog',
    imports: [
        CommonModule,
        DialogModule,
        FieldsetModule,
        InputTextModule,
        MinutesToHoursPipe
    ],
    providers: [PullRequestRepositoryService],
    templateUrl: './pull-request-stats-dialog.component.html',
    styleUrl: './pull-request-stats-dialog.component.scss'
})
export class PullRequestStatsDialogComponent {
    protected visible: boolean = false;
    protected pullRequestStats: PullRequestStats = {};

    constructor(private pullRequestRepository: PullRequestRepositoryService) {}

    public showStatsFor(externalId: string) {
        this.pullRequestRepository.getStatsFor(externalId).subscribe({
            next: (response) => {
                this.pullRequestStats = response;
                this.visible = true;
            }
        });
    }
}

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PullRequestStatsDialogComponent } from './pull-request-stats-dialog.component';

describe('PullRequestStatsDialogComponent', () => {
    let component: PullRequestStatsDialogComponent;
    let fixture: ComponentFixture<PullRequestStatsDialogComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [PullRequestStatsDialogComponent]
        }).compileComponents();

        fixture = TestBed.createComponent(PullRequestStatsDialogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});

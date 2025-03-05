import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamUserDialogComponent } from './team-user-dialog.component';

describe('TeamUserDialogComponent', () => {
    let component: TeamUserDialogComponent;
    let fixture: ComponentFixture<TeamUserDialogComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [TeamUserDialogComponent]
        }).compileComponents();

        fixture = TestBed.createComponent(TeamUserDialogComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});

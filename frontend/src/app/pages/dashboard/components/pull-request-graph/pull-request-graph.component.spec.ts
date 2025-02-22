import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PullRequestGraphComponent } from './pull-request-graph.component';

describe('PullRequestGraphComponent', () => {
    let component: PullRequestGraphComponent;
    let fixture: ComponentFixture<PullRequestGraphComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [PullRequestGraphComponent]
        }).compileComponents();

        fixture = TestBed.createComponent(PullRequestGraphComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});

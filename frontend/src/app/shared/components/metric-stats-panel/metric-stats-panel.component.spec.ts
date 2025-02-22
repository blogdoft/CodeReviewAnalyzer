import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MetricStatsPanelComponent } from './metric-stats-panel.component';

describe('MetricStatsPanelComponent', () => {
    let component: MetricStatsPanelComponent;
    let fixture: ComponentFixture<MetricStatsPanelComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [MetricStatsPanelComponent]
        }).compileComponents();

        fixture = TestBed.createComponent(MetricStatsPanelComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});

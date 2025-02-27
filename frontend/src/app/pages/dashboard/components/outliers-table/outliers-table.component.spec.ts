import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OutliersTableComponent } from './outliers-table.component';

describe('OutliersTableComponent', () => {
    let component: OutliersTableComponent;
    let fixture: ComponentFixture<OutliersTableComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [OutliersTableComponent]
        }).compileComponents();

        fixture = TestBed.createComponent(OutliersTableComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PaginatedLookupComponent } from './paginated-lookup.component';

describe('PaginatedLookupComponent', () => {
    let component: PaginatedLookupComponent;
    let fixture: ComponentFixture<PaginatedLookupComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [PaginatedLookupComponent]
        }).compileComponents();

        fixture = TestBed.createComponent(PaginatedLookupComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});

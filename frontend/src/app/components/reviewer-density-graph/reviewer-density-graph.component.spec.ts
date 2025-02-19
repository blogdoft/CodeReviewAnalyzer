import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewerDensityGraphComponent } from './reviewer-density-graph.component';

describe('ReviewerDensityGraphComponent', () => {
  let component: ReviewerDensityGraphComponent;
  let fixture: ComponentFixture<ReviewerDensityGraphComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReviewerDensityGraphComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReviewerDensityGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

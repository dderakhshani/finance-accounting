import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MatErrorSummaryComponent } from './mat-error-summary.component';

describe('MatErrorSummaryComponent', () => {
  let component: MatErrorSummaryComponent;
  let fixture: ComponentFixture<MatErrorSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MatErrorSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MatErrorSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

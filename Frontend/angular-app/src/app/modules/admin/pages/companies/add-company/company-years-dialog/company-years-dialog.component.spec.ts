import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyYearsDialogComponent } from './company-years-dialog.component';

describe('CompanyYearsDialogComponent', () => {
  let component: CompanyYearsDialogComponent;
  let fixture: ComponentFixture<CompanyYearsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyYearsDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyYearsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

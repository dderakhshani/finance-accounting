import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyYearsListComponent } from './company-years-list.component';

describe('CompanyYearsListComponent', () => {
  let component: CompanyYearsListComponent;
  let fixture: ComponentFixture<CompanyYearsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyYearsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanyYearsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

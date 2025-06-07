import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoVoucherFormulaListComponent } from './auto-voucher-formula-list.component';

describe('AutoVoucherFormulaListComponent', () => {
  let component: AutoVoucherFormulaListComponent;
  let fixture: ComponentFixture<AutoVoucherFormulaListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutoVoucherFormulaListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AutoVoucherFormulaListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

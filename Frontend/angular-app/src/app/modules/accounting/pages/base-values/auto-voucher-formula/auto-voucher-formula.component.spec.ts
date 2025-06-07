import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoVoucherFormulaComponent } from './auto-voucher-formula.component';

describe('AutoVoucherFormulaComponent', () => {
  let component: AutoVoucherFormulaComponent;
  let fixture: ComponentFixture<AutoVoucherFormulaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutoVoucherFormulaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AutoVoucherFormulaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

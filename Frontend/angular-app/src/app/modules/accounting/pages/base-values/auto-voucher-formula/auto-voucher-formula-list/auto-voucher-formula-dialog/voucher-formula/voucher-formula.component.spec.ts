import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VoucherFormulaComponent } from './voucher-formula.component';

describe('VoucherFormulaComponent', () => {
  let component: VoucherFormulaComponent;
  let fixture: ComponentFixture<VoucherFormulaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VoucherFormulaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VoucherFormulaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

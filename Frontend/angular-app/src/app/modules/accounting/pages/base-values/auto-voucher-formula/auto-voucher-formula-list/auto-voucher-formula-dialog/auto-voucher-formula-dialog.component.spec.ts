import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoVoucherFormulaDialogComponent } from './auto-voucher-formula-dialog.component';

describe('AutoVoucherFormulaDialogComponent', () => {
  let component: AutoVoucherFormulaDialogComponent;
  let fixture: ComponentFixture<AutoVoucherFormulaDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutoVoucherFormulaDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AutoVoucherFormulaDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

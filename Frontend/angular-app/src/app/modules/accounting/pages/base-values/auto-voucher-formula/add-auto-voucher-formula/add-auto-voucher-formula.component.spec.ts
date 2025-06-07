import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAutoVoucherFormulaComponent } from './add-auto-voucher-formula.component';

describe('AddAutoVoucherFormulaComponent', () => {
  let component: AddAutoVoucherFormulaComponent;
  let fixture: ComponentFixture<AddAutoVoucherFormulaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddAutoVoucherFormulaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAutoVoucherFormulaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

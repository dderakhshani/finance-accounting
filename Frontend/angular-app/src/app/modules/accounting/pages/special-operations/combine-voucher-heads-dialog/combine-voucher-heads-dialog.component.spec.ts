import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CombineVoucherHeadsDialogComponent } from './combine-voucher-heads-dialog.component';

describe('CombineVoucherHeadsDialogComponent', () => {
  let component: CombineVoucherHeadsDialogComponent;
  let fixture: ComponentFixture<CombineVoucherHeadsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CombineVoucherHeadsDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CombineVoucherHeadsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

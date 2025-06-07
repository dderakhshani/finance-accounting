import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VoucherHeadHistoryDialogComponent } from './voucher-head-history-dialog.component';

describe('VoucherHeadHistoryDialogComponent', () => {
  let component: VoucherHeadHistoryDialogComponent;
  let fixture: ComponentFixture<VoucherHeadHistoryDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VoucherHeadHistoryDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VoucherHeadHistoryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

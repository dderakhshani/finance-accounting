import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LockVoucherHeadComponent } from './lock-voucher-head.component';

describe('LockVoucherHeadComponent', () => {
  let component: LockVoucherHeadComponent;
  let fixture: ComponentFixture<LockVoucherHeadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LockVoucherHeadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LockVoucherHeadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

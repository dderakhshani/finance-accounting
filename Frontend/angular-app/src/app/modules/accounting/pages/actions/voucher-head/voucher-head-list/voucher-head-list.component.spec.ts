import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VoucherHeadListComponent } from './voucher-head-list.component';

describe('VoucherHeadListComponent', () => {
  let component: VoucherHeadListComponent;
  let fixture: ComponentFixture<VoucherHeadListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VoucherHeadListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VoucherHeadListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

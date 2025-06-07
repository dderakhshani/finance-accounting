import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VoucherDetailsListComponent } from './voucher-details-list.component';

describe('VoucherDetailsListComponent', () => {
  let component: VoucherDetailsListComponent;
  let fixture: ComponentFixture<VoucherDetailsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VoucherDetailsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VoucherDetailsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

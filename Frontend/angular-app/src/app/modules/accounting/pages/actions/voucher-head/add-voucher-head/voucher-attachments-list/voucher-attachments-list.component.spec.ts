import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VoucherAttachmentsListComponent } from './voucher-attachments-list.component';

describe('VoucherAttachmentsListComponent', () => {
  let component: VoucherAttachmentsListComponent;
  let fixture: ComponentFixture<VoucherAttachmentsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VoucherAttachmentsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VoucherAttachmentsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

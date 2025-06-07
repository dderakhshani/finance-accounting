import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddVoucherHeadComponent } from './add-voucher-head.component';

describe('AddVoucherHeadComponent', () => {
  let component: AddVoucherHeadComponent;
  let fixture: ComponentFixture<AddVoucherHeadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddVoucherHeadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddVoucherHeadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

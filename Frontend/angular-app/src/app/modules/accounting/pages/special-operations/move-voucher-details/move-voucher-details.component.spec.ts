import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoveVoucherDetailsComponent } from './move-voucher-details.component';

describe('MoveVoucherDetailsComponent', () => {
  let component: MoveVoucherDetailsComponent;
  let fixture: ComponentFixture<MoveVoucherDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MoveVoucherDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MoveVoucherDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

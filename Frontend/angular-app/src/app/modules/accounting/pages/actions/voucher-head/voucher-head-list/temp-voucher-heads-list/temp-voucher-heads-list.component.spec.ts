import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TempVoucherHeadsListComponent } from './temp-voucher-heads-list.component';

describe('TempVoucherHeadsListComponent', () => {
  let component: TempVoucherHeadsListComponent;
  let fixture: ComponentFixture<TempVoucherHeadsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TempVoucherHeadsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TempVoucherHeadsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RenumberVoucherHeadsComponent } from './renumber-voucher-heads.component';

describe('RenumberVoucherHeadsComponent', () => {
  let component: RenumberVoucherHeadsComponent;
  let fixture: ComponentFixture<RenumberVoucherHeadsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RenumberVoucherHeadsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RenumberVoucherHeadsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

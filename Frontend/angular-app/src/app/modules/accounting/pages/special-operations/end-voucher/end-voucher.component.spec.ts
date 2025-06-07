import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EndVoucherComponent } from './end-voucher.component';

describe('EndVoucherComponent', () => {
  let component: EndVoucherComponent;
  let fixture: ComponentFixture<EndVoucherComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EndVoucherComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EndVoucherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StartVoucherComponent } from './start-voucher.component';

describe('StartVoucherComponent', () => {
  let component: StartVoucherComponent;
  let fixture: ComponentFixture<StartVoucherComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StartVoucherComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StartVoucherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

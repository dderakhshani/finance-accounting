import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InsertBetweenVoucherHeadsComponent } from './insert-between-voucher-heads.component';

describe('InsertBetweenVoucherHeadsComponent', () => {
  let component: InsertBetweenVoucherHeadsComponent;
  let fixture: ComponentFixture<InsertBetweenVoucherHeadsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InsertBetweenVoucherHeadsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InsertBetweenVoucherHeadsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

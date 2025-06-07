import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccumulateReceiptComponent } from './accumulate-receipt.component';

describe('AccumulateReceiptComponent', () => {
  let component: AccumulateReceiptComponent;
  let fixture: ComponentFixture<AccumulateReceiptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccumulateReceiptComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccumulateReceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

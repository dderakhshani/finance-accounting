import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TemporaryReceiptPrintComponent } from './temporary-receipt-print.component';

describe('TemporaryReceiptPrintComponent', () => {
  let component: TemporaryReceiptPrintComponent;
  let fixture: ComponentFixture<TemporaryReceiptPrintComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TemporaryReceiptPrintComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TemporaryReceiptPrintComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

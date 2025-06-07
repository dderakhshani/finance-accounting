import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveReceiptWithMessageDialogComponent } from './remove-receipt-with-message-dialog.component';

describe('RemoveReceiptWithMessageDialogComponent', () => {
  let component: RemoveReceiptWithMessageDialogComponent;
  let fixture: ComponentFixture<RemoveReceiptWithMessageDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RemoveReceiptWithMessageDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RemoveReceiptWithMessageDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTemporaryReceiptComponent } from './add-temporary-receipt.component';

describe('AddTemporaryReceiptComponent', () => {
  let component: AddTemporaryReceiptComponent;
  let fixture: ComponentFixture<AddTemporaryReceiptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddTemporaryReceiptComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddTemporaryReceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

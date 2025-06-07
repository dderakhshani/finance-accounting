import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddManualTemporaryReceiptComponent } from './add-manual-temporary-receipt.component';

describe('AddManualTemporaryReceiptComponent', () => {
  let component: AddManualTemporaryReceiptComponent;
  let fixture: ComponentFixture<AddManualTemporaryReceiptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddManualTemporaryReceiptComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddManualTemporaryReceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

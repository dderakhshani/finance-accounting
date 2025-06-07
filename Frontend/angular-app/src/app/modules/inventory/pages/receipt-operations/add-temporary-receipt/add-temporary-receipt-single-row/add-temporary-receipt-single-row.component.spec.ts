import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddTemporaryReceiptSingleRowComponent } from './add-temporary-receipt-single-row.component';
describe('AddTemporaryReceiptSingleRowComponent', () => {
  let component: AddTemporaryReceiptSingleRowComponent;
  let fixture: ComponentFixture<AddTemporaryReceiptSingleRowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddTemporaryReceiptSingleRowComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddTemporaryReceiptSingleRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

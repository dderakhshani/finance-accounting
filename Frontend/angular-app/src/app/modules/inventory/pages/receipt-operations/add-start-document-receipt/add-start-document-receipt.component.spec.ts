import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddStartDocumentReceiptComponent } from './add-start-document-receipt.component';

describe('AddStartDocumentReceiptComponent', () => {
  let component: AddStartDocumentReceiptComponent;
  let fixture: ComponentFixture<AddStartDocumentReceiptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddStartDocumentReceiptComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddStartDocumentReceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

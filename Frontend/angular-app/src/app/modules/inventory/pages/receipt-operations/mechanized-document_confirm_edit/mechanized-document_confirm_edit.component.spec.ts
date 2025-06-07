import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReceiptDetailsComponent } from '../../component/receipt-details/receipt-details.component';





describe('ReceiptDetailsComponent', () => {
  let component: ReceiptDetailsComponent;
  let fixture: ComponentFixture<ReceiptDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ReceiptDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceiptDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

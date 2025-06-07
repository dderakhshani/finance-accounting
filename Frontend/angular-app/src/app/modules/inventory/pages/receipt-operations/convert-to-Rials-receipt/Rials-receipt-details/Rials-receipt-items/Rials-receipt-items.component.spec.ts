import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RialsReceiptItemsComponent } from './Rials-receipt-items.component';



describe('RialsReceiptItemsComponent', () => {
  let component: RialsReceiptItemsComponent;
  let fixture: ComponentFixture<RialsReceiptItemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RialsReceiptItemsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RialsReceiptItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

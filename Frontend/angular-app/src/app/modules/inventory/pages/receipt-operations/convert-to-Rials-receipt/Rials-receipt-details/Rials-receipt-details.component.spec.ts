import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RialsReceiptDetailsComponent } from './Rials-receipt-details.component';



describe('RialsReceiptDetailsComponent', () => {
  let component: RialsReceiptDetailsComponent;
  let fixture: ComponentFixture<RialsReceiptDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RialsReceiptDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RialsReceiptDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

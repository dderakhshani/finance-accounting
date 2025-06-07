import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RialsReceiptListComponent } from './Rials-receipt-list.component';


describe('RialsReceiptListComponent', () => {
  let component: RialsReceiptListComponent;
  let fixture: ComponentFixture<RialsReceiptListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RialsReceiptListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RialsReceiptListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

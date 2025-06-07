import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RialsReceiptComponent } from './Rials-receipt.component';


describe('RialsReceiptListComponent', () => {
  let component: RialsReceiptComponent;
  let fixture: ComponentFixture<RialsReceiptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RialsReceiptComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RialsReceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

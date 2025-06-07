import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReceiptListComponent } from './receipt-list.component';


describe('ReceiptListComponent', () => {
  let component: ReceiptListComponent;
  let fixture: ComponentFixture<ReceiptListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReceiptListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceiptListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

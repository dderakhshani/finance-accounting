import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComboInvoiceComponent } from '../combo-invoice/combo-invoice.component';


describe('ComboInvoiceComponent', () => {
  let component: ComboInvoiceComponent;
  let fixture: ComponentFixture<ComboInvoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ComboInvoiceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComboInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

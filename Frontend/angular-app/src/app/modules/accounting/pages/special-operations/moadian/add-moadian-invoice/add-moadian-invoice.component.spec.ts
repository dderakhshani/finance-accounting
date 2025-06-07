import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMoadianInvoiceComponent } from './add-moadian-invoice.component';

describe('AddMoadianInvoiceComponent', () => {
  let component: AddMoadianInvoiceComponent;
  let fixture: ComponentFixture<AddMoadianInvoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddMoadianInvoiceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMoadianInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

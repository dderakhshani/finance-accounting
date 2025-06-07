import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoadianInvoiceDetailsListComponent } from './moadian-invoice-details-list.component';

describe('MoadianInvoiceDetailsListComponent', () => {
  let component: MoadianInvoiceDetailsListComponent;
  let fixture: ComponentFixture<MoadianInvoiceDetailsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MoadianInvoiceDetailsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MoadianInvoiceDetailsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

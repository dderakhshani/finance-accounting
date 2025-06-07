import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoadianInvoiceDetailDialogComponent } from './moadian-invoice-detail-dialog.component';

describe('MoadianInvoiceDetailDialogComponent', () => {
  let component: MoadianInvoiceDetailDialogComponent;
  let fixture: ComponentFixture<MoadianInvoiceDetailDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MoadianInvoiceDetailDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MoadianInvoiceDetailDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

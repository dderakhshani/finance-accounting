import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateMoadianInvoicesStatusByIdsDialogComponent } from './update-moadian-invoices-status-by-ids-dialog.component';

describe('UpdateMoadianInvoicesStatusByIdsDialogComponent', () => {
  let component: UpdateMoadianInvoicesStatusByIdsDialogComponent;
  let fixture: ComponentFixture<UpdateMoadianInvoicesStatusByIdsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateMoadianInvoicesStatusByIdsDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateMoadianInvoicesStatusByIdsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddChequeAttachmentsComponent } from './add-cheque-attachments.component';

describe('AddChequeAttachmentsComponent', () => {
  let component: AddChequeAttachmentsComponent;
  let fixture: ComponentFixture<AddChequeAttachmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddChequeAttachmentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddChequeAttachmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

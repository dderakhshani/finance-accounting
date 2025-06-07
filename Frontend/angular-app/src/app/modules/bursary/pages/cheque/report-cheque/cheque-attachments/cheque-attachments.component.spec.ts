import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChequeAttachmentsComponent } from './cheque-attachments.component';

describe('ChequeAttachmentsComponent', () => {
  let component: ChequeAttachmentsComponent;
  let fixture: ComponentFixture<ChequeAttachmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChequeAttachmentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChequeAttachmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

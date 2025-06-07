import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowChequeSheetAttachmentsComponent } from './show-cheque-sheet-attachments.component';

describe('ShowChequeSheetAttachmentsComponent', () => {
  let component: ShowChequeSheetAttachmentsComponent;
  let fixture: ComponentFixture<ShowChequeSheetAttachmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowChequeSheetAttachmentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowChequeSheetAttachmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

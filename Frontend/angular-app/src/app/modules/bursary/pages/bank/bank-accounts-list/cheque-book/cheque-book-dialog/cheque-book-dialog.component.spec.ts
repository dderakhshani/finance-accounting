import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChequeBookDialogComponent } from './cheque-book-dialog.component';

describe('ChequeBookDialogComponent', () => {
  let component: ChequeBookDialogComponent;
  let fixture: ComponentFixture<ChequeBookDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChequeBookDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChequeBookDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

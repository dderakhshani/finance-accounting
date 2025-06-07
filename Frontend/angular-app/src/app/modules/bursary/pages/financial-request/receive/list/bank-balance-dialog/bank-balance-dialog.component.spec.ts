import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BankBalanceDialogComponent } from './bank-balance-dialog.component';

describe('BankBalanceDialogComponent', () => {
  let component: BankBalanceDialogComponent;
  let fixture: ComponentFixture<BankBalanceDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BankBalanceDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BankBalanceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

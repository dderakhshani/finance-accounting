import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonBankAccountDialogComponent } from './person-bank-account-dialog.component';

describe('PersonBankAccountDialogComponent', () => {
  let component: PersonBankAccountDialogComponent;
  let fixture: ComponentFixture<PersonBankAccountDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonBankAccountDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonBankAccountDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

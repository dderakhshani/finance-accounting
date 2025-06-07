import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonBankAccountsListComponent } from './person-bank-accounts-list.component';

describe('PersonBankAccountsListComponent', () => {
  let component: PersonBankAccountsListComponent;
  let fixture: ComponentFixture<PersonBankAccountsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonBankAccountsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonBankAccountsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

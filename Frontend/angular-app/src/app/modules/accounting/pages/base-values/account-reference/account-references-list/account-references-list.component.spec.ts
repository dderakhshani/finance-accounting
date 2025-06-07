import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountReferencesListComponent } from './account-references-list.component';

describe('AccountReferencesListComponent', () => {
  let component: AccountReferencesListComponent;
  let fixture: ComponentFixture<AccountReferencesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountReferencesListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountReferencesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountReferencesGroupComponent } from './account-references-group.component';

describe('AccountReferencesGroupComponent', () => {
  let component: AccountReferencesGroupComponent;
  let fixture: ComponentFixture<AccountReferencesGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountReferencesGroupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountReferencesGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

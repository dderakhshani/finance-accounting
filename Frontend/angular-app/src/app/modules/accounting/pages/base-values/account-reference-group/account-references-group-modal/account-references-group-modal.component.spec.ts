import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountReferencesGroupModalComponent } from './account-references-group-modal.component';

describe('AccountReferencesGroupModalComponent', () => {
  let component: AccountReferencesGroupModalComponent;
  let fixture: ComponentFixture<AccountReferencesGroupModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountReferencesGroupModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountReferencesGroupModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

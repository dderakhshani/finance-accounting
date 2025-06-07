import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountHeadModalComponent } from './account-head-modal.component';

describe('AccountHeadModalComponent', () => {
  let component: AccountHeadModalComponent;
  let fixture: ComponentFixture<AccountHeadModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountHeadModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountHeadModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

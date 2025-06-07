import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountHeadComponent } from './account-head.component';

describe('AccountHeadComponent', () => {
  let component: AccountHeadComponent;
  let fixture: ComponentFixture<AccountHeadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountHeadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountHeadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserYearListComponent } from './user-year-list.component';

describe('UserYearListComponent', () => {
  let component: UserYearListComponent;
  let fixture: ComponentFixture<UserYearListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserYearListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserYearListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

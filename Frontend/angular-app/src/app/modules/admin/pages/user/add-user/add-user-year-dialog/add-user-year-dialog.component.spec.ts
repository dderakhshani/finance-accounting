import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUserYearDialogComponent } from './add-user-year-dialog.component';

describe('AddUserYearDialogComponent', () => {
  let component: AddUserYearDialogComponent;
  let fixture: ComponentFixture<AddUserYearDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddUserYearDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUserYearDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

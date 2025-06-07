import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonPhoneDialogComponent } from './person-phone-dialog.component';

describe('PersonPhoneDialogComponent', () => {
  let component: PersonPhoneDialogComponent;
  let fixture: ComponentFixture<PersonPhoneDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonPhoneDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonPhoneDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

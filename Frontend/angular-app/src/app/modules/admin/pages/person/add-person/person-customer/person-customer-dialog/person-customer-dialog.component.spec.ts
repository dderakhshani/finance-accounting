import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonCustomerDialogComponent } from './person-customer-dialog.component';

describe('PersonCustomerDialogComponent', () => {
  let component: PersonCustomerDialogComponent;
  let fixture: ComponentFixture<PersonCustomerDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonCustomerDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonCustomerDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

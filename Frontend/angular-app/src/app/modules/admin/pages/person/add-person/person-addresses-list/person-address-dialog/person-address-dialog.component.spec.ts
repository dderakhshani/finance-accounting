import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonAddressDialogComponent } from './person-address-dialog.component';

describe('PersonAddressDialogComponent', () => {
  let component: PersonAddressDialogComponent;
  let fixture: ComponentFixture<PersonAddressDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonAddressDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonAddressDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

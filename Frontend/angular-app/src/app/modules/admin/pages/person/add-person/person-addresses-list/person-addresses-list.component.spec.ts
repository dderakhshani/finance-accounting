import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonAddressesListComponent } from './person-addresses-list.component';

describe('PersonAddressesListComponent', () => {
  let component: PersonAddressesListComponent;
  let fixture: ComponentFixture<PersonAddressesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonAddressesListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonAddressesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

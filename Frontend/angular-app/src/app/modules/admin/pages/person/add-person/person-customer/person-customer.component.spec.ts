import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonCustomerComponent } from './person-customer.component';

describe('PersonCustomerComponent', () => {
  let component: PersonCustomerComponent;
  let fixture: ComponentFixture<PersonCustomerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonCustomerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

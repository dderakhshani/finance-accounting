import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonEmployeeComponent } from './person-employee.component';

describe('PersonEmployeeComponent', () => {
  let component: PersonEmployeeComponent;
  let fixture: ComponentFixture<PersonEmployeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonEmployeeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

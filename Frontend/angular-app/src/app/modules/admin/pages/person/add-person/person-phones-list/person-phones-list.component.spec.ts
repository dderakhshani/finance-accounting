import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonPhonesListComponent } from './person-phones-list.component';

describe('PersonPhonesListComponent', () => {
  let component: PersonPhonesListComponent;
  let fixture: ComponentFixture<PersonPhonesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PersonPhonesListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonPhonesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUsanceComponent } from './add-usance.component';

describe('AddUsanceComponent', () => {
  let component: AddUsanceComponent;
  let fixture: ComponentFixture<AddUsanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddUsanceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUsanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

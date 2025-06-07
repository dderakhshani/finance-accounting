import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAccountReferenceComponent } from './add-account-reference.component';

describe('AddAccountReferenceComponent', () => {
  let component: AddAccountReferenceComponent;
  let fixture: ComponentFixture<AddAccountReferenceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddAccountReferenceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAccountReferenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

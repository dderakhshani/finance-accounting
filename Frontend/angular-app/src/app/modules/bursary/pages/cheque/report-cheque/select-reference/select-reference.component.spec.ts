import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectReferenceComponent } from './select-reference.component';

describe('SelectReferenceComponent', () => {
  let component: SelectReferenceComponent;
  let fixture: ComponentFixture<SelectReferenceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectReferenceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectReferenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

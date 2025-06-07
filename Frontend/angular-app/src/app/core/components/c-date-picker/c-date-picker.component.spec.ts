import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CDatePickerComponent } from './c-date-picker.component';

describe('CDatePickerComponent', () => {
  let component: CDatePickerComponent;
  let fixture: ComponentFixture<CDatePickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CDatePickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CDatePickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

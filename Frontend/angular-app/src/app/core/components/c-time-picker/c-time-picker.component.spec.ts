import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CTimePickerComponent } from './c-time-picker.component';

describe('CTimePickerComponent', () => {
  let component: CTimePickerComponent;
  let fixture: ComponentFixture<CTimePickerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CTimePickerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CTimePickerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

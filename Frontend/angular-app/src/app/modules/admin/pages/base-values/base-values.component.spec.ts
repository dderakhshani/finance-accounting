import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseValuesComponent } from './base-values.component';

describe('BaseValuesComponent', () => {
  let component: BaseValuesComponent;
  let fixture: ComponentFixture<BaseValuesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BaseValuesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BaseValuesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseValueTypesComponent } from './base-value-types.component';

describe('BaseValueTypesComponent', () => {
  let component: BaseValueTypesComponent;
  let fixture: ComponentFixture<BaseValueTypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BaseValueTypesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BaseValueTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

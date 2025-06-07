import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseValueTypeDialogComponent } from './base-value-type-dialog.component';

describe('BaseValueTypesDialogComponent', () => {
  let component: BaseValueTypeDialogComponent;
  let fixture: ComponentFixture<BaseValueTypeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BaseValueTypeDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BaseValueTypeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

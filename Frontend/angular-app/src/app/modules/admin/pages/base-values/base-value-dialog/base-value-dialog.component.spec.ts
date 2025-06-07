import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseValueDialogComponent } from './base-value-dialog.component';

describe('BaseValueDialogComponent', () => {
  let component: BaseValueDialogComponent;
  let fixture: ComponentFixture<BaseValueDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BaseValueDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BaseValueDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

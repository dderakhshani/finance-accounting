import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HttpValidationErrorDialogComponent } from './http-validation-error-dialog.component';

describe('HttpValidationErrorDialogComponent', () => {
  let component: HttpValidationErrorDialogComponent;
  let fixture: ComponentFixture<HttpValidationErrorDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HttpValidationErrorDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HttpValidationErrorDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

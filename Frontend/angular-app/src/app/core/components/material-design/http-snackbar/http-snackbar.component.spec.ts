import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HttpSnackbarComponent } from './http-snackbar.component';

describe('HttpSnackbarComponent', () => {
  let component: HttpSnackbarComponent;
  let fixture: ComponentFixture<HttpSnackbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HttpSnackbarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HttpSnackbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

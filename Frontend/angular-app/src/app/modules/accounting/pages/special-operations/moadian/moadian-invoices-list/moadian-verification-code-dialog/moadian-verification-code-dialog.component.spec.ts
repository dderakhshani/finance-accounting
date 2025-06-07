import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoadianVerificationCodeDialogComponent } from './moadian-verification-code-dialog.component';

describe('MoadianVerificationCodeDialogComponent', () => {
  let component: MoadianVerificationCodeDialogComponent;
  let fixture: ComponentFixture<MoadianVerificationCodeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MoadianVerificationCodeDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MoadianVerificationCodeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

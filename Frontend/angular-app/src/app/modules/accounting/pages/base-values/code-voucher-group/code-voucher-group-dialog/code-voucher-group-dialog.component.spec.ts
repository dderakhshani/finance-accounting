import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeVoucherGroupDialogComponent } from './code-voucher-group-dialog.component';

describe('CodeVoucherGroupDialogComponent', () => {
  let component: CodeVoucherGroupDialogComponent;
  let fixture: ComponentFixture<CodeVoucherGroupDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CodeVoucherGroupDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CodeVoucherGroupDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

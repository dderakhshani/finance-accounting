import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeVoucherExtendTypeDialogComponent } from './code-voucher-extend-type-dialog.component';

describe('CodeVoucherExtendTypeDialogComponent', () => {
  let component: CodeVoucherExtendTypeDialogComponent;
  let fixture: ComponentFixture<CodeVoucherExtendTypeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CodeVoucherExtendTypeDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CodeVoucherExtendTypeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

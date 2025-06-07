import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeVoucherExtendTypeComponent } from './code-voucher-extend-type.component';

describe('CodeVoucherExtendTypeComponent', () => {
  let component: CodeVoucherExtendTypeComponent;
  let fixture: ComponentFixture<CodeVoucherExtendTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CodeVoucherExtendTypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CodeVoucherExtendTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

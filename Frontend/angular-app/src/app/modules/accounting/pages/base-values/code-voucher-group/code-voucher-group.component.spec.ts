import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CodeVoucherGroupComponent } from './code-voucher-group.component';

describe('CodeVoucherGroupComponent', () => {
  let component: CodeVoucherGroupComponent;
  let fixture: ComponentFixture<CodeVoucherGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CodeVoucherGroupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CodeVoucherGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

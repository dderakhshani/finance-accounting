import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LedgerReport2Component } from './ledger-report2.component';

describe('LedgerReport2Component', () => {
  let component: LedgerReport2Component;
  let fixture: ComponentFixture<LedgerReport2Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LedgerReport2Component ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LedgerReport2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditDialogComponent } from './audit-dialog.component';

describe('AuditListDialogComponent', () => {
  let component: AuditDialogComponent;
  let fixture: ComponentFixture<AuditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuditDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

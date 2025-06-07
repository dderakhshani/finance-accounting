import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissionDialogComponent } from './permission-dialog.component';

describe('PermissionsDialogComponent', () => {
  let component: PermissionDialogComponent;
  let fixture: ComponentFixture<PermissionDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PermissionDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PermissionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

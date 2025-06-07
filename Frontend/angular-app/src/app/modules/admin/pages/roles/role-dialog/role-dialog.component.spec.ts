import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleDialogComponent } from './role-dialog.component';

describe('RolesDialogComponent', () => {
  let component: RoleDialogComponent;
  let fixture: ComponentFixture<RoleDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoleDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RoleDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

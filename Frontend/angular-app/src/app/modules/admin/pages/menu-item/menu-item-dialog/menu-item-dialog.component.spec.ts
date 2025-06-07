import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuItemDialogComponent } from './menu-item-dialog.component';

describe('MenuItemDialogComponent', () => {
  let component: MenuItemDialogComponent;
  let fixture: ComponentFixture<MenuItemDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MenuItemDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuItemDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuItemRolesListComponent } from './menu-item-roles-list.component';

describe('MenuItemRolesListComponent', () => {
  let component: MenuItemRolesListComponent;
  let fixture: ComponentFixture<MenuItemRolesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MenuItemRolesListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuItemRolesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

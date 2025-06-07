import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HelpSidebarItemComponent } from './help-sidebar-item.component';

describe('HelpSidebarItemComponent', () => {
  let component: HelpSidebarItemComponent;
  let fixture: ComponentFixture<HelpSidebarItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HelpSidebarItemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HelpSidebarItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

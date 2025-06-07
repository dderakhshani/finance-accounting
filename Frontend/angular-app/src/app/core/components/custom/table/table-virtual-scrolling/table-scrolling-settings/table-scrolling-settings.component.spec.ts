import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableScrollingSettingsComponent } from './table-scrolling-settings.component';

describe('TableScrollingSettingsComponent', () => {
  let component: TableScrollingSettingsComponent;
  let fixture: ComponentFixture<TableScrollingSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TableScrollingSettingsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TableScrollingSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

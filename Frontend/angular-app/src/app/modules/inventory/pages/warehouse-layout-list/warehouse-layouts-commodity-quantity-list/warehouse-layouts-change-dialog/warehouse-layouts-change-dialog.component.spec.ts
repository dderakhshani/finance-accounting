import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeWarehouseLayoutsDialogComponent } from './warehouse-layouts-change-dialog.component';

describe('WarehousDialogComponent', () => {
  let component: ChangeWarehouseLayoutsDialogComponent;
  let fixture: ComponentFixture<ChangeWarehouseLayoutsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ChangeWarehouseLayoutsDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeWarehouseLayoutsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

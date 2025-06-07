import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseLayoutsAddInventoryDialogComponent } from './warehouse-layouts-add-inventory-dialog.component';

describe('WarehousDialogComponent', () => {
  let component: WarehouseLayoutsAddInventoryDialogComponent;
  let fixture: ComponentFixture<WarehouseLayoutsAddInventoryDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WarehouseLayoutsAddInventoryDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseLayoutsAddInventoryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

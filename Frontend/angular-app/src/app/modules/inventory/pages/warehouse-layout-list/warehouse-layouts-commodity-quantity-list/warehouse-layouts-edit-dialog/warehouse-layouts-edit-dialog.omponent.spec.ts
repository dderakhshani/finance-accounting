import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditWarehouseLayoutsDialogComponent } from './warehouse-layouts-edit-dialog.component';

describe('WarehousDialogComponent', () => {
  let component: EditWarehouseLayoutsDialogComponent;
  let fixture: ComponentFixture<EditWarehouseLayoutsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EditWarehouseLayoutsDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditWarehouseLayoutsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

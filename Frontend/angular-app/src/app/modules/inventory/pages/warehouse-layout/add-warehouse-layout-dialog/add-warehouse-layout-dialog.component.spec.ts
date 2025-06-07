import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddWarehouseLayoutDialogComponent } from './add-warehouse-layout-dialog.component';

describe('WarehouseLayoutDialogComponent', () => {
  let component: AddWarehouseLayoutDialogComponent;
  let fixture: ComponentFixture<AddWarehouseLayoutDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddWarehouseLayoutDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddWarehouseLayoutDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

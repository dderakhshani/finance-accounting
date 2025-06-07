import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateWarehouseLayoutDialogComponent } from './update-warehouse-layout-dialog.component';

describe('UpdateWarehouseLayoutDialogComponent', () => {
  let component: UpdateWarehouseLayoutDialogComponent;
  let fixture: ComponentFixture<UpdateWarehouseLayoutDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateWarehouseLayoutDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateWarehouseLayoutDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

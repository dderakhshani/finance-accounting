import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseDialogComponent } from './warehouse-dialog.component';

describe('WarehousDialogComponent', () => {
  let component: WarehouseDialogComponent;
  let fixture: ComponentFixture<WarehouseDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

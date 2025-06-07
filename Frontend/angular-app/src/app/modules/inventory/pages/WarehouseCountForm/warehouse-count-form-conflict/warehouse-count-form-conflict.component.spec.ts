import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseCountFormConflictComponent } from './warehouse-count-form-conflict.component';

describe('WarehouseCountFormConflictComponent', () => {
  let component: WarehouseCountFormConflictComponent;
  let fixture: ComponentFixture<WarehouseCountFormConflictComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseCountFormConflictComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseCountFormConflictComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

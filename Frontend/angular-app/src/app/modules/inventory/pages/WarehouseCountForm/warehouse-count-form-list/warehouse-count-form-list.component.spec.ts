import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseCountFormListComponent } from './warehouse-count-form-list.component';

describe('WarehouseCountFormListComponent', () => {
  let component: WarehouseCountFormListComponent;
  let fixture: ComponentFixture<WarehouseCountFormListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseCountFormListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseCountFormListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

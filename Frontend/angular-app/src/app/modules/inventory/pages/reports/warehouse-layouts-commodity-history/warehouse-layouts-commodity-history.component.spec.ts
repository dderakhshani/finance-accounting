import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseLayoutsCommodityHistoryComponent } from './warehouse-layouts-commodity-history.component';

describe('WarehouseLayoutsCommodityHistoryComponent', () => {
  let component: WarehouseLayoutsCommodityHistoryComponent;
  let fixture: ComponentFixture<WarehouseLayoutsCommodityHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseLayoutsCommodityHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseLayoutsCommodityHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

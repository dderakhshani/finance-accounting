import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseStocksAvailableCommodityComponent } from './warehouse-stocks-available-commodity.component';

describe('WarehouseStocksAvailableCommodityComponent', () => {
  let component: WarehouseStocksAvailableCommodityComponent;
  let fixture: ComponentFixture<WarehouseStocksAvailableCommodityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WarehouseStocksAvailableCommodityComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseStocksAvailableCommodityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

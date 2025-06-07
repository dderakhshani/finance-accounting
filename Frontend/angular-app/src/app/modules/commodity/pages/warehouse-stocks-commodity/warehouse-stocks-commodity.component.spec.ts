import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WarehouseStocksCommodityComponent } from './warehouse-stocks-commoditycomponent';

describe('WarehouseStocksCommodityComponent', () => {
  let component: WarehouseStocksCommodityComponent;
  let fixture: ComponentFixture<WarehouseStocksCommodityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [WarehouseStocksCommodityComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseStocksCommodityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

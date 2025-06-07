import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WarehouseLayoutsCommodityQuantityComponent } from './warehouse-layouts-commodity-quantity.component';

describe('WarehouseLayoutsCommodityQuantityComponent', () => {
  let component: WarehouseLayoutsCommodityQuantityComponent;
  let fixture: ComponentFixture<WarehouseLayoutsCommodityQuantityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseLayoutsCommodityQuantityComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseLayoutsCommodityQuantityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

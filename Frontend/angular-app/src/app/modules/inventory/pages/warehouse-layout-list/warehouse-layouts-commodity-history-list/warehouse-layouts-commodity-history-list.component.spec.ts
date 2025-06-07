import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseLayoutsCommodityHistoryListComponent } from './warehouse-layouts-commodity-history-list.component';

describe('WarehouseLayoutsCommodityHistoryListComponent', () => {
  let component: WarehouseLayoutsCommodityHistoryListComponent;
  let fixture: ComponentFixture<WarehouseLayoutsCommodityHistoryListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseLayoutsCommodityHistoryListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseLayoutsCommodityHistoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

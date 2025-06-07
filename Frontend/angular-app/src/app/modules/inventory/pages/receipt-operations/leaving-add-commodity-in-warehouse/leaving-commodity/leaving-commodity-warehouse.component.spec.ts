import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LeavingCommodityWarehouseComponent } from './leaving-commodity-warehouse.component';


describe('LeavingCommodityWarehouseComponent', () => {
  let component: LeavingCommodityWarehouseComponent;
  let fixture: ComponentFixture<LeavingCommodityWarehouseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LeavingCommodityWarehouseComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LeavingCommodityWarehouseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

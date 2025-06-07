import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PlacementWarehouseDirectReceiptComponent } from './placement-warehouse-direct-receipt.component';



describe('PlacementWarehouseDirectReceiptComponent', () => {
  let component: PlacementWarehouseDirectReceiptComponent;
  let fixture: ComponentFixture<PlacementWarehouseDirectReceiptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlacementWarehouseDirectReceiptComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlacementWarehouseDirectReceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

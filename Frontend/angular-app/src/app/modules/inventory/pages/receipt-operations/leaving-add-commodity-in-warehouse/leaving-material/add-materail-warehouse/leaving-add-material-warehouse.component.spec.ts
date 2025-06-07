import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LeavingAddMarerialWarehouseComponent } from './leaving-add-material-warehouse.component';




describe('LeavingAddMarerialWarehouseComponent', () => {
  let component: LeavingAddMarerialWarehouseComponent;
  let fixture: ComponentFixture<LeavingAddMarerialWarehouseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LeavingAddMarerialWarehouseComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LeavingAddMarerialWarehouseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

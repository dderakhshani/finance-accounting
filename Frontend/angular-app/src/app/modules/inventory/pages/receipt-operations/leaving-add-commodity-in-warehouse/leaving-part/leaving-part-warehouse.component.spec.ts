import { ComponentFixture, TestBed } from '@angular/core/testing';
import { leavingPartWarehouseComponent } from './leaving-part-warehouse.component';




describe('leavingPartWarehouseComponent', () => {
  let component: leavingPartWarehouseComponent;
  let fixture: ComponentFixture<leavingPartWarehouseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [leavingPartWarehouseComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(leavingPartWarehouseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

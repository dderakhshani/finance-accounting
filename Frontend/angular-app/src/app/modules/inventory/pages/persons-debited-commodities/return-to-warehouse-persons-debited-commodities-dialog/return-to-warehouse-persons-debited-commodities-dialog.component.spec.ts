import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UpdateReturnToWarehousePersonsDebitedDialogComponent } from './return-to-warehouse-persons-debited-commodities-dialog.component';


describe('UpdateReturnToWarehousePersonsDebitedDialogComponent', () => {
  let component: UpdateReturnToWarehousePersonsDebitedDialogComponent;
  let fixture: ComponentFixture<UpdateReturnToWarehousePersonsDebitedDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UpdateReturnToWarehousePersonsDebitedDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateReturnToWarehousePersonsDebitedDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseCountFormDetailComponent } from './warehouse-count-form-detail.component';

describe('WarehouseCountFormDetailComponent', () => {
  let component: WarehouseCountFormDetailComponent;
  let fixture: ComponentFixture<WarehouseCountFormDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseCountFormDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseCountFormDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

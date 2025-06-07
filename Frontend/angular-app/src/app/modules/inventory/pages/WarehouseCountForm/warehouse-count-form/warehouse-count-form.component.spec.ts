import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseCountFormComponent } from './warehouse-count-form.component';

describe('WarehouseCountFormComponent', () => {
  let component: WarehouseCountFormComponent;
  let fixture: ComponentFixture<WarehouseCountFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseCountFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseCountFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

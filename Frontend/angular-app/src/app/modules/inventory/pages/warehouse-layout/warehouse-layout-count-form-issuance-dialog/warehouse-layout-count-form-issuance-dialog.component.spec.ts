import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarehouseLayoutCountFormIssuanceDialogComponent } from './warehouse-layout-count-form-issuance-dialog.component';

describe('WarehouseLayoutCountFormIssuanceDialogComponent', () => {
  let component: WarehouseLayoutCountFormIssuanceDialogComponent;
  let fixture: ComponentFixture<WarehouseLayoutCountFormIssuanceDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarehouseLayoutCountFormIssuanceDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarehouseLayoutCountFormIssuanceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

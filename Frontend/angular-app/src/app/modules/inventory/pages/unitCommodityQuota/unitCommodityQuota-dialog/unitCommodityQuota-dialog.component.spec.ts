import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UnitCommodityQuotaDialogComponent } from './unitCommodityQuota-dialog.component';

describe('UnitCommodityQuotaDialogComponent', () => {
  let component: UnitCommodityQuotaDialogComponent;
  let fixture: ComponentFixture<UnitCommodityQuotaDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UnitCommodityQuotaDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UnitCommodityQuotaDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

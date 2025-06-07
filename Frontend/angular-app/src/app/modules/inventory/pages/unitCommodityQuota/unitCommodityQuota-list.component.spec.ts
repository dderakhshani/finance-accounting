import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UnitCommodityQuotaListComponent } from './unitCommodityQuota-list.component';



describe('UnitCommodityQuotaListComponent', () => {
  let component: UnitCommodityQuotaListComponent;
  let fixture: ComponentFixture<UnitCommodityQuotaListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UnitCommodityQuotaListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UnitCommodityQuotaListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

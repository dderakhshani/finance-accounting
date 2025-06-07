import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AssetsCommoditySerialDialog } from './assets-commodity-serial-dialog.component';



describe('AssetsCommoditySerialDialog', () => {
  let component: AssetsCommoditySerialDialog;
  let fixture: ComponentFixture<AssetsCommoditySerialDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AssetsCommoditySerialDialog ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AssetsCommoditySerialDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

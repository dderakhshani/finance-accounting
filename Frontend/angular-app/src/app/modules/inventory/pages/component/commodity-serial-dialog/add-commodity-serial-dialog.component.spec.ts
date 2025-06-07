import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddCommoditySerialDialog } from './add-commodity-serial-dialog.component';


describe('AddCommoditySerialDialog', () => {
  let component: AddCommoditySerialDialog;
  let fixture: ComponentFixture<AddCommoditySerialDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddCommoditySerialDialog ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddCommoditySerialDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

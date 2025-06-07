import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComboCodeVoucherGroupsComponent } from './combo-code-voucher-groups.component';



describe('ComboCodeVoucherGroupsComponent', () => {
  let component: ComboCodeVoucherGroupsComponent;
  let fixture: ComponentFixture<ComboCodeVoucherGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ComboCodeVoucherGroupsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComboCodeVoucherGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

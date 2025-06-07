import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BomItemsComponent } from '../../../bom/bom-dialog/bom-items/bom-items.component';


describe('BomItemsComponent', () => {
  let component: BomItemsComponent;
  let fixture: ComponentFixture<BomItemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BomItemsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BomItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

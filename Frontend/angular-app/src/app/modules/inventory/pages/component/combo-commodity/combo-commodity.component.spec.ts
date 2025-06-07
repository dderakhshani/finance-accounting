import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComboCommodityComponent } from './combo-commodity.component';



describe('ComboCommodityComponent', () => {
  let component: ComboCommodityComponent;
  let fixture: ComponentFixture<ComboCommodityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ComboCommodityComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComboCommodityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

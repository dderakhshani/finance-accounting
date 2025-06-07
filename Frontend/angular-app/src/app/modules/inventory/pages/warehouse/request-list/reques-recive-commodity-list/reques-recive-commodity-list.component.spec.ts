import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RequesReciveCommodityListComponent } from './reques-recive-commodity-list.component';


describe('RequesReciveCommodityListComponent', () => {
  let component: RequesReciveCommodityListComponent;
  let fixture: ComponentFixture<RequesReciveCommodityListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RequesReciveCommodityListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RequesReciveCommodityListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

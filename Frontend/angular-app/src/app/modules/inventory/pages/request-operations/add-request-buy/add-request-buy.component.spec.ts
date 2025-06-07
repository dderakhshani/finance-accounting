import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddRequestBuyComponent } from './add-request-buy.component';




describe('AddRequestBuyComponent', () => {
  let component: AddRequestBuyComponent;
  let fixture: ComponentFixture<AddRequestBuyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddRequestBuyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddRequestBuyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

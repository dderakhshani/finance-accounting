import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddRequestCommodityComponent } from './add-request-commodity.component';

describe('AddRequestCommodityComponent', () => {
  let component: AddRequestCommodityComponent;
  let fixture: ComponentFixture<AddRequestCommodityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddRequestCommodityComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddRequestCommodityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { requesBuyListComponent } from '../../request-list/reques-buy-list/reques-buy-list.component';




describe('requesBuyListComponent', () => {
  let component: requesBuyListComponent;
  let fixture: ComponentFixture<requesBuyListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [requesBuyListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(requesBuyListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

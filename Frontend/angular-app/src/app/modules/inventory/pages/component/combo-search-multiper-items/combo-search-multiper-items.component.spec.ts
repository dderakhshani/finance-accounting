import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComboSearchMultiperItemsComponent } from './combo-search-multiper-items.component';




describe('ComboSearchMultiperItemsComponent', () => {
  let component: ComboSearchMultiperItemsComponent;
  let fixture: ComponentFixture<ComboSearchMultiperItemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ComboSearchMultiperItemsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComboSearchMultiperItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

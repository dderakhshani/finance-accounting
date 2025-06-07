import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComboSearchComponent } from './combo-search.component';




describe('ComboSearchComponent', () => {
  let component: ComboSearchComponent;
  let fixture: ComponentFixture<ComboSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ComboSearchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComboSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

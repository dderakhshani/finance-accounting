import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComboAccountRefrenceComponent } from './combo-account-refrence.component';


describe('ComboAccountRefrenceComponent', () => {
  let component: ComboAccountRefrenceComponent;
  let fixture: ComponentFixture<ComboAccountRefrenceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ComboAccountRefrenceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComboAccountRefrenceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

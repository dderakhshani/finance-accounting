import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComboAddSelectBaseValueComponent } from './combo-add-select-base-value.component';

describe('ComboInvoiceComponent', () => {
  let component: ComboAddSelectBaseValueComponent;
  let fixture: ComponentFixture<ComboAddSelectBaseValueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ComboAddSelectBaseValueComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComboAddSelectBaseValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

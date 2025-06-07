import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddFactorComponent } from './add-factor.component';



describe('AddManualTemporaryReceiptComponent', () => {
  let component: AddFactorComponent;
  let fixture: ComponentFixture<AddFactorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddFactorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddFactorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

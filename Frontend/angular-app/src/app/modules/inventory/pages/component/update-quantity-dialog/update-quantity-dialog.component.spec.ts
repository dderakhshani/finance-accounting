import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UpdateQuantityDialogComponent } from './update-quantity-dialog.component';




describe('UpdateQuantityDialogComponent', () => {
  let component: UpdateQuantityDialogComponent;
  let fixture: ComponentFixture<UpdateQuantityDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UpdateQuantityDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateQuantityDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

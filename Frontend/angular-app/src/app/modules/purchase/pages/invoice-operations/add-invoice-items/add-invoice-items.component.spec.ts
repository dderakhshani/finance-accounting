import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AddIncoiceItemsComponent } from './add-invoice-items.component';





describe('AddIncoiceItemsComponent', () => {
  let component: AddIncoiceItemsComponent;
  let fixture: ComponentFixture<AddIncoiceItemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddIncoiceItemsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddIncoiceItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

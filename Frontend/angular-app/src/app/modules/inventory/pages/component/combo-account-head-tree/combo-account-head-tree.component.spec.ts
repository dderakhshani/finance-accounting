import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComboAccountHeadTreeComponent } from './combo-account-head-tree.component';




describe('ComboAccountHeadTreeComponent', () => {
  let component: ComboAccountHeadTreeComponent;
  let fixture: ComponentFixture<ComboAccountHeadTreeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ComboAccountHeadTreeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComboAccountHeadTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

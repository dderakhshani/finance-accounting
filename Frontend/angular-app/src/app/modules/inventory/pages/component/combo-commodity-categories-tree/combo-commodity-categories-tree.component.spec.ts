import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ComboWarhouseTreeComponent } from '../combo-warhouse-tree/combo-warhouse-tree.component';




describe('ComboWarhouseTreeComponent', () => {
  let component: ComboWarhouseTreeComponent;
  let fixture: ComponentFixture<ComboWarhouseTreeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ComboWarhouseTreeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComboWarhouseTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

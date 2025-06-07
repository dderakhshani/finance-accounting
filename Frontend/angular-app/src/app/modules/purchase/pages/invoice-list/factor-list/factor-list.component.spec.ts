import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FactorListComponent } from './factor-list.component';



describe('FactorListComponent', () => {
  let component: FactorListComponent;
  let fixture: ComponentFixture<FactorListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FactorListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FactorListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ArchiveFactorListComponent } from './archive-factor-list.component';



describe('ArchiveFactoryListComponent', () => {
  let component: ArchiveFactorListComponent;
  let fixture: ComponentFixture<ArchiveFactorListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ArchiveFactorListComponent]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ArchiveFactorListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

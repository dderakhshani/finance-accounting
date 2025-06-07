import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ExportToExcelTejaratSystemComponent } from './export-to-excel-tejarat-system.component';



describe('ExportToExcelTejaratSystemComponent', () => {
  let component: ExportToExcelTejaratSystemComponent;
  let fixture: ComponentFixture<ExportToExcelTejaratSystemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExportToExcelTejaratSystemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportToExcelTejaratSystemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

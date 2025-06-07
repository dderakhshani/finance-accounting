import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DocumentItemsBomDialog } from './document-Items-bom-dialog.component';


describe('DocumentItemsBomDialog', () => {
  let component: DocumentItemsBomDialog;
  let fixture: ComponentFixture<DocumentItemsBomDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DocumentItemsBomDialog ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DocumentItemsBomDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

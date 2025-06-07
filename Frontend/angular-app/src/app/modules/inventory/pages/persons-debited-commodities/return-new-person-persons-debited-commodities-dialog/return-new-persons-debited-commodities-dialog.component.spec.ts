import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UpdateNewPersonsDebitedDialogComponent } from './return-new-persons-debited-commodities-dialog.component';



describe('UpdateNewPersonsDebitedDialogComponent', () => {
  let component: UpdateNewPersonsDebitedDialogComponent;
  let fixture: ComponentFixture<UpdateNewPersonsDebitedDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UpdateNewPersonsDebitedDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateNewPersonsDebitedDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

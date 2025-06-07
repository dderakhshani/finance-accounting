import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RialsDebitDetailsDialogComponent } from './Rials-debit-details-dialog.component';

describe('RialsCostDetailsComponent', () => {
  let component: RialsDebitDetailsDialogComponent;
  let fixture: ComponentFixture<RialsDebitDetailsDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RialsDebitDetailsDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RialsDebitDetailsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BomDialogComponent } from './bom-dialog.component';

describe('BomDialogComponent', () => {
  let component: BomDialogComponent;
  let fixture: ComponentFixture<BomDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BomDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BomDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

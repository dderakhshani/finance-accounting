import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommodityCategoryPropertyDialogComponent } from './commodity-category-property-dialog.component';

describe('CommodityCategoryPropertyDialogComponent', () => {
  let component: CommodityCategoryPropertyDialogComponent;
  let fixture: ComponentFixture<CommodityCategoryPropertyDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommodityCategoryPropertyDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommodityCategoryPropertyDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

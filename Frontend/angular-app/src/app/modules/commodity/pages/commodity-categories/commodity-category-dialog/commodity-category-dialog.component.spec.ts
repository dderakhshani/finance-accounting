import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommodityCategoryDialogComponent } from './commodity-category-dialog.component';

describe('CommodityCategoryDialogComponent', () => {
  let component: CommodityCategoryDialogComponent;
  let fixture: ComponentFixture<CommodityCategoryDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommodityCategoryDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommodityCategoryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

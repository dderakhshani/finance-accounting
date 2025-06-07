import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommodityCategoriesComponent } from './commodity-categories.component';

describe('CommodityCategoriesComponent', () => {
  let component: CommodityCategoriesComponent;
  let fixture: ComponentFixture<CommodityCategoriesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommodityCategoriesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommodityCategoriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

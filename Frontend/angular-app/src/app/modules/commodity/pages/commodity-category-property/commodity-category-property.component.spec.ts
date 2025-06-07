import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommodityCategoryPropertyComponent } from './commodity-category-property.component';

describe('CommodityCategoryPropertyComponent', () => {
  let component: CommodityCategoryPropertyComponent;
  let fixture: ComponentFixture<CommodityCategoryPropertyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommodityCategoryPropertyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommodityCategoryPropertyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

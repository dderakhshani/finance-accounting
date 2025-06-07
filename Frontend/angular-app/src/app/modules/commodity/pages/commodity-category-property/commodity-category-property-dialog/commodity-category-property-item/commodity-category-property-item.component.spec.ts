import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommodityCategoryPropertyItemComponent } from './commodity-category-property-item.component';

describe('CommodityCategoryPropertyItemComponent', () => {
  let component: CommodityCategoryPropertyItemComponent;
  let fixture: ComponentFixture<CommodityCategoryPropertyItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommodityCategoryPropertyItemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommodityCategoryPropertyItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

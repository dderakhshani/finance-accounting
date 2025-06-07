import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableColumnFiltersComponent } from './table-column-filters.component';

describe('TableColumnFiltersComponent', () => {
  let component: TableColumnFiltersComponent;
  let fixture: ComponentFixture<TableColumnFiltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TableColumnFiltersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TableColumnFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

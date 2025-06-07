import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableColumnSortingComponent } from './table-column-sorting.component';

describe('TableColumnSortingComponent', () => {
  let component: TableColumnSortingComponent;
  let fixture: ComponentFixture<TableColumnSortingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TableColumnSortingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TableColumnSortingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {MatMenuTrigger} from "@angular/material/menu";
import {TableColumnFilterTypes} from "../../models/table-column-filter-types";

@Component({
  selector: 'app-table-column-filters',
  templateUrl: './table-column-filters.component.html',
  styleUrls: ['./table-column-filters.component.scss']
})
export class TableColumnFiltersComponent implements OnInit {
  @Input()  isFiltered!: boolean;
  @Input()  isHovered!: boolean;
  @Input()  title!: string;
  @Input()  filters!: any[];
  @Input()  columnFilterOperands!: any[];
  @Input()  columnCurrentFilterIndex!: number;
  @Input()  columnFilter!: any;

  @Output() updateFiltersEvent: EventEmitter<MatMenuTrigger> = new EventEmitter<MatMenuTrigger>();
  constructor() { }

  ngOnInit(): void {
  }

  protected readonly tableColumnFilterTypes = TableColumnFilterTypes;

  updateFilters(menuTrigger?: MatMenuTrigger) {

    this.updateFiltersEvent.emit(menuTrigger);
  }

  deleteFilter(columnFilter : any) {
    if (columnFilter && typeof columnFilter.setDefaultFilter === 'function') {
      columnFilter.setDefaultFilter();
    } else {
      console.warn('filter is not defined or setDefaultFilter is not a function for column:', columnFilter);
    }

    this.updateFilters()
  }

  addFilter(columnFilter: any) {
    if(columnFilter && typeof columnFilter.addFilter === 'function'){

    columnFilter.addFilter()

    }else {
      console.warn('filter is not defined or addFilter is not a function for column:', columnFilter);

    }

  }

  removeFilter(columnFilter: any, filterIndex: number) {
    if (columnFilter && typeof columnFilter.removeFilter === 'function') {
      columnFilter.removeFilter(filterIndex)
    }else {

      console.warn('filter is not defined or removeFilter is not a function for column:', columnFilter , 'filterIndex:', filterIndex);
    }
  }
}

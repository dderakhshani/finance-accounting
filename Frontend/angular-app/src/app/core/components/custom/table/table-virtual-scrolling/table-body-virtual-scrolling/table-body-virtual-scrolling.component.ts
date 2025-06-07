import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {TableColumnDataType} from "../../models/table-column-data-type";
import {TableScrollingConfigurations} from "../../models/table-scrolling-configurations";
import {Column, viewMode} from "../../models/column";
import {BehaviorSubject} from "rxjs";

@Component({
  selector: 'app-table-body-virtual-scrolling',
  templateUrl: './table-body-virtual-scrolling.component.html',
  styleUrls: ['./table-body-virtual-scrolling.component.scss']
})
export class TableBodyVirtualScrollingComponent implements OnInit {
  @Input() tableConfigurations!: TableScrollingConfigurations;
  @Input() tableRows: any[] = [];
  @Input() virtualItems: any[] = [];
  @Input() showColumns!: Column[];
  @Input() dataStream$: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([]);
  @Input() dataSlice: any[] = [];
  @Input() cdkVirtual: boolean = false;
  @Input() viewMode: viewMode = viewMode.default;
  @Output() rowClickedEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() rowDoubleClickedEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() selectItemEvent: EventEmitter<any> = new EventEmitter<any>();
  expandedRowIndex: number | null = null;
  expandedRowGroupIndex: { [key: string]: boolean } = {};

  constructor() {
  }

  ngOnInit(): void {
  }

  protected readonly fieldTypes = TableColumnDataType;

  trackByFn(index: number, item: any) {

    return item ? item.id || item : index;
  }

  onRowClick(row: any) {
    this.rowClickedEvent.emit(row);
  }

  onRowDoubleClick(row: any) {
    this.rowDoubleClickedEvent.emit(row);
  }

  selectItem(row: any) {
    this.selectItemEvent.emit(row);
  }

  async toggleTemplate(index: number) {

    this.expandedRowIndex = this.expandedRowIndex === index ? null : index;
  }

  toggleExpandedGroup(key: string) {
    if (!this.expandedRowGroupIndex) {
      this.expandedRowGroupIndex = {};
    }


    this.expandedRowGroupIndex[key] = !this.expandedRowGroupIndex[key];
  }


  gett(group: any) {
    console.log(!group.items[0]?.groupKey);
    return !group.items[0]?.groupKey;
  }

  getfas(group: any) {
    console.log(!!group.items[0]?.groupKey);
    return !!group.items[0]?.groupKey;
  }

  gettt(groupedData: any) {
    console.log(groupedData);
    return "";
  }
}

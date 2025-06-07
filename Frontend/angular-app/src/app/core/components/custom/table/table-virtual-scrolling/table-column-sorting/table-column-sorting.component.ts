import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-table-column-sorting',
  templateUrl: './table-column-sorting.component.html',
  styleUrls: ['./table-column-sorting.component.scss']
})
export class TableColumnSortingComponent implements OnInit {
  @Input() columnCurrentSortIndex!:  number ;
  @Input() isSorted!: boolean;
  @Input() isHovered!: boolean;
  @Input()  columnCurrentSortDirection!: string;
  @Output() handleSortEvent: EventEmitter<any> = new EventEmitter<any>();
  constructor() { }

  ngOnInit(): void {
  }


  handleSort() {
    this.handleSortEvent.emit();

  }
}

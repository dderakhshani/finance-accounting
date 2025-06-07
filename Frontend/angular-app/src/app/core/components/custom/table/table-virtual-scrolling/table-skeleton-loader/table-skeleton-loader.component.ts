import {Component, Input, OnInit} from '@angular/core';
import {Column} from "../../models/column";

@Component({
  selector: 'app-table-skeleton-loader',
  templateUrl: './table-skeleton-loader.component.html',
  styleUrls: ['./table-skeleton-loader.component.scss']
})
export class TableSkeletonLoaderComponent implements OnInit {
 @Input() height :number =40;
 @Input() showColumns : Column[] = [];

  skeletonColumns = Array(35).fill(0);

  constructor() { }

  ngOnInit(): void {
  }


}

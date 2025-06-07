import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Node} from "../list-control/models/node";
import {ControllerAction} from "../list-control/models/controller-action";
import {ListControl} from "../list-control/list-control";
import {ListControlActionTypes} from "../list-control/models/list-control-action-types";


@Component({
  selector: 'app-tree-item',
  templateUrl: './tree-item.component.html',
  styleUrls: ['./tree-item.component.scss']
})
export class TreeItemComponent implements OnInit {

  constructor() {
  }

  ngOnInit(): void {

  }




  @Input() useNew:boolean = false;
  @Input() node!:any;

  @Input() idKey:string = 'id';
  @Input() parentKey:string = 'parentId';
  @Input() childrenKey:string = 'children';
  @Input() levelCodeKey:string = 'level';
  @Input() titleKey:string = 'title'
  @Input() codeKey:string = 'code'
  @Input() isSearching:boolean = false;
  @Input() currentLevel : number = 0;
  @Input() restrictedLevel!: number;
  @Input() selectable:boolean = false;
  @Input() editable:boolean = false;
  @Input() canAdd:boolean = false;

  @Output() onClick:EventEmitter<any> = new EventEmitter<any>()
  @Output() onEdit:EventEmitter<any> = new EventEmitter<any>()
  @Output() onAdd:EventEmitter<any> = new EventEmitter<any>()
  @Output() onSelect:EventEmitter<any> = new EventEmitter<any>()

}

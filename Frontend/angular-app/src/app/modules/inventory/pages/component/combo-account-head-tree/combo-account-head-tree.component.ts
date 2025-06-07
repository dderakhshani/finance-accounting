import { style } from '@angular/animations';
import { FlatTreeControl } from '@angular/cdk/tree';
import { ElementRef, EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { MatMenuTrigger } from '@angular/material/menu';
import { FormControl } from '@angular/forms';


interface FlatNode {
  expandable: boolean;
  title: string;
  level: number;
  id: number;
  parentId: number
}

export class source {
  public id!: any;
  public parentId!: number;
  public children: source[] = [];
  public title!: string;
  public hasChild: boolean = false;
  public expandable: boolean = false;
}

@Component({
  selector: 'app-combo-account-head-tree',
  templateUrl: './combo-account-head-tree.component.html',
  styleUrls: ['./combo-account-head-tree.component.scss']
})
export class ComboAccountHeadTreeComponent implements OnChanges {
  @ViewChild('input')
  input!: ElementRef<HTMLInputElement>;
  title = new FormControl('');
  
  public nodes: any[] = [];
  @Input() isDisable: boolean = false;
  @Input() DefaultId: any = undefined
  @Input() isRequired: boolean = false;
  @Output() SelectId = new EventEmitter<any>();
  @Input() lablelTitleCombo: string = "";
  @Input() isLastLevel: boolean = false;
  @Input() tabindex: number = 1;

  nodes_filter: any[] = [];

  constructor(private _mediator: Mediator, private Service: PagesCommonService, private api: ApiCallService) {

  }
  async ngOnChanges() {


    //--------get data flat and convert to tree by component
    await this.initialize();
    if(this.isDisable)
      this.title.disable()

  }

  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
   
  }



  onSelectNode(id: any) {

    var item = this.nodes.find(a => a.id == id);
    this.SelectId.emit(item);
    this.title.setValue(item?.title);

    this.DefaultId = id;


    if (this.title.value == undefined) {
      this.title.setValue(undefined);
      this.DefaultId = undefined
    }

  }
  
  onSearchTerm() {

    const filterValue = this.input.nativeElement.value.toLowerCase();

    this.nodes = this.nodes_filter.filter(o => o.title.toLowerCase().includes(filterValue));

  }

  onSelectionChange(event: any) {

    this.onSelectNode(event.option.value);
  }

  async initialize() {

    await this.api.getAccountHead(false).then(res => {
      this.nodes = this.api.AccountHead;
      this.nodes_filter = this.api.AccountHead;
      //-----------------------نمایش مقدار اولیه----------------------
      if (this.nodes.length > 0 && this.DefaultId != undefined) {

        var item = this.nodes.find(a => a.id == Number(this.DefaultId));

        var value = item?.title;
        this.title.setValue(value);

       
      }
      if (this.DefaultId == undefined) {
        this.title.setValue(undefined);
      }

    })


  }
}




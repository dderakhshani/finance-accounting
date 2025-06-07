import { Component, Input, ViewChild } from '@angular/core';
import { FlatTreeControl } from '@angular/cdk/tree';
import { EventEmitter, OnChanges, Output } from '@angular/core';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { GetCommodityCategoriesALLQuery } from '../../../repositories/commodity-categories/get-commodity-categories-all-query';
import { MatMenuTrigger } from '@angular/material/menu';

interface FlatNode {
  expandable: boolean;
  name: string;
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
  selector: 'app-combo-commodity-categories-tree',
  templateUrl: './combo-commodity-categories-tree.component.html',
  styleUrls: ['./combo-commodity-categories-tree.component.scss']
})
export class CommodityCategoriesTreeComponent implements OnChanges {
  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger | undefined;
  openMenu() {
    this.trigger?.openMenu();
  }


  public nestedNodes: source[] = [];
  public nestedNodes_Filter: source[] = [];
  public nodes: any[] = [];
  @Input() isDisable: boolean = false;
  @Input() DefaultId: any = undefined
  @Input() isRequired: boolean = false;
  @Output() SelectId = new EventEmitter<any>();
  @Input() lablelTitleCombo: string = "";
  @Input() isLastLevel: boolean = false;
  @Input() tabindex: number = 1;

  title: string = "";
  searchTerm: string = "";
  nodes_filter: any[] = [];
  constructor(
    private _mediator: Mediator,
    private Service: PagesCommonService,
    private ApiCallService: ApiCallService,
    public _notificationService: NotificationService,
   

  ) {

  }

  onSelectNode(id:any,title :any) {
   
    this.SelectId.emit(id);
    this.title = title;
    this.DefaultId = id;

    this.searchTerm = "";

    if (this.title == undefined) {
      this.title = '';
    }

  }

  async ngOnChanges() {

    //--------get data flat and convert to tree by component
    await this.initialize()
    if (!this.isLastLevel ) {
      var list = this.createNestedTree();
      this.dataSource.data = list;
    }
    //------get data tree and convert date by backend
    else if (this.isLastLevel) {
      this.dataSource.data = this.nodes;
    }

    

    //-----------------------نمایش مقدار اولیه----------------------
    if (this.nodes.length > 0 && this.title == "" && this.DefaultId != undefined) {

      var item = this.nodes.find(a => a.id == Number(this.DefaultId));

      var value = item?.title;
      this.title = value;



    }
    if (this.DefaultId == undefined) {
      this.title = '';
    }
    this.onfocus();
  }

  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
  }
  async initialize() {

    if (this.isLastLevel) {

      ///ToDo
    }
    else if (!this.isLastLevel) {
      await this.ApiCallService.GetCommodityCategoriesALL();
      this.nodes = this.ApiCallService.CommodityCategory;
      

      //await this._mediator.send(new GetCommodityCategoriesALLQuery()).then(async (res) => {
      //  this.nodes = res
      //})

    }
  }

  createNestedTree(): source[] {

    this.nestedNodes = [];

    this.nodes.filter(x => !x.parentId).forEach(flatNode => {
      this.nestedNodes.push(flatNode);
    });

    this.nestedNodes.forEach(node => {
      this.findChildren(node);
    });

    return this.nestedNodes;
  }
  findChildren(node: source) {
    node.hasChild = false;
    node.children = [];

    let children = this.getChildren(node);
    if (children.length > 0) {
      node.hasChild = true;
      children.forEach(childNode => {
        node.children.push(childNode);
        this.findChildren(childNode);
      });

    }

  }
  getChildren(node: source) {

    return this.nodes.filter(x => x.parentId === node.id);
  }
  onfocus() {

    window.setTimeout(function () {
      document.getElementById('searchTerm')?.focus();
    }, 0);
  }
  onSearchTerm() {
    if (this.searchTerm && this.isLastLevel) {

      this.nodes = [...this.nodes_filter.filter(x => x.title.toLowerCase().includes(this.searchTerm.toLowerCase()))]
    } else {


      this.nodes = [...this.nodes_filter]

    }
  }

  private _transformer = (node: source, level: number) => {
    return {
      expandable: !!node.children && node.children.length > 0,
      name: node.title,
      level: level,
      parentId: node.parentId,
      id: node.id
    };
  };

  treeControl = new FlatTreeControl<FlatNode>(
    node => node.level,
    node => node.expandable,

  );

  treeFlattener = new MatTreeFlattener(
    this._transformer,
    node => node.level,
    node => node.expandable,
    node => node.children,
  );

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);
  hasChild = (_: number, node: FlatNode) => node.expandable;

}




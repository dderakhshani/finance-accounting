import { style } from '@angular/animations';
import { FlatTreeControl } from '@angular/cdk/tree';
import { EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';

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
  selector: 'app-combo-tree',
  templateUrl: './combo-tree.component.html',
  styleUrls: ['./combo-tree.component.scss']
})
export class ComboTreeComponent implements OnChanges {
  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger | undefined;
  openMenu() {
    this.trigger?.openMenu();
  }

  public nestedNodes: source[] = [];
  public nestedNodes_Filter: source[] = [];
  @Input()  nodes: any[] = [];
  @Input()  isTree: boolean = true;
  @Input()  isFlatSource: boolean = true;
  @Input()  isDisable: boolean = false;
  @Input()  DefaultId: any = undefined
  @Input()  lablelTitleCombo: string = "";
  @Input()  isRequired: boolean = false;
  @Output() SelectId = new EventEmitter<any>();
  @Output() SelectTitle = new EventEmitter<any>();
  @Input() tabindex: number = 1;
  title:        string = "";
  searchTerm:   string = "";
  parentNames:  string[] = []
  nodes_filter: any[] = [];

  onSelectNode(id: any ,title :string) {

    this.SelectId.emit(id);
    this.SelectTitle.emit(title);
    this.title = title;
    this.DefaultId = id;
    this.parentNames = [];
    this.parentNames.push(this.title);

    if (this.title == undefined) {
      this.title = '';
    }

  }
  constructor() {

  }
  ngOnChanges(): void {

    //--------get data flat and convert to tree by component
    if (this.isTree && this.isFlatSource) {
      var list = this.createNestedTree();
      this.dataSource.data = list;
    }
    //------get data tree and convert date by backend
    else if (this.isTree && !this.isFlatSource) {
      this.dataSource.data = this.nodes;
    }
    //----------get data flat and view flat
    else if (!this.isTree && this.nodes.length > 0) {
      this.nodes_filter = this.nodes;
    }
    //-----------------------نمایش مقدار اولیه----------------------
    if (this.nodes.length > 0 && this.title == "" && this.DefaultId != undefined) {
      var value = this.nodes.find(a => a.id == Number(this.DefaultId))?.title;
      this.title = value;


    }
    if (this.DefaultId == undefined) {
      this.title = '';
    }
  }

  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
  }
  async initialize() {

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

  onSearchTerm() {


     this.nestedNodes_Filter;
    var list: any = document.getElementsByClassName("btn-tree-node");
    var $this: any = this;
    Array.prototype.forEach.call(list, function (el) {

      if (el.textContent.includes($this.searchTerm) && ($this.searchTerm != "" && $this.searchTerm != undefined)) {
        el.style.backgroundColor = "#f9ff00";
      }
      else {
        el.style.backgroundColor = null;
      }
    });

    if (this.searchTerm && !this.isTree) {

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




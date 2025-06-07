import {FlatTreeControl} from '@angular/cdk/tree';
import { EventEmitter, OnChanges, Output } from '@angular/core';
import {Component, Input} from '@angular/core';
import {MatTreeFlatDataSource, MatTreeFlattener} from '@angular/material/tree';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { WarehouseLayoutTree } from '../../../entities/warehouse-layout';


/** Flat node with expandable and level information */
interface WarehouseLayouFlatNode {
  expandable: boolean;
  name: string;
  level: number;
  id:number;
  parentId:number
}


 @Component({
  selector: 'app-layout-list',
  templateUrl: './layout-list.component.html',
  styleUrls: ['./layout-list.component.scss']
})
export class LayoutListComponent implements OnChanges {

  _nodes: WarehouseLayoutTree[] = [];
  nodesFilter: WarehouseLayoutTree[] = [];
  searchTerm:string="";
  @Input() nodes:WarehouseLayoutTree[]=[];
  @Output() parentId = new EventEmitter<number>();


  onSelectNode(id:number) {
    this.parentId.emit(id);

  }
    
   constructor(public _notificationService: NotificationService) {

  }
   ngOnChanges(): void {
     this.dataSource.data = this.nodes;
     this.nodesFilter = this.nodes


   }
  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
  }
  async initialize() {

  }
  private _transformer = (node: WarehouseLayoutTree, level: number) => {
    return {
      expandable: !!node.children && node.children.length > 0,
      name: node.title,
      level: level,
      parentId:node.parentId,
      id:node.id
    };
  };

  treeControl = new FlatTreeControl<WarehouseLayouFlatNode>(
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
 hasChild = (_: number, node: WarehouseLayouFlatNode) => node.expandable;


 onSearchTerm() {

  var list :any= document.getElementsByClassName("btn-tree-node-list");

   var $this:any=this;
    var i:number=0
    var x,y:number=0;
   Array.prototype.forEach.call(list, function(el) {

       if(el.textContent.includes($this.searchTerm) && ($this.searchTerm!="" && $this.searchTerm!=undefined))
       {
         el.style.backgroundColor = "#f9ff00";
         if(i==0)
        {
          x = el.offsetTop;
          y = el.offsetLeft;

          window.scroll({
            top: x,
            behavior: 'smooth'
          });
        }
       }
       else{
         el.style.backgroundColor =null;
       }
       i=i+1;

   });
   //this.dataSource.data = this.nodes;
   //this.nodesFilter.forEach(a => {
   //  var ss = this.haschild(a);
    
     
   //})
   
  
 }
   haschild(WarehouseLayoutTree: any) {
     
     if (WarehouseLayoutTree?.children?.length > 0) {
       WarehouseLayoutTree?.children?.forEach((a: any) => {

         if (a.title.includes(this.searchTerm))
           this.dataSource.data.push(a);
         
           this.haschild(a)
         
         
       }
       )
     }
     else {
       if (WarehouseLayoutTree.title.includes(this.searchTerm))
         return WarehouseLayoutTree
       else
         return null
     }
   }
   

}


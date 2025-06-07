import { style } from '@angular/animations';

import { EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatMenuTrigger } from '@angular/material/menu';



export class source {
  public id!: any;
  public parentId!: any;
  public selected: boolean = false;
  public title!: string;
 
}


@Component({
  selector: 'app-combo-search-multiper-items',
  templateUrl: './combo-search-multiper-items.component.html',
  styleUrls: ['./combo-search-multiper-items.component.scss']
})
export class ComboSearchMultiperItemsComponent implements OnChanges {

  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger | undefined;
  openMenu() {
    this.trigger?.openMenu();
  }


  @Input() nodes_filter: any[] = [];
  @Input() nodes: any[] = [];
  @Input() isDisable: boolean = false;
  @Input() lablelTitleCombo: string = "";
  @Input() isRequired: boolean = false;
  @Input() listItems: any[] = [];
  @Output() SelectedList = new EventEmitter<any[]>();
  @Output() DeleteItem = new EventEmitter<any>();
  @Output() SearchTerm = new EventEmitter<string>();
  @Input() tabindex: number = 1;
  @Input() viewDetalis: boolean = true;
  searchTerm: string = "";
  nodesSelected: any[] = []
  sources: any[] = [];
  sources_filter: source[] = [];
  selectedAll: boolean = false;
  open: boolean = false;

  onSelectNode(item: any, ob: MatCheckboxChange) {
    //اگر وجود نداشت اضافه شود
    if (this.nodesSelected.find(a => a.id == item.id) == undefined && ob.checked) {
      this.nodesSelected.push(item);
      this.SelectedList.emit(this.nodesSelected);
    }
    //اگر وجود داشت حذف شود
    else {
      this.onDeleteItem(item.id)
      this.selectedAll = false;
    }
    this.searchTerm = "";
  }
  constructor() {

  }
  ngOnChanges(): void {

    this.nodesSelected = this.listItems;
    this.sources=[]
      this.nodes.forEach(req => {
        let selected = this.listItems.filter(a => a.id == req.id).length > 0 ? true : false;
        this.sources.push(
          {
            id: req.id,
            selected: selected,
            title: req.title,
            parentId: req.parentId,
            accountHeadId: req?.accountHeadId
          }
        )
      });
      this.sources_filter = this.sources;
        

    //if (this.sources.filter(a => a.parentId != undefined).length > 0) {
    //  this.sources.sort((x, y) => x.parentId > y.parentId ? 1 : -1)
    //}
    //else {
    //  this.sources.sort((x, y) => x.title > y.title ? 1 : -1)
    //}
   
   
    
  }

  async ngOnInit() {

    await this.resolve();
  }

  async resolve() {

    await this.initialize()
  }
  async initialize() {

  }
  async onDeleteItem(id: number) {
    var sorce = this.sources.find(a => a.id == id)
    if (sorce != undefined)
      sorce.selected = false;
    this.nodesSelected = this.nodesSelected.filter(a => a.id != id);
    
    this.SelectedList.emit(this.nodesSelected);
    this.DeleteItem.emit(sorce);
  }
  
  onSelectAll(ob: MatCheckboxChange) {
    
    this.sources.forEach(a => {
      a.selected = ob.checked;
      this.onSelectNode(a, ob);
    })
    this.selectedAll = ob.checked;
   
  }
  onSearchTerm() {

   
      if (this.searchTerm) {
      
        this.sources = [...this.sources_filter.filter(x => x.title.toLowerCase().includes(this.searchTerm.toLowerCase()))]
      } else {
        this.sources = [...this.sources_filter]
    }

    
  }
  onfocus() {

    setTimeout(() => {
      document.getElementById('searchTerm')?.focus();
    }, 1);

  }
}




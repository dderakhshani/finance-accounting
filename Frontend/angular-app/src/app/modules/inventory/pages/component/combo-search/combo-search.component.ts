import { style } from '@angular/animations';

import { EventEmitter, OnChanges, Output, ViewChild } from '@angular/core';
import { Component, Input } from '@angular/core';
import { MatMenuTrigger } from '@angular/material/menu';




@Component({
  selector: 'app-combo-search',
  templateUrl: './combo-search.component.html',
  styleUrls: ['./combo-search.component.scss']
})
export class ComboSearchComponent implements OnChanges {
  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger | undefined;
  openMenu() {
    this.trigger?.openMenu();
  }

  @Input() nodes: any[] = [];
  @Input() isDisable: boolean = false;
  
  @Input() DefaultId: any = undefined
  @Input() lablelTitleCombo: string = "";
  @Input() isRequired: boolean = false;
  @Output() SelectId = new EventEmitter<any>();
  @Output() SelectItem = new EventEmitter<any>();
  @Input() isInternalSearch: boolean = false;
  @Output() SearchTerm = new EventEmitter<string>();
  @Input() tabindex: number = 1;

  title: string = "";
  searchTerm: string = "";
  nodes_filter: any[] = [];

  onSelectNode(id: any) {

    let node = this.nodes.find(a => a.id == id);
    this.SelectId.emit(id);
    this.SelectItem.emit(node);
   
    this.DefaultId = id;
    this.title = node?.title;

    if (this.title == undefined) {
      this.title = '';
    }
    this.searchTerm = "";
    this.onfocus();

  }
  constructor() {

  }
  ngOnChanges(): void {

    if (this.nodes.length > 0) {
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


  onSearchTerm() {

    
    if (!this.isInternalSearch) {
      if (this.searchTerm) {

        this.nodes = [...this.nodes_filter.filter(x => x.title.toLowerCase().includes(this.searchTerm.toLowerCase()))]
      } else {
        this.nodes = [...this.nodes_filter]
      }
    }
    else {
      this.SearchTerm.emit(this.searchTerm);
    }
  }

  onfocus() {

    setTimeout(() => {
      document.getElementById('searchTerm')?.focus();
    }, 1);

  }
}




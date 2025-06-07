import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {ListControl} from "./list-control/list-control";
import {Node} from "./list-control/models/node";
import {ControllerAction} from "./list-control/models/controller-action";
import {ListControlActionTypes} from "./list-control/models/list-control-action-types";
import {NodeBranch} from "./list-control/models/node-branch";
import {FormControl} from "@angular/forms";
import {
  TableConfigurations
} from "../table/models/table-configurations";
import {TableColumnDataType} from "../table/models/table-column-data-type";
import {TableOptions} from "../table/models/table-options";
import {TableColumn} from "../table/models/table-column";
import {ToolBar} from "../table/models/tool-bar";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import {TableScrollingConfigurations} from "../table/models/table-scrolling-configurations";
import {FontFamilies} from "../table/models/font-families";
import {FontWeights} from "../table/models/font-weights";
import {Column, TypeFilterOptions} from "../table/models/column";
import {DecimalFormat} from "../table/models/decimal-format";
import {TableColumnFilter} from "../table/models/table-column-filter";
import {TableColumnFilterTypes} from "../table/models/table-column-filter-types";
import {TablePaginationOptions} from "../table/models/table-pagination-options";
import {PrintOptions} from "../table/models/print_options";

@Component({
  selector: 'app-tree',
  templateUrl: './tree.component.html',
  styleUrls: ['./tree.component.scss']
})
export class TreeComponent implements OnInit ,OnChanges{
  // Old needs to be deleted
  @Input() useNew: boolean = false;
  //


  // main data which is passed in
  _rawNodes: any[] = []
  _searchedRawNodes: any[] = [];

  @Input() set rawNodes(nodes: any[]) {
    if (this.isSearching) {
      this._searchedRawNodes = nodes;
    } else {
      this._rawNodes = nodes;
    }
    this.generateNodes();
  };


  get rawNodes() {
    if (this.isSearching) {
      return this._searchedRawNodes
    } else {
      return this._rawNodes;
    }
  }


  // tree data which is generated
  _nestedNodes: any[] = []
  // grid data which is generated
  _gridNodes: any[] = []
  rootNodes: any[] = [
    {
      id: 1,
      title: 'Node 1',
      rowIndex: 1,
      selected: false,

    }
  ]

  set nodes(nodes: any[]) {
    if (this.displayLayout === 'tree') {
      this._nestedNodes = nodes;
    }
    if (this.displayLayout === 'grid') {
      this._gridNodes = nodes;
    }
  }

  get nodes() {
    if (this.displayLayout === 'tree') {
      return this._nestedNodes
    }
    if (this.displayLayout === 'grid') {
      return this._gridNodes
    } else {
      return []
    }
  }


  _displayLayout: 'tree' | 'grid' = 'tree';
  @Input() set displayLayout(newLayout: 'tree' | 'grid') {
    this._displayLayout = newLayout;
    this.generateNodes()
  }

  get displayLayout() {
    return this._displayLayout
  }

  path: any[] = [];

  @Input() idKey: string = 'id';
  @Input() parentKey: string = 'parentId';
  @Input() childrenKey: string = 'children';
  @Input() levelCodeKey: string = 'levelCode';
  @Input() titleKey: string = 'title';
  @Input() codeKey: string = 'code';
  @Input() restrictedLevel!: number;


  @Input() height!: string;

  @Input() searchTitle: string = 'جستجو';
  @Input() searchableProperties: string[] = ['title']
  @Input() searchFu! : any

  searchFormControl = new FormControl('')
  @Input() minimumCharactersNeedToSearch: number = 3;
  isSearching: boolean = false;

  @Output() onClick: EventEmitter<any> = new EventEmitter<any>()
  @Output() onClickPath: EventEmitter<any> = new EventEmitter<any>()
  @Input() canAdd: boolean = false;
  @Output() onAdd: EventEmitter<any> = new EventEmitter<any>()
  @Input() selectable: boolean = false;
  @Output() onSelect: EventEmitter<any> = new EventEmitter<any>()
  @Input() editable: boolean = false;
  @Output() onEdit: EventEmitter<any> = new EventEmitter<any>()
  nodesBreadcrumb!: any[];

  toolBar: ToolBar = {
    showTools: {
      tableSettings: true,
      includeOnlySelectedItemsLocal: false,
      excludeSelectedItemsLocal: false,
      includeOnlySelectedItemsFilter: false,
      excludeSelectedItemsFilter: false,
      undoLocal: false,
      deleteLocal: false,
      restorePreviousFilter: true,
      refresh: false,
      exportExcel: false,
      fullScreen: true,
      printFile: true,
      removeAllFiltersAndSorts: true,
      showAll: true
    },
    isLargeSize: false
  }
  defaultColumnSettings = {
    class: '',
    style: {},
    display: true,
    sortable: true,
    filter: undefined,
    displayFunction: undefined,
    disabled: false,
    options: [],
    optionsValueKey: 'id',
    optionsTitleKey: [],
    filterOptionsFunction: undefined,
    filteredOptions: [],
    asyncOptions: undefined,
    showSum: false,
    sumColumnValue: 0,
    matTooltipDisabled: true,
    fontSize: 12,
    fontFamily: FontFamilies.IranSans,
    fontWeight: FontWeights.normal,
    isCurrencyField: false,
    isDisableDrop: false,
    typeFilterOptions: TypeFilterOptions.None,
    lineStyle: 'onlyShowFirstLine',
  };
  columns!: Column[]
  // GridLayout
  tableConfigurations !: TableScrollingConfigurations;

  constructor() {
  }

  ngOnInit(): void {
    this.resolver()
  }
  async ngOnChanges(changes: SimpleChanges) {
    if (changes['searchFu'] ) {
      setTimeout(()=>{
        this.handleSearchWithIdNodes(this.searchFu  ,false)
      })
    }
  }
  resolver() {
    this.columns = [
      {
        ...this.defaultColumnSettings,
        index: 0,
        field: 'selected',
        title: '#',
        width: 1,
        type: TableColumnDataType.Select,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
        digitsInfo: DecimalFormat.Default,
      },

      {
        ...this.defaultColumnSettings,
        index: 1,
        field: 'rowIndex',
        title: 'ردیف',
        width: 1,
        type: TableColumnDataType.Index,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
      },

      {
        ...this.defaultColumnSettings,
        index: 2,
        field: this.titleKey,
        title: 'عنوان',
        width: 18,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter(this.titleKey, TableColumnFilterTypes.Text),
        sumColumnValue: 0,
        lineStyle: 'onlyShowFirstLine',
      },
    ]

    this.tableConfigurations = new TableScrollingConfigurations(
      this.columns, new TableOptions(), new TablePaginationOptions(), this.toolBar, new PrintOptions('گزارش '));
    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.showTopSettingMenu = true;

  }

  // Tree Layout
  // generateNodes() {
  //
  //   if (this.displayLayout === 'tree') {
  //     this.nodes = this.rawNodes.filter(x => !x[this.parentKey]);
  //     this.nodes.forEach(node => {
  //       this.setNodeChildren(node);
  //     });
  //   }
  //   if (this.displayLayout === 'grid') {
  //     this.nodes = !this.isSearching ? this.rawNodes.filter(x => !x[this.parentKey]) : this.rawNodes;
  //   }
  // }
  generateNodes() {
    // تعیین منبع داده بر اساس حالت جستجو
    const nodesToDisplay = this.isSearching ?
      (this._searchedRawNodes || []) :
      (this.rawNodes || []);

    if (this.displayLayout === 'tree') {
      // فیلتر گرههای ریشه برای ساختار درختی
      this.nodes = nodesToDisplay.filter(x => !x[this.parentKey]);

      // تنظیم فرزندان برای هر گره
      this.nodes.forEach(node => this.setNodeChildren(node));
    }

    if (this.displayLayout === 'grid') {
      // در حالت گرید فقط در حالت عادی گرههای ریشه را نشان میدهیم
      this.nodes = this.isSearching ?
        nodesToDisplay : // در حالت جستجو همه نتایج را نشان دهید
        nodesToDisplay.filter(x => !x[this.parentKey]); // در حالت عادی فقط ریشهها
    }
  }

  setNodeChildren(node: any) {

    node[this.childrenKey] = [];
    if (node._expanded) {
      let children = this.getChildren(node, this.rawNodes);
      children.forEach((childNode: any) => {
        node[this.childrenKey].push(childNode);
        this.setNodeChildren(childNode);
      });
    }
  }
  handleSearchWithIdNodes(searchTerm = this.searchFu, isSearching: boolean = false) {
    // Reset searched nodes when no search term
    if (!searchTerm || searchTerm.length < this.minimumCharactersNeedToSearch) {
      this._searchedRawNodes = [];
      this.isSearching = false;
      this.generateNodes();
      return;
    }

    this.isSearching = false;
    this.collapseAll();

    let searchedNodes = [...this.rawNodes].filter(item =>
       {
        let value = item[this.idKey];

        if (!isNaN(Number(value))) {
          return value.toString().includes(searchTerm);
        }

        return value?.toString().toLowerCase().includes(searchTerm?.toLowerCase());
      }
    );

    // Add parent nodes
    if (searchedNodes.length > 0) {
      searchedNodes.filter(x =>
        x[this.parentKey] &&
        !searchedNodes.find(y => y[this.idKey] === x[this.parentKey])
      ).forEach((node: any) => {
        let parent = this.getParent(node, this.rawNodes);
        while (parent) {
          if (!searchedNodes.find(x => x[this.idKey] === parent[this.idKey])) {
            searchedNodes.push(parent);
          }
          parent = this.getParent(parent, this.rawNodes);
        }
      });
    }

    this.isSearching = isSearching;
    this._searchedRawNodes = [...searchedNodes]; // Store search results separately
    this.expandArray(this._searchedRawNodes);
    this.generateNodes();
  }


  handleSearchNodes(searchTerm = this.searchFormControl.value ,isSearching:boolean=true ) {

    // reset
    this._searchedRawNodes = []
    this.isSearching = false;
    this.collapseAll()

    if (this.searchFormControl.value?.length >= this.minimumCharactersNeedToSearch) {
      let searchedNodes = this.rawNodes.filter(item => {
        return this.searchableProperties.some(field => item[field]?.toLowerCase()?.includes(searchTerm?.toLowerCase()))
      })

      if (searchedNodes.length > 0) {
        searchedNodes.filter(x => x[this.parentKey] && !searchedNodes.find(y => y[this.idKey] === x[this.parentKey])).forEach((node: any) => {
          let parent = this.getParent(node, this.rawNodes)
          while (parent) {
            if (!searchedNodes.find(x => x[this.idKey] === parent[this.idKey])) {
              searchedNodes.push(parent)
            }
            parent = this.getParent(parent, this.rawNodes)
          }
        })
      }
      this.isSearching = isSearching;
      this.rawNodes = searchedNodes;
      this.expandArray(this.rawNodes)
    }

    this.generateNodes()

  }

  // General Functions
  onNodeClick(node: any) {

    this.onClick.emit(node)

    this.nodesBreadcrumb = [node];
    if (this.searchFormControl.value?.length >= this.minimumCharactersNeedToSearch) {
      this.searchFormControl.setValue('');
      this.collapseAll();

      let nodeToToggle = this.rawNodes.find(x => x[this.idKey] === node[this.idKey]);
      while (nodeToToggle) {
        this.expand(nodeToToggle);
        nodeToToggle = this.getParent(nodeToToggle, this.rawNodes)
      }
      this.generateNodes()

    } else {
      this.toggle(node)
      this.rawNodes = [...this.rawNodes]
    }
  }

  onNodeEdit(node: any) {
    this.onEdit.emit(node)
  }

  onNodeAdd(node: any) {
    this.onAdd.emit(node)
  }

  onNodeSelect(node: any) {
    setTimeout(() => {
      this.onSelect.emit(node)
    }, 1)
  }


  toggle(node: any) {
    let expandedSibling = this.getSiblings(node).find(x => x._expanded);
    if (expandedSibling) {
      this.collapse(expandedSibling);
    }
    node._expanded ? this.collapse(node) : this.expand(node);
  }

  expand(node: any) {
    node._expanded = true;
    return node;
  }

  expandArray(nodes: any[]) {
    nodes.map(x => {
      x._expanded = true;
      return x
    })
  }

  collapse(node: any) {
    node._expanded = false;
    this.collapseDescendants(node);
  }

  collapseDescendants(node: any) {
    this.getDescendants(node).filter(x => x._expanded).forEach(descendantNode => {
      this.collapse(descendantNode);
    })
  }

  collapseAll() {
    this.rawNodes = this.rawNodes.map(node => {
      node._expanded = false;
      return node;
    });
  }

  select(node: any) {
    node._selected = true;
  }

  selectAll() {
    this.rawNodes.forEach(node => {
      node._selected = true;
    })
  }

  selectAllDescendants(node: any) {
    this.getDescendants(node).forEach(descendantNode => {
      this.select(descendantNode);
    })
  }

  selectAllParents(node: any) {
    if (node.parentId) {
      let parent = this.getParent(node, this.rawNodes);
      if (parent) {
        parent._selected = true;
        if (parent.parentId) {
          this.selectAllParents(parent);
        }
      }
    }
  }

  deselect(node: any) {
    node._selected = false;
  }

  deselectAll() {
    this.rawNodes.forEach(node => {
      node._selected = false;
    })
  }

  deselectAllDescendants(node: any) {
    this.getDescendants(node).forEach(descendantNode => {
      this.deselect(descendantNode);
    })
  }

  // Tree Helper Methods
  getSelectedNodes() {
    return this.rawNodes.filter(x => x._selected);
  }

  getSiblings(node: any) {
    return this.rawNodes.filter(x => x[this.parentKey] === node[this.parentKey] && x != node);
  }

  getChildren(node: any, nodes: any[]) {
    return nodes.filter(x => x[this.parentKey] === node[this.idKey]);
  }

  getDescendants(node: any) {
    return this.rawNodes.filter(x => (x[this.levelCodeKey].toString()).startsWith(node[this.levelCodeKey].toString()));
  }

  getParent(node: any, nodes: any[]) {
    return nodes.find(x => x[this.idKey] === node[this.parentKey]);
  }

  isRestrictedLevel(): boolean {

    if (!this.nodes || this.nodes.length === 0 || !this.restrictedLevel) return false;

    const firstNodeLevel = this.getLevelFromCode(this.nodes[0].levelCode);
    return firstNodeLevel >= this.restrictedLevel;
  }

  getLevelFromCode(levelCode: string): number {
    return levelCode.length / 4;
  }


  handlePathEvent(event: any[]) {
    this.path = event;
    this.onClickPath.emit(event);
  }
}

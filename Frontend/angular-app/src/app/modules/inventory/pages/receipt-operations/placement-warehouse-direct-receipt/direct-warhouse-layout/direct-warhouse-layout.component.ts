import {Component, EventEmitter, Input, Output} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {WarehouseLayout, WarehouseLayoutTree} from "../../../../entities/warehouse-layout";
import {Warehouse} from '../../../../entities/warehouse';
import {
  GetWarehousesQuery,
} from '../../../../repositories/warehouse/queries/get-warehouses-query';
import {
  GetWarehouseLayoutTreesQuery
} from '../../../../repositories/warehouse-layout/queries/get-warehouse-layouts-tree-query';
import {
  GetParentIdAllChildByCapacityAvailabeQuery
} from '../../../../repositories/warehouse-layout/queries/get-warehouse-layouts-parentId-all-child-query';
import {addModelArgs, ReceiptItem} from '../../../../entities/receipt-item';
import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";

@Component({
  selector: 'app-direct-warhouse-layout',
  templateUrl: './direct-warhouse-layout.component.html',
  styleUrls: ['./direct-warhouse-layout.component.scss']
})

export class RecieptWarhouseLayoutComponent extends BaseComponent {

  WarehouseLayouts: WarehouseLayout[] = [];
  WarehouseLayouts_Filter: WarehouseLayout[] = [];
  WarehouseLayoutTree: WarehouseLayoutTree[] = [];

  RowWarehouseLayout: WarehouseLayout | undefined = undefined;

  Warehouses: Warehouse[] = [];
  viewType: string = "grid"

  showFiller = false;
  ParentId: number | undefined = undefined;
  isToggled: boolean = false;
  searchTerm: string = "";
  @Input() commodityId: number | undefined = undefined;
  @Input() warhoseId: number | undefined = undefined;
  @Input() receiptItem: ReceiptItem | undefined = undefined;
  @Input() warehouseLayoutId: number | undefined = undefined;
  @Input() isComplited: boolean | undefined = false;
  @Output() SelectWarhouseLayout = new EventEmitter<addModelArgs>();

  constructor(
    private _mediator: Mediator
  ) {
    super();

  }

  async ngOnInit() {
    await this.resolve();
  }

  ngOnChanges(): void {
   
    if (this.commodityId != undefined) {
      this.get();
      if (this.receiptItem != undefined) {
       
        
        if (this.warehouseLayoutId) {
          this.onShowDetails(this.warehouseLayoutId);
        }
        
      }

    }

  }

  onSelectWarhouseLayout(warhouseLayoutId: number, quantity: number) {

    var w_l = this.WarehouseLayouts.find(a => a.id == warhouseLayoutId);
    if (w_l != undefined) {
      let cc = w_l.capacityAvailable;
      w_l.capacityAvailable = Number(cc) - quantity;
    }

    this.SelectWarhouseLayout.emit({warhouseLayoutId: warhouseLayoutId, quantity: quantity});

  }


  async resolve() {


    await this._mediator.send(new GetWarehousesQuery()).then(res => {
      this.Warehouses = res.data;

    })
    await this.initialize()
  }


  async get() {
    var searchQueries = [new SearchQuery(
      {
        propertyName: 'warehouseId',
        comparison: 'equal',
        values: [this.warhoseId],
      }
    )]

    this.WarehouseLayoutTree = [];
    this.WarehouseLayouts = [];
    
    await this._mediator.send(new GetWarehouseLayoutTreesQuery(0, 0, searchQueries, "id ASC")).then(res => {

      this.WarehouseLayoutTree = res.data;

    })

  }

  async onShowDetails(id: number) {


    if (id != undefined) {
      this.ParentId = id;


      await this._mediator.send(new GetParentIdAllChildByCapacityAvailabeQuery(id)).then(res => {

        this.WarehouseLayouts = [];


        this.WarehouseLayouts = res.data;
        this.WarehouseLayouts_Filter = res.data;


        this.WarehouseLayouts.forEach(a => a.capacityNeed = (Number(a.capacityAvailable) <= Number(this.receiptItem?.quantityChose) ? a.capacityAvailable : Number(this.receiptItem?.quantityChose))
          
        );
        this.WarehouseLayouts_Filter.forEach(a => a.capacityNeed = (Number(a.capacityAvailable) <= Number(this.receiptItem?.quantityChose) ? a.capacityAvailable : Number(this.receiptItem?.quantityChose)));

      })
    }


  }

  WarehouseIdSelect(id: number) {

    this.form.setValue(id);
    this.get();
  }


  add() {


  }


  //----------------------------------ویرایش------------------------------
  onClickDetaislCard(WarehouseLayout: WarehouseLayout) {


    if (WarehouseLayout.lastLevel == false) {
      this.update(WarehouseLayout);
    } else {
      this.onShowDetails(Number(WarehouseLayout.id))
      this.RowWarehouseLayout = WarehouseLayout;
    }
  }


  onSearchTerm() {

    if (this.searchTerm) {
      this.WarehouseLayouts = [...this.WarehouseLayouts_Filter.filter(x => x.title.toLowerCase().includes(this.searchTerm.toLowerCase()))]
    } else {
      this.WarehouseLayouts = [...this.WarehouseLayouts_Filter]
    }
  }

  async update(WarehouseLayout: any) {

  }

  delete() {

  }

  async initialize() {

  }


  close() {
  }
}



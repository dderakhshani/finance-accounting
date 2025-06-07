import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import { BaseComponent } from '../../../../../../../core/abstraction/base.component';
import { PageModes } from '../../../../../../../core/enums/page-modes';
import { Mediator } from "../../../../../../../core/services/mediator/mediator.service";
import { SearchQuery } from '../../../../../../../shared/services/search/models/search-query';
import { RequestItemCommodity } from '../../../../../entities/request-commodity-warehouse';
import { Warehouse } from '../../../../../entities/warehouse';
import { WarehouseLayout, WarehouseLayoutTree } from '../../../../../entities/warehouse-layout';
import { WarehouseLayoutsAddInventoryCommand } from '../../../../../repositories/warehouse-layout/commands/warehouse-layouts-add-inventory-command';
import { GetSuggestionWarehouseLayoutByCommodityCategoriesQuery } from '../../../../../repositories/warehouse-layout/queries/get-warehouse-layouts-suggestion-commodity-category-query';
import { GetWarehouseLayoutTreesQuery } from '../../../../../repositories/warehouse-layout/queries/get-warehouse-layouts-tree-query';

@Component({
  selector: 'app-warehouse-layouts-add-inventory-dialog',
  templateUrl: './warehouse-layouts-add-inventory-dialog.component.html',
  styleUrls: ['./warehouse-layouts-add-inventory-dialog.component.scss']
})
export class WarehouseLayoutsAddInventoryDialogComponent extends BaseComponent {


  pageModes = PageModes;
  requestItemCommodity!: RequestItemCommodity;
  WarehouseLayouts: WarehouseLayout[] = [];
  WarehouseLayoutTree: WarehouseLayoutTree[] = [];
  public warehouses: Warehouse[] = [];

  constructor(
    private _mediator:Mediator,
    private dialogRef: MatDialogRef<WarehouseLayoutsAddInventoryDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();

    this.requestItemCommodity = data.ItemCommodity;

    this.request = new WarehouseLayoutsAddInventoryCommand();


  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {


    await this.initialize()
  }

  async initialize() {

    let newRequest = new WarehouseLayoutsAddInventoryCommand()


    newRequest.commodityTitle = this.requestItemCommodity.commodityName;
    newRequest.commodityId = Number(this.requestItemCommodity.commodityId);
    newRequest.quantity = Number(this.requestItemCommodity.quantity);
    newRequest.commodityCode = this.requestItemCommodity.commodityCode;
    newRequest.layoutTitle = this.requestItemCommodity.layoutTitle;
    this.request = newRequest;
    this.disableControls();

  }


  async getWarehouseLayoutTree() {

    let searchQuery = [
      new SearchQuery({
        propertyName: 'warehouseId',
        comparison: 'equal',
        values: [this.form.controls.warehouseId.value],
        nextOperand:'and'
      }),

    ]

    this.WarehouseLayoutTree = [];

    await this._mediator.send(new GetWarehouseLayoutTreesQuery(0, 0, searchQuery, "id ASC")).then(res => {
      ;
      this.WarehouseLayoutTree = res.data;


    })

  }
  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);
    this.getWarehouseLayoutTree()

  }
  getWarehouseLayoutId(id: number) {

    this.form.controls.warehouseLayoutId.patchValue(id);


  }
  debitReferenceSelect(item: any) {

    this.form.controls.debitAccountReferenceId.setValue(item?.id);
    this.form.controls.debitAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }
  creditReferenceSelect(item: any) {

    this.form.controls.creditAccountReferenceId.setValue(item?.id);
    this.form.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }

  debitAccountHeadIdSelect(item: any) {

    this.form.controls.debitAccountHeadId.setValue(item?.id);

  }
  creditAccountHeadIdSelect(item: any) {

    this.form.controls.creditAccountHeadId.setValue(item?.id);

  }
  async add(){

    await this._mediator.send(<WarehouseLayoutsAddInventoryCommand>this.request).then(res => {

      this.dialogRef.close({

        response:res,

      })

    });
  }
  disableControls() {

    this.form.controls['commodityTitle'].disable();
    this.form.controls['commodityId'].disable();
    this.form.controls['commodityCode'].disable();
    this.form.controls['quantity'].disable();

  }



  async update(entity?: any) {

  }

  async delete() {

  }



  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

}


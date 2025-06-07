import { Component, Inject } from '@angular/core';
import { Warehouse } from "../../../entities/warehouse";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { PageModes } from "../../../../../core/enums/page-modes";
import { CreateWarehouseCommand } from "../../../repositories/warehouse/commands/create-warehouse-command";
import { UpdateWarehouseCommand } from "../../../repositories/warehouse/commands/update-warehouse-command";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { DeleteWarehouseCommand } from "../../../repositories/warehouse/commands/delete-warehouse-command";
import { CommodityCategory } from "../../../../commodity/entities/commodity-category";

import { Permission } from '../../../../admin/entities/permission';
import { GetPermissionsQuery } from '../../../../admin/repositories/permission/queries/get-permissions-query';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { GetCommodityCategoriesTreeQuery } from '../../../repositories/commodity-categories/get-commodity-categories-tree-query';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { GetWarehouseQuery } from '../../../repositories/warehouse/queries/get-warehouse-query';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { GetALLCodeVoucherGroupsQuery } from '../../../repositories/receipt/queries/receipt/get-code-voucher-groups-query';

@Component({
  selector: 'app-warehouse-dialog',
  templateUrl: './warehouse-dialog.component.html',
  styleUrls: ['./warehouse-dialog.component.scss']
})
export class WarehouseDialogComponent extends BaseComponent {

  pageModes = PageModes;
  Warehouse!: Warehouse;
  CommodityCategories: CommodityCategory[] = [];
  filterCommodityCategories: CommodityCategory[] = [];
  filterReceiptAllStatus: ReceiptAllStatusModel[] = [];
  filterReceiptNodes: any[] = [];
  permissions: Permission[] = [];

  constructor(
    private _mediator: Mediator,
    @Inject(MAT_DIALOG_DATA) data: any,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    private dialogRef: MatDialogRef<WarehouseDialogComponent>,
  ) {
    super();

    this.Warehouse = data.Warehouse;



    this.pageMode = data.pageMode;
    this.request = new CreateWarehouseCommand();

  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {


   
    await this._mediator.send(new GetCommodityCategoriesTreeQuery()).then(res => {
      this.CommodityCategories = res;
      
    })
    await this._mediator.send(new GetALLCodeVoucherGroupsQuery(0, 0, undefined, "id ASC")).then(res => {

      this.filterReceiptNodes = res.data;
    });
    await this.PermissionsFilter();
    await this.initialize()
  }

  async initialize() {


    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreateWarehouseCommand()
      if (this.Warehouse) {
        newRequest.parentId = this.Warehouse.id;


      }
      this.request = newRequest;
    }
    if (this.pageMode === PageModes.Update) {

      this._mediator.send(new GetWarehouseQuery(this.Warehouse.id)).then(res => {
        this.Warehouse = res;

        let newRequest = new UpdateWarehouseCommand()
        newRequest.title = this.Warehouse.title;
        newRequest.id = this.Warehouse.id;
        this.request = newRequest;

        this.form.controls.title.setValue(this.Warehouse.title);
        this.form.controls.id.setValue(this.Warehouse.id);
        this.form.controls.sort.setValue(this.Warehouse.sort);
        this.form.controls.accountHeadId.setValue(this.Warehouse.accountHeadId);
        this.form.controls.isActive.setValue(this.Warehouse.isActive);
        this.form.controls.parentId.setValue(this.Warehouse.parentId);
        this.filterCommodityCategories = this.Warehouse.commodityCategories;
        this.filterReceiptAllStatus = this.Warehouse.receiptAllStatus;
      })

    }
    //-------------آوردن اطلاعات کامل برای نمایش تامین کننده در هنگام ویرایش
    let defaultId: any = undefined;
    if (this.Warehouse?.accessPermission) {
      defaultId = this.Warehouse?.accessPermission.toString();
    }



  }

  async add() {
    var requestnNew = new CreateWarehouseCommand();
    requestnNew.title = this.form.controls.title.value;
    requestnNew.accessPermission = this.form.controls.accessPermission.value;
    requestnNew.isActive = this.form.controls.isActive.value;
    requestnNew.parentId = this.form.controls.parentId.value;
    requestnNew.sort = this.form.controls.sort.value;
    requestnNew.accountHeadId = this.form.controls.accountHeadId.value;
    requestnNew.CommodityCategories = this.filterCommodityCategories;
    requestnNew.ReceiptAllStatus = this.filterReceiptAllStatus;
   

    await this._mediator.send(<CreateWarehouseCommand>requestnNew).then(res => {

      this.dialogRef.close({

        response: res,
        pageMode: this.pageMode
      })

    });
  }

  async update(entity?: any) {
    var requestnNew = new UpdateWarehouseCommand();
    requestnNew.title = this.form.controls.title.value;
    requestnNew.accessPermission = this.form.controls.accessPermission.value;
    requestnNew.isActive = this.form.controls.isActive.value;
    requestnNew.id = this.form.controls.id.value;
    requestnNew.sort = this.form.controls.sort.value;
    requestnNew.parentId = this.form.controls.parentId.value;
    requestnNew.CommodityCategories = this.filterCommodityCategories;
    requestnNew.ReceiptAllStatus = this.filterReceiptAllStatus;
    requestnNew.accountHeadId = this.form.controls.accountHeadId.value;

    await this._mediator.send(<UpdateWarehouseCommand>requestnNew).then(res => {
      this.dialogRef.close({
        response: res,
        pageMode: this.pageMode
      })
    });
  }

  async delete() {
    await this._mediator.send(new DeleteWarehouseCommand((<UpdateWarehouseCommand>this.request).id ?? 0)).then(res => {

      this.dialogRef.close({
        response: res,
        pageMode: PageModes.Delete
      })

    });
  }

  getCommodityCategoryList(items: CommodityCategory[]) {

    this.filterCommodityCategories = items;

  }
  getReceiptAllStatusList(items: ReceiptAllStatusModel[]) {

    this.filterReceiptAllStatus = items;

  }
  accountHeadIdSelect(item: any) {

    this.form.controls.accountHeadId.setValue(item?.id);

  }

  async PermissionsFilter() {

    var filter = [

      new SearchQuery({
        propertyName: 'subsystem',
        comparison: 'contains',
        values: ['inventory'],
        nextOperand: 'or'
      }),

    ]

    //----------------
    await this._mediator.send(new GetPermissionsQuery(0, 0, filter)).then(res => {

      this.permissions = res.data
    })

  }
  PermissionSelect(id: number) {

    this.form.controls.accessPermission.setValue(id);

  }
  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

}


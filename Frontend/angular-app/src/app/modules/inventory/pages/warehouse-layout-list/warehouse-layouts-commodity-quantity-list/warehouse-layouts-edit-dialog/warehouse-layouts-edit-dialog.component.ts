import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from '../../../../../../core/abstraction/base.component';
import { PageModes } from '../../../../../../core/enums/page-modes';
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { WarehouseLayout, WarehouseLayoutTree } from '../../../../entities/warehouse-layout';
import { WarehouseLayoutsCommodityQuantity } from '../../../../entities/warehouse-layouts-commodity-quantity';
import { EditWarehouseLayoutInventoryCommand } from '../../../../repositories/warehouse-layout/commands/edit-warehouse-layout-inventory-command';




@Component({
  selector: 'app-warehouse-layouts-edit-dialog',
  templateUrl: './warehouse-layouts-edit-dialog.component.html',
  styleUrls: ['./warehouse-layouts-edit-dialog.component.scss']
})
export class EditWarehouseLayoutsDialogComponent extends BaseComponent {


  pageModes = PageModes;
  WarehouseLayout!: WarehouseLayoutsCommodityQuantity;
  WarehouseLayouts: WarehouseLayout[] = [];
  Total: number = 0
  //-------------------نحوه محاسبه--------------------------
  typeCalculateId: number = 1;
  typeCalculate = [

    { "name": "اصلاحیه افزایش موجودی", ID: "1", "checked": true },
    { "name": "اصلاحیه کاهش موجودی", ID: "-1", "checked": false }

  ]

  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<EditWarehouseLayoutsDialogComponent>,
    public Service: PagesCommonService,
    @Inject(MAT_DIALOG_DATA) data: any,
    public _notificationService: NotificationService,
  ) {
    super();

    this.WarehouseLayout = data.warehouseLayout;
    this.request = new EditWarehouseLayoutInventoryCommand();


  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {


    await this.initialize()
  }

  async initialize() {

    let newRequest = new EditWarehouseLayoutInventoryCommand()

    newRequest.warehouseLayoutTitle = this.WarehouseLayout.warehouseLayoutTitle;
    newRequest.commodityTitle = this.WarehouseLayout.commodityTitle;
    newRequest.warehouseLayoutsCommodityQuantitId = Number(this.WarehouseLayout.id);
    newRequest.currentQuantity = Number(this.WarehouseLayout.quantity);
    newRequest.newQuantity = Number(this.WarehouseLayout.quantity);
    newRequest.commodityCode = this.WarehouseLayout.commodityCode;
    newRequest.commodityId = Number(this.WarehouseLayout.commodityId);
    newRequest.warehouseLayoutId = Number(this.WarehouseLayout.warehouseLayoutId);
    newRequest.warehouseId = Number(this.WarehouseLayout.warehouseId);


    this.request = newRequest;
    this.disableControls();


  }

  async add() {


    if (this.form.controls.quantity.value != undefined && this.form.controls.newQuantity.value >=0) {
      let newRequest = new EditWarehouseLayoutInventoryCommand()
      newRequest.warehouseLayoutsCommodityQuantitId = this.form.controls.warehouseLayoutsCommodityQuantitId.value;
      newRequest.quantity = this.form.controls.quantity.value;
      newRequest.commodityId = this.form.controls.commodityId.value;
      newRequest.warehouseLayoutId = this.form.controls.warehouseLayoutId.value;
      newRequest.warehouseId = this.form.controls.warehouseId.value;
      newRequest.mode = this.typeCalculateId;
      this.request = newRequest;

      await this._mediator.send(<EditWarehouseLayoutInventoryCommand>this.request).then(res => {

        this.dialogRef.close({

          response: res,

        })

      });
    }
    else if (this.form.controls.quantity.value == undefined) {
      this.Service.showHttpFailMessage('تعداد اصلاحی را وارد نمایید');
    }
    else if (this.form.controls.newQuantity.value <=0) {
      this.Service.showHttpFailMessage('تعداد پس از اصلاح نمی تواند منفی باشد.');
    }
  }
  disableControls() {
    this.form.controls['warehouseLayoutTitle'].disable();
    this.form.controls['newQuantity'].disable();
    this.form.controls['currentQuantity'].disable();
    this.form.controls['commodityTitle'].disable();
    this.form.controls['commodityId'].disable();
    this.form.controls['commodityCode'].disable();

  }
  AssignmentConfirmation(Id: number) {

    this.typeCalculateId = Id;
    this.onComputing();


  }
  onComputing() {
    let total = Number(this.typeCalculateId) * Number(this.form.controls.quantity.value) + Number(this.form.controls.currentQuantity.value)
    this.form.controls.newQuantity.setValue(total);

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


import { Receipt } from '../../../../../entities/receipt';
import { FormArray, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from "@angular/router";
import { Warehouse } from '../../../../../entities/warehouse';
import { PageModes } from '../../../../../../../core/enums/page-modes';
import { Commodity } from '../../../../../../commodity/entities/commodity';
import { Component, ElementRef, TemplateRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../../../core/constants/form-action-types";
import { IdentityService } from "../../../../../../identity/repositories/identity.service";
import { PagesCommonService } from '../../../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../../../shared/services/notification/notification.service';
import { GetTemporaryRecepitQuery } from '../../../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-query';
import { UpdateLeavingMaterialWarehouseCommand } from '../../../../../repositories/warehouse-layout/commands/leaving-warehouse/update-leaving-material-warehouse-command';
import { MaterialItemsComponent } from '../material-items/material-items.component';
import { UpdateAvgPriceAfterChangeBuyPriceCommand } from '../../../../../repositories/receipt/commands/reciept/update-avg-price-after-change-buy-price-command';

@Component({
  selector: 'app-update-materail-warehouse',
  templateUrl: './update-materail-warehouse.component.html',
  styleUrls: ['./update-materail-warehouse.component.scss']
})
export class UpdateMarerialWarehouseComponent extends BaseComponent {


  public documentTags: string[] = [];
  public codeVoucherGroup: any = "";
  public isSubmitForm: boolean = false;
  public commodities: Commodity[] = [];
  public _receipt: Receipt | undefined = undefined;
  public IsViewDebit: boolean = false;
  public IsViewCredit: boolean = false
  //------------------درحالتی که جابه جایی بین انبار ها باشد از طریق انبار تحویل گیرنده فیلتر شود
  //------------------اگر خروج تنها از انبار باشد با انبار تحویل دهنده فیلتر شود
  public commodityCategorylevelCode: any = undefined;

  child: any;
  @ViewChild(MaterialItemsComponent)
  set appShark(child: MaterialItemsComponent) {
    this.child = child
  };
  constructor(
    private router: Router,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
    this.request = new UpdateLeavingMaterialWarehouseCommand();
  }


  async ngOnInit() {


  }

  async ngAfterViewInit() {
    await this.resolve()
  }
  async resolve(params?: any) {
    this.formActions = [

      FormActionTypes.refresh,
      FormActionTypes.list,
    ];
    this.disableControls()
    await this.initialize()

  }


  async initialize(entity?: any) {
    //-----------------------------------
    if (entity || this.getQueryParam('id')) {
      await this.ApiCallService.getReceiptAllStatus('');

      this.initializeUpdate();

    }
  }
  async initializeUpdate(entity?: any) {
    if (!entity) entity = await this.get(this.getQueryParam('id'))
    {
      this.request = new UpdateLeavingMaterialWarehouseCommand().mapFrom(entity);
      this._receipt = entity;
      if (this._receipt?.tagArray)
        this.documentTags = this._receipt?.tagArray

      //-------------------------کد نوع رسید
      this.codeVoucherGroup = this.ApiCallService.AllReceiptStatus.find(a => a.id == this._receipt?.codeVoucherGroupId)?.uniqueName

    }
    this.pageMode = PageModes.Update;

    this.disableControls();
  }

  //-----------------------------------------------
  disableControls() {

    this.form.controls['documentNo'].disable();

  }
  //----------------ذخیره------------------------------
  async update() {

    if (this.form.controls['warehouseId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار را انتخاب نمایید');
      return;

    }

    var tagstring: string = await this.Service.TagConvert(this.documentTags);
    this.form.controls.tags.setValue(tagstring);



    await this._mediator.send(<UpdateLeavingMaterialWarehouseCommand>this.request).then(response => {
      this.get(this.getQueryParam('id'))
    });

  }
  get receiptDocumentItems(): FormArray {

    return this.form.controls['receiptDocumentItems'] as FormArray;
  }

  //-----------------انبار تحویل دهنده-------------------------------
  async WarehouseIdSelect(item: Warehouse) {

    this.form.controls.fromwarehouseId.setValue(item?.id);

  }

  async navigateToReceiptList() {

    await this.router.navigateByUrl('/inventory/materilaReceiptList')
  }
  async reset() {
    this.form.controls['warehouseId'].value = undefined;


    this.codeVoucherGroup = "";
    return super.reset();

  }

  TagSelect(tagstring: string[]) {
    this.documentTags = tagstring;
  }
  //----------------------------------------------
  async AddItems() {
    this.child.AddItems();


  }
  async AddNew() {
    if (this.getQueryParam('codeVoucherGroupId') != undefined) {
      await this.router.navigateByUrl('/inventory/receipt-operations/leavingMaterialWarehouse?codeVoucherGroupId=' + this.getQueryParam('codeVoucherGroupId'))
    }
    else {
      await this.router.navigateByUrl('/inventory/receipt-operations/leavingMaterialWarehouse')
    }

  }
  async get(Id: number) {
    return await this._mediator.send(new GetTemporaryRecepitQuery(Id))
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

  close(): any {
  }

  delete(param?: any): any {
  }

  async add() {

  }
  async edit() {
  }
}

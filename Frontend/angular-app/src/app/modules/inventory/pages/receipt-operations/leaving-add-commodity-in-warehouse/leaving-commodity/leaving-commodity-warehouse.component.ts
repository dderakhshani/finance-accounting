import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../../core/constants/form-action-types";
import { IdentityService } from "../../../../../identity/repositories/identity.service";
import { Receipt } from '../../../../entities/receipt';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { LeavingCosumableWarehouseCommand } from '../../../../repositories/warehouse-layout/commands/leaving-warehouse/leaving-consumable-warehouse-command';

import { ReceiptItem } from '../../../../entities/receipt-item';
import { ApiCallService } from '../../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery } from '../../../../repositories/receipt/queries/receipt/get-receipt-by-documnetNo-query';
import { LeavingDocumentItem } from '../../../../entities/leaving-cosumable-documentItem';



@Component({
  selector: 'app-leaving-commodity-warehouse',
  templateUrl: './leaving-commodity-warehouse.component.html',
  styleUrls: ['./leaving-commodity-warehouse.component.scss']
})
export class LeavingCommodityWarehouseComponent extends BaseComponent {


  documentTags: string[] = [];

  public _Request: Receipt | undefined = undefined;
  public Receipt: Receipt | undefined = undefined;

  public requestCodeVoucherGroupUniqueCode: string | undefined = undefined;//نیاز برای شناخت کالاهای اموال
  public AssetsList: number[] = [];
  public documentStauseRequest: boolean = false;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private identityService: IdentityService,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    private _mediator: Mediator,
    public ApiCallService: ApiCallService,
  ) {
    super(route, router);

    this.request = new LeavingCosumableWarehouseCommand()

  }
  async ngAfterViewInit() {
    await this.resolve()
  }
  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.list,
    ];
    await this.initialize()
  }


  async initialize(entity?: any) {

    //---------------اگر از فرم دریافت کالا از انبار آمده باشد----------------

    if (this.getQueryParam('documentNo')) {

      this.documentStauseRequest = true;

      this.form.controls['documentNo'].setValue(this.getQueryParam('documentNo'));

      this.form.controls['codeVoucherGroupId'].setValue(this.getQueryParam('codeVoucherGroupId'));
      await this.get();


    }
    else {
      this.documentStauseRequest = false;
      this.request = new LeavingCosumableWarehouseCommand();
    }
    this.form.controls['codeVoucherGroupId'].setValue(this.getQueryParam('codeVoucherGroupId'));
  }

  //====================جستجو درخواست=======================================
  //----------------جستجو درخواست-------------------------------------------
  async getRequest() {
    if (this.form.controls.documentNo.value != undefined) {
      this.get()
    }
    else {
      this.Service.showHttpFailMessage('شماره درخواست خرید وارد نمایید');
    }

  }

  async get() {


    await this._mediator.send(new GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery(this.form.controls['documentNo']?.value, this.form.controls['codeVoucherGroupId'].value)).then(async (res) => {
      this._Request = res;
      this.requestCodeVoucherGroupUniqueCode = this.ApiCallService.AllReceiptStatus.find(a => a.id == this._Request?.codeVoucherGroupId)?.uniqueName;

    })

  }


  async navigateToReceiptList() {

    await this.router.navigateByUrl('request-list/requestCommodityList')

  }
  async reset() {
    await this.deleteQueryParam("documentNo")
    return super.reset();
  }


  async onSave() {


    let valid = await this.validate();
    if (valid) {
      var request = new LeavingCosumableWarehouseCommand();
      var items: LeavingDocumentItem[] = [];
      var assetsId: any[] = [];
      
      this._Request?.items.forEach(a => {
        if (a.assetsSerials != undefined && a.assetsSerials.length > 0) {
          var dd = a.assetsSerials.filter(a => a.selected);

          assetsId = dd.map(function (el) { return el.id; });

        }
        a.layouts.forEach(b => {
          items.push({
            documentItemId: a.id,
            warehouseId: Number(b.warehouseId),
            warehouseLayoutId: Number(b.warehouseLayoutId),
            commodityId: Number(b.commodityId),
            quantity: Number(b.quantityChose),
            warehouseLayoutQuantityId: Number(b.id),
            description: a.description,
            assetsId: assetsId,
            measureId: a.mainMeasureId
          })
        })

      })
      request.id = this._Request?.id;

      request.codeVoucherGroupId = this.form.controls['codeVoucherGroupId'].value;
      request.documentItems = items;
      await this._mediator.send(<LeavingCosumableWarehouseCommand>request).then(res => {
        this.get();
        
      })

    }
  }
  async validate() {
    var valid: boolean = true;

    var validation = await this._Request?.items.forEach(a => {

      
      var sumQuantity: number = 0;

      a.layouts.forEach(l => {

        sumQuantity = sumQuantity + Number(l.quantityChose);
        if (Number(a.quantity) <= 0) {
          this.Service.showHttpFailMessage('تعداد خروجی  کالا  ' + l.commodityTitle + ' موجود نمی باشد ');
          valid = false;

        }
        if (sumQuantity > Number(a.quantity)) {
          this.Service.showHttpFailMessage('تعداد خروجی  کالا  ' + l.commodityTitle + ' مغایرت  با تعداد درخواستی دارد');
          valid = false;

        }
        else if (!l.allowOutput) {
          this.Service.showHttpFailMessage('اجازه خروج کالا ' + l.commodityTitle + ' وجود ندارد ');
          valid = false;

        }

      })

    })
    return valid;
  }
  assetIdSelect(items: any[], receiptItem: ReceiptItem) {

    items.forEach(b => {
      var assts = receiptItem.assetsSerials.find(a => a.id == b.id)
      if (assts != undefined)
        assts.selected = true;
    })

  }
  codeVoucherGroupSelect(item: any) {

    this.form.controls.codeVoucherGroupId.setValue(item?.id);

  }
  async navigateToHistory(commodityId: number) {

    var url = `inventory/commodityReceiptReports?commodityId=${commodityId}&warehouseId=${this._Request?.warehouseId}`
    this.router.navigateByUrl(url)

  }
  async navigateToReceiptDetails(documentNo: any, documentStauseBaseValue:number) {
    await this.router.navigateByUrl(`inventory/receiptDetails?documnetNo=${documentNo}&displayPage=archive&isImportPurchase=false&documentStauseBaseValue=${documentStauseBaseValue}`)
  }
  async update() {

  }
  delete() {


  }
  async edit() {
  }
  close(): any {
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
}

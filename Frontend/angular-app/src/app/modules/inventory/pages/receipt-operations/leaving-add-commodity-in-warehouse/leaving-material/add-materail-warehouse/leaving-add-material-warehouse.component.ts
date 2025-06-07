import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../../../core/constants/form-action-types";
import { IdentityService } from "../../../../../../identity/repositories/identity.service";
import { Warehouse } from '../../../../../entities/warehouse';
import { Receipt } from '../../../../../entities/receipt';
import { FormArray, FormControl } from '@angular/forms';
import { Commodity } from '../../../../../../commodity/entities/commodity';
import { AddItemsCommand } from '../../../../../repositories/receipt/commands/receipt-items/add-receipt-items-command';
import { PagesCommonService } from '../../../../../../../shared/services/pages/pages-common.service';
import { ReceiptAllStatusModel } from '../../../../../entities/receipt-all-status';
import { GetTemporaryRecepitQuery } from '../../../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-query';
import { AddleavingMaterialWarehouseCommand } from '../../../../../repositories/warehouse-layout/commands/leaving-warehouse/add-leaving-material-warehouse-command';
import { NotificationService } from '../../../../../../../shared/services/notification/notification.service';
import { TableComponent } from '../../../../../../../core/components/custom/table/table.component';
import { MaterialItemsComponent } from '../material-items/material-items.component';
import { ApiCallService } from '../../../../../../../shared/services/pages/api-call/api-call.service';
import { UpdateAvgPriceAfterChangeBuyPriceCommand } from '../../../../../repositories/receipt/commands/reciept/update-avg-price-after-change-buy-price-command';
import { GetRecepitQuery } from '../../../../../repositories/receipt/queries/receipt/get-receipt-query';


@Component({
  selector: 'app-leaving-add-material-warehouse',
  templateUrl: './leaving-add-material-warehouse.component.html',
  styleUrls: ['./leaving-add-material-warehouse.component.scss']
})
export class LeavingAddMarerialWarehouseComponent extends BaseComponent {

  public documentTags: string[] = [];
  public isSubmitForm: boolean = false;
  public temp_ToWarehouseId: any;
  public temp_FromWarehouseId: any = this.Service.FromWarehoseId;//انبار پیش فرض خروجی باید باشد
  public temp_documentDate: any;
  public temp_codeVoucherGroupId: any;
  public temp_creditAccountHeadId: any;
  public temp_debitAccountHeadId: any;
  public codeVoucherGroup: string = "";
  public commodities: Commodity[] = [];
  public _Receipt: Receipt | undefined = undefined;
  public ViewId: any = this.Service.ViewIdRemoveAddWarehouse;
  public IsViewDebit: boolean = false;
  public IsViewCredit: boolean = false
  //---------------شمارش تعداد سطرهای کالا -----------------
  public rowCount: number = 1;
  //------------------درحالتی که جابه جایی بین انبار ها باشد از طریق انبار تحویل گیرنده فیلتر شود
  //------------------اگر خروج تنها از انبار باشد با انبار تحویل دهنده فیلتر شود



  child: any;
  @ViewChild(MaterialItemsComponent)
  set appShark(child: MaterialItemsComponent) {
    this.child = child
  };
  constructor(
    private router: Router,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public ServiceAPI: ApiCallService,
    public Service: PagesCommonService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);
    this.request = new AddleavingMaterialWarehouseCommand()
  }


  async ngOnInit() {

    

  }
  async ngAfterViewInit() {
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
    this.isSubmitForm = false
    this.request = new AddleavingMaterialWarehouseCommand();
     entity = await this.get(this.getQueryParam('RequestId'))
    {
      this.request = new AddleavingMaterialWarehouseCommand().mapFrom(entity);
      this.form.controls.fromWarehouseId.setValue(undefined)
      this.form.controls.id.setValue(undefined)
      this.form.controls.codeVoucherGroupId.setValue(undefined)
      this.form.controls.documentStauseBaseValue.setValue(undefined)
      this.form.controls.fromWarehouseId.setValue(undefined)
      this.form.controls.toWarehouseId.setValue(undefined)
      this.form.controls.documentDescription.setValue(undefined)
      this.form.controls.documentNo.setValue(undefined);



    }
    this.form.controls['documentNo'].disable();
    
    
    this.ServiceAPI.getReceiptAllStatus('').then(a => {
      this.ViewId = this.ServiceAPI.ReceiptStatus.find(a => a.code == this.getQueryParam('codeVoucherGroupId'))?.viewId;

    })

  }
  async getData(Id: number) {
    return await this._mediator.send(new GetRecepitQuery(Id))
  }

  //----------------ذخیره------------------------------
  async add() {
    //اگر رسید محصول باشد ، انبار تحویل دهنده از روی فرمول ساخت خوانده شود.
    if (this.codeVoucherGroup == this.Service.ProductReceiptWarehouse) {
      this.form.controls.fromWarehouseId.setValue(this.child.fromWarehouseId);

    }
    if (this.form.controls['documentNo'].value != undefined) {
      this.Service.showWarrningMessage('رسید قبلا ذخیره شده است')
      return;
    }
    if (this.form.controls['codeVoucherGroupId'].value == undefined) {

      this.Service.showHttpFailMessage('نوع رسید را بدرستی انتخاب نمایید');
      return;

    }
    if (this.form.controls['fromWarehouseId'].value == undefined && this.codeVoucherGroup == this.Service.RemoveMaterialWarhouse) {

      this.Service.showHttpFailMessage('انبار تحویل تحویل دهنده را انتخاب نمایید');
      return;

    }
    if (this.form.controls['toWarehouseId'].value == undefined && this.codeVoucherGroup != this.Service.RemoveMaterialWarhouse) {

      this.Service.showHttpFailMessage('انبار تحویل تحویل گیرنده را انتخاب نمایید');
      return;

    }


    if (this.form.controls['fromWarehouseId'].value == this.form.controls['toWarehouseId'].value) {
      this.Service.showHttpFailMessage('انبار تحویل دهنده و انبار تحویل گیرنده یکسان انتخاب شده است');
      return;
    }
    if (this.form.controls['documentDate'].value == undefined) {

      this.Service.showHttpFailMessage('تاریخ رسید را انتخاب نمایید');
      return;

    }
    var tagstring: string = await this.Service.TagConvert(this.documentTags);


    this.form.controls.tags.setValue(tagstring);



    this.form.controls.isManual.setValue(true);
    this.form.controls.viewId.setValue(this.Service.ViewIdRemoveAddWarehouse);


    await this._mediator.send(<AddleavingMaterialWarehouseCommand>this.request).then(response => {

      this.isSubmitForm = true;


      var activeTab = this.Service.TabManagerService.activeTab;
      if (activeTab != undefined) {
        this.Service.TabManagerService.closeTab(activeTab)
      }
      let UpdateAvgPrice = new UpdateAvgPriceAfterChangeBuyPriceCommand();
      UpdateAvgPrice.documentId = response.documentId;
      this._mediator.send(<UpdateAvgPriceAfterChangeBuyPriceCommand>UpdateAvgPrice);

      this.navigateToUpdateReceipt(response.id);
    });

  }

  get receiptDocumentItems(): FormArray {

    return this.form.controls['receiptDocumentItems'] as FormArray;
  }

  //-----------------انبار تحویل دهنده-------------------------------
  async FromWarehouseIdSelect(item: Warehouse) {


    this.form.controls.fromWarehouseId.setValue(item?.id);
    this.form.controls.creditAccountHeadId.setValue(item?.accountHeadId);

    if (item.title.includes('انبار كالاى نيمه ساخته')) {
      this.form.controls.creditAccountReferenceGroupId.setValue(5)
    }

    if (item.title.includes('انبار امانی ما نزد دیگران') || item.title.includes('انبار امانی دیگران نزد ما ') || item.title.includes('انبار كالاى نيمه ساخته')) {
      this.IsViewCredit = true;
    }


    this.AddFirstRow();


  }
  //------------------انبار تحویل گیرنده------------------------
  async ToWarehouseIdSelect(item: Warehouse) {


    this.form.controls.toWarehouseId.setValue(item?.id);
    this.form.controls.debitAccountHeadId.setValue(item?.accountHeadId);

    if (item.title.includes('انبار كالاى نيمه ساخته')) {
      this.form.controls.debitAccountReferenceGroupId.setValue(5)
    }
    if (item.title.includes('انبار امانی ما نزد دیگران') || item.title.includes('انبار امانی دیگران نزد ما ') || item.title.includes('انبار كالاى نيمه ساخته')) {
      this.IsViewDebit = true;
    }


    this.AddFirstRow();

  }
  //----------------فقط در قسمت انتخاب انبار صدا زده می شود
  //-----------------در هنگامی که اولین بار است که انبار انتخاب می شود یک سطر برای کالا اضافه شود.
  async AddFirstRow() {

    if (this.rowCount == 1 && !this.getQueryParam('RequestId')) {
      await this.AddItems();
    }
    else if (!this.getQueryParam('RequestId')) { 
      //اگر نوع انبار تغییر کرد داده های کالاهای قبلی باید پاک شود.
      var list: FormArray = <FormArray>this.form.controls['receiptDocumentItems'];
      list.reset();
    }

  }

  async codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    //اگر نوع رسید تغییر کرد داده های قبلی باید پاک شود.
    if (this.codeVoucherGroup != "" && !this.getQueryParam('RequestId')) {
      this.reset();
    }

    this.form.controls.codeVoucherGroupId.setValue(item?.id);
    this.codeVoucherGroup = item.uniqueName;
    this.form.controls.documentStauseBaseValue.setValue(this.Service.ViewIdRemoveAddWarehouse);

    if (this.codeVoucherGroup == this.Service.RemoveMaterialWarhouse) {
      this.form.controls.debitAccountHeadId.setValue(item?.defultDebitAccountHeadId);
    }
    if (this.codeVoucherGroup == this.Service.ProductReceiptWarehouse) {
      this.form.controls.creditAccountHeadId.setValue(item?.defultCreditAccountHeadId);
    }

    if (this.codeVoucherGroup == this.Service.ProductReceiptWarehouse) {//--------------جابه جایی محصول با فرمول ساخت
      //در این شرایط یک انبار به صورت پیش فرض برای بک اند ارسال می کنیم تا بتوانیم رسید خروجی را بزنیم.
      this.form.controls.fromWarehouseId.setValue(this.Service.FromWarehoseId);

    }

  }


  async navigateToUpdateReceipt(id: number) {

    await this.router.navigateByUrl(`inventory/receipt-operations/updateMaterilaReceip?id=${id}`)

  }
  async navigateToReceiptList(id: number) {

    await this.router.navigateByUrl(`inventory/materilaReceiptList`)

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
  async reset() {


    this.isSubmitForm = false;
    this.SetTempValue();
    super.reset();
    this.GetTempValue();



  }
  SetTempValue() {
    this.temp_FromWarehouseId = this.form.controls['fromWarehouseId'].value;
    this.temp_ToWarehouseId = this.form.controls['toWarehouseId'].value;
    this.temp_codeVoucherGroupId = this.form.controls['codeVoucherGroupId'].value;
    this.temp_documentDate = this.form.controls['documentDate'].value;
    this.temp_creditAccountHeadId = this.form.controls['creditAccountHeadId'].value;
    this.temp_debitAccountHeadId = this.form.controls['debitAccountHeadId'].value;
  }
  GetTempValue() {
    this.form.controls['fromWarehouseId'].setValue(this.temp_FromWarehouseId);
    this.form.controls['toWarehouseId'].setValue(this.temp_ToWarehouseId);
    this.form.controls['codeVoucherGroupId'].setValue(this.temp_codeVoucherGroupId);
    this.form.controls['documentDate'].setValue(this.temp_documentDate);
    this.form.controls['creditAccountHeadId'].setValue(this.temp_creditAccountHeadId);
    this.form.controls['debitAccountHeadId'].setValue(this.temp_debitAccountHeadId);
  }
  TagSelect(tagstring: string[]) {
    this.documentTags = tagstring;
  }
  //----------------------------------------------
  async AddItems() {

    this.child.AddItems();
    this.rowCount = this.rowCount + 1

  }
  async get(Id: number) {
    return await this._mediator.send(new GetTemporaryRecepitQuery(Id))
  }
  close(): any {
  }

  delete(param?: any): any {
  }
  async update() {

  }
  async edit() {
  }
}

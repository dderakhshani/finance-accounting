import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../../core/constants/form-action-types";
import { IdentityService } from "../../../../../identity/repositories/identity.service";
import { Warehouse } from '../../../../entities/warehouse';
import { Receipt } from '../../../../entities/receipt';
import { AddReceiptCommand } from '../../../../repositories/receipt/commands/temporary-receipt/add-receipt-command';
import { FormArray, FormControl } from '@angular/forms';
import { Commodity } from '../../../../../commodity/entities/commodity';
import { AddItemsCommand } from '../../../../repositories/receipt/commands/receipt-items/add-receipt-items-command';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { ReceiptAllStatusModel } from '../../../../entities/receipt-all-status';
import { DocumentState } from '../../../../entities/documentState';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { ManualItemsComponent } from '../../../component/add-manual-items/add-manual-items.component';
import { ApiCallService } from '../../../../../../shared/services/pages/api-call/api-call.service';

@Component({
  selector: 'app-add-manual-temporary-receipt',
  templateUrl: './add-manual-temporary-receipt.component.html',
  styleUrls: ['./add-manual-temporary-receipt.component.scss']
})
export class AddManualTemporaryReceiptComponent extends BaseComponent {


  public documentTags: string[] = [];
  public _Receipt: Receipt | undefined = undefined;
  public commodities: Commodity[] = [];
  public pageModeTypeUpdate: boolean = false;
  public isSubmitForm: boolean = false;
  public temp_warehouseId: any;
  public temp_codeVoucherGroupId: any;
  public temp_documentDate: any;
  public codevocher: string | undefined = '';
  public ViewId: any = this.Service.CodeTemporaryReceipt;
  //---------------شمارش تعداد سطرهای کالا -----------------
  public rowCount: number = 1;
  child: any;
  @ViewChild(ManualItemsComponent)
  set appShark(child: ManualItemsComponent) {
    this.child = child
  };
  constructor(
    private router: Router,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public ServiceAPI: ApiCallService,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
    this.pageModeTypeUpdate = false;
    this.request = new AddReceiptCommand()

  }

  async ngOnInit() {

    
  }
  initialize(params?: any) {
    this.request = new AddReceiptCommand();

    if (this.getQueryParam('codeVocherGroup')) {
      this.ViewId = this.ServiceAPI.ReceiptStatus.find(a => a.id == this.getQueryParam('codeVocherGroup'))?.viewId;
      this.codevocher = this.ServiceAPI.ReceiptStatus.find(a => a.id == this.getQueryParam('codeVocherGroup'))?.uniqueName;
      this.form.controls.codeVoucherGroupId.setValue(this.getQueryParam('codeVocherGroup'));
    }
    this.disableControls()
  }
  async ngAfterViewInit() {
    await this.resolve()

    
  }

  async resolve(params?: any) {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.list,
    ];

    await this.initialize();

  }


  //-----------------------------------------------
  disableControls() {

    this.form.controls['documentNo'].disable();

  }
  //----------------ذخیره------------------------------
  async add() {

    if (this.form.controls['warehouseId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار تحویل گیرنده را انتخاب نمایید');
      return;

    }
    if (this.form.controls['codeVoucherGroupId'].value == undefined) {

      this.Service.showHttpFailMessage('نوع رسید را بدرستی انتخاب نمایید');
      return;

    }
    if (this.form.controls.documentDate.value == undefined) {

      this.Service.showHttpFailMessage('تاریخ رسید را انتخاب کنید');
      return;

    }

    var tagstring: string = await this.Service.TagConvert(this.documentTags);
    this.form.controls.tags.setValue(tagstring);
    this.form.controls.isManual.setValue(true);
    this.form.controls.documentStauseBaseValue.setValue(DocumentState.Temp);

    await this._mediator.send(<AddReceiptCommand>this.request).then(response => {
      this.form.controls['documentNo'].setValue(response.documentNo);
      this.isSubmitForm = true;
    });

  }
  get receiptDocumentItems(): FormArray {

    return this.form.controls['receiptDocumentItems'] as FormArray;
  }
  public commodityCategorylevelCode: any = undefined;
  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);
    this.commodityCategorylevelCode = item?.levelCode;
    this.AddFirstRow();

  }
  ReferenceSelect(item: any) {

    this.form.controls.creditAccountReferenceId.setValue(item?.id);
    this.form.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);

  }

  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.form.controls.codeVoucherGroupId.setValue(item?.id);
    if (item?.id == undefined) {
      this.WarehouseIdSelect(new Warehouse());
    }

  }
  requesterReferenceSelect(item: any) {

    this.form.controls.requesterReferenceId.setValue(item?.id);

  }
  followUpReferenceIdSelect(item: any) {

    this.form.controls.followUpReferenceId.setValue(item?.id);

  }

  async navigateToReceiptList() {

    await this.router.navigateByUrl('/inventory/temporaryReceiptList?codeVocherGroup=' + this.getQueryParam('codeVocherGroup'))
  }
  async reset() {

    this.SetTempValue();
    super.reset();
    this.GetTempValue();
    this.rowCount = 1;
    this.AddFirstRow();

  }
  SetTempValue() {
    this.temp_warehouseId = this.form.controls['warehouseId'].value;
    this.temp_codeVoucherGroupId = this.form.controls['codeVoucherGroupId'].value;
    this.temp_documentDate = this.form.controls['documentDate'].value;
  }
  GetTempValue() {
    this.form.controls['warehouseId'].setValue(this.temp_warehouseId);
    this.form.controls['codeVoucherGroupId'].setValue(this.temp_codeVoucherGroupId);
    this.form.controls['documentDate'].setValue(this.temp_documentDate);

  }
  TagSelect(tagstring: string[]) {
    this.documentTags = tagstring;
  }
  //----------------------------------------------
  async AddItems() {

    this.child.AddItems();
    this.rowCount = this.rowCount + 1

  }
  //----------------فقط در قسمت انتخاب انبار صدا زده می شود
  //-----------------در هنگامی که اولین بار است که انبار انتخاب می شود یک سطر برای کالا اضافه شود.
  async AddFirstRow() {

    if (this.rowCount == 1) {
      await this.AddItems();
    }

  }
  close(): any {
  }

  delete(param?: any): any {
  }

  async get(Id: number) {

  }

  async update() {

  }
  async edit() {
  }
}

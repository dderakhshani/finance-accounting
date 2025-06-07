import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { TableConfigurations} from 'src/app/core/components/custom/table/models/table-configurations';
import { ConfirmDialogComponent, ConfirmDialogIcons } from 'src/app/core/components/material-design/confirm-dialog/confirm-dialog.component';
import { FormActionTypes } from 'src/app/core/constants/form-action-types';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { AccountReference } from 'src/app/modules/bursary/entities/account-reference';
import { AccountHeads, AccountReferencesGroupEnums, ReceiptInsertedByCustomerStatus } from 'src/app/modules/bursary/entities/enums';
import { ReceiptsInsertedByCustomers } from 'src/app/modules/bursary/entities/receipts-inserted-by-customers';
import { CreateCustomerReceiptCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/create-customer-receipt-command';
import { CreateFinancialAttachmentCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/create-financial-attachment-command';
import { CreateReceiptCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/create-receipt-command';
import { UpdateStatusReceiptInsertedByCustomersCommand } from 'src/app/modules/bursary/repositories/financial-request/receipts-list-inserted-by-customers/commands/update-status-receipt-inserted-by-customers-command';
import { GetReceiptsInsertedByCustomersQuery } from 'src/app/modules/bursary/repositories/financial-request/receipts-list-inserted-by-customers/queries/get-receipts-inserted-by-customers-query';
import {  GetBursaryAccountReferencesQuery } from 'src/app/modules/bursary/repositories/financial-request/referenceAccount/queries/get-account-references-query';
import { UploadAttachmentCommand } from 'src/app/shared/repositories/attachments/upload-attachment-command';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';
import { environment } from 'src/environments/environment';
import { AttachmentDialogComponent } from './attachment-dialog/attachment-dialog.component';
import { RemoveReceiptWithMessageDialogComponent } from './remove-receipt-with-message-dialog/remove-receipt-with-message-dialog.component';
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-receipts-list-inserted-by-customers',
  templateUrl: './receipts-list-inserted-by-customers.component.html',
  styleUrls: ['./receipts-list-inserted-by-customers.component.scss']
})
export class ReceiptsListInsertedByCustomersComponent extends BaseComponent {

  @ViewChild('buttonShowAttached', { read: TemplateRef }) buttonShowAttached!: TemplateRef<any>;
  tableConfigurations!: TableConfigurations;
  receipts: ReceiptsInsertedByCustomers[] = []
  public accountHeadEnums = AccountHeads;
  public AccountReferencesGroupEnums = AccountReferencesGroupEnums;

  constructor(private _mediator: Mediator,
    private route: ActivatedRoute,
    private router: Router, public dialog: MatDialog) {
    super(route, router);
  }

  async ngOnInit() {
    await this.resolve();
  }
  ngAfterViewInit(): void {
    this.resolve();
  }

  async addReceiptsToBursary(item: FormGroup) {


    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید ثبت در خزانه داری',
        message:'آیا از ثبت این سند مطمئن هستید ؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {


      if (result == true) {

        var image=await fetch(environment.crmServerAddress+"/"+item.value.imageUrl).then(res=>res.blob());

        let fileAddress =  await this._mediator.send(new UploadAttachmentCommand(new File([image], "financialRequestFile."+image.type.split("/")[1])));
        let financialAttachment = new CreateFinancialAttachmentCommand();
        financialAttachment.addressUrl = fileAddress;

        let selectedDocument = item.value;
        let accountReferences: AccountReference[] = [];

        var accountReferenceFillter = [new SearchQuery({
          propertyName: 'code',
          comparison: 'equal',
          values: [selectedDocument.bankAccountCode],
        }),
        new SearchQuery({
          propertyName: 'code',
          comparison: 'equal',
          values: [selectedDocument.customerAccountCode],
        })];

        await this._mediator.send(new GetBursaryAccountReferencesQuery(0, 10, accountReferenceFillter)).then(x => accountReferences = x.data);

      //  await this._mediator.send(new GetAccountReferencesQuery(0, 10, accountReferenceFillter)).then(x => accountReferences = x.data);


        var command = new CreateCustomerReceiptCommand();
        command.codeVoucherGroupId = 2259;
        command.description = "Added By Receipt System :"+selectedDocument.id;
        command.documentDate = <Date>selectedDocument.paymentDate;
        var detail = new CreateReceiptCommand();
        detail.amount = selectedDocument.amount;
        detail.creditAccountHeadId = this.accountHeadEnums.AccountHeadCode_2304;
        detail.creditAccountReferenceGroupId = this.AccountReferencesGroupEnums.AccountReferencesGroupCode_31;
        detail.creditAccountReferenceId = accountReferences.filter((x: any) => x.code == selectedDocument.customerAccountCode)[0].id;
        detail.debitAccountHeadId = this.accountHeadEnums.AccountHeadCode_2601;
        detail.debitAccountReferenceGroupId = this.AccountReferencesGroupEnums.AccountReferencesGroupCode_02;
        detail.debitAccountReferenceId = accountReferences.filter((x: any) => x.code == selectedDocument.bankAccountCode)[0].id;
        detail.description = "Automatic Bursary Document";
        detail.documentTypeBaseId = 28509;
        detail.financialReferenceTypeBaseId = 28516;
        detail.isRial = true;

        command.attachments.push(financialAttachment);


        (<CreateCustomerReceiptCommand>command).financialRequestDetails.push(detail);
        await this._mediator.send(<CreateCustomerReceiptCommand>command);

        var updateCustomerDocument = new UpdateStatusReceiptInsertedByCustomersCommand(<number>selectedDocument.id);
        updateCustomerDocument.status = ReceiptInsertedByCustomerStatus.AddToBursary;
        updateCustomerDocument.description = "Add To Bursary";
        let res = await this._mediator.send(<UpdateStatusReceiptInsertedByCustomersCommand>updateCustomerDocument);
        if (res) this.get();
      }
    });




  }

  showReceiptAttached(item: any) {
    const dialogRef = this.dialog.open(AttachmentDialogComponent, {
      width: '80%',
      height: '80%',
      data: { data: JSON.stringify(item.value.imageUrl) },
    });

    dialogRef.afterClosed().subscribe(result => { });
  }


  async resolve() {

    this.formActions = [
      FormActionTypes.edit,
      FormActionTypes.refresh,
      FormActionTypes.delete
    ];

    let colButtonShowAttached = new TableColumn(
      'showAttached',
      'مشاهده پیوست',
      TableColumnDataType.Template,
      '25%',
      true,
    );

    colButtonShowAttached.template = this.buttonShowAttached;

    let columns: TableColumn[] = [

      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '1.5%'),
      new TableColumn(
        "bankName", "نام بانک",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter(
          "bankName",
          TableColumnFilterTypes.Text,
        )
      ),
      new TableColumn(
        "accountNumber",
        "شماره حساب ",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("accountNumber", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "customerAccountCode",
        "کد مشتری",
        TableColumnDataType.Text,
        "10%",
        true,
        new TableColumnFilter("customerAccountCode", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "customerName",
        "نام مشتری",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("customerName", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "amount",
        "مبلغ",
        TableColumnDataType.Money,
        "15%",
        true,
        new TableColumnFilter("amount", TableColumnFilterTypes.Money)
      ),
      //,

      new TableColumn(
        "trackingNumber",
        "شماره پیگیری",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("trackingNumber", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "paymentDate",
        "تاریخ پرداخت",
        TableColumnDataType.Date,
        "15%",
        true,
        new TableColumnFilter("paymentDate", TableColumnFilterTypes.Date)
      ),
      colButtonShowAttached,

    ];
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, '', true));

    await this.get()
  }
  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
  async get(param?: any) {

    var response = await this._mediator.send(new GetReceiptsInsertedByCustomersQuery());
    this.receipts = response;
    this.form = new FormArray(this.receipts.map((x) => this.createForm(x)));
    response.length && (this.tableConfigurations.pagination.totalItems = response.length);

  }

  update(param?: any) {
    throw new Error('Method not implemented.');
  }
  delete(param?: any) {
    throw new Error('Method not implemented.');
  }
  close() {
    throw new Error('Method not implemented.');
  }


  removeReceipt(item:FormGroup){
    let receipt = item.value;

    const dialogRef = this.dialog.open(RemoveReceiptWithMessageDialogComponent, {
      width: '20%',
      height: '30%',
      data: { data: JSON.stringify(receipt.id) },
    });

    dialogRef.afterClosed().subscribe(result => {
      this.get();
    });

  }


}

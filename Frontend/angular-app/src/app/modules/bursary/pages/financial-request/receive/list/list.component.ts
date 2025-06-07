
import { Component, Pipe, TemplateRef, ViewChild } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { TableConfigurations } from 'src/app/core/components/custom/table/models/table-configurations';
import { ConfirmDialogComponent, ConfirmDialogIcons } from 'src/app/core/components/material-design/confirm-dialog/confirm-dialog.component';
import { FormActionTypes } from 'src/app/core/constants/form-action-types';
import { PageModes } from 'src/app/core/enums/page-modes';
import { FormAction } from 'src/app/core/models/form-action';
import { LoaderService } from 'src/app/core/services/loader.service';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { AccountingDocument } from 'src/app/modules/bursary/entities/accounting-document';
import { DocumentHead } from 'src/app/modules/bursary/entities/document-head';
import { AccountReferencesGroupEnums, ReceiptInsertedByCustomerStatus } from 'src/app/modules/bursary/entities/enums';
import { FinancialRequest } from 'src/app/modules/bursary/entities/financial-request';
import { RequestPayment } from 'src/app/modules/bursary/entities/request-payment';
import { CreateAccountingDocumentCommand } from 'src/app/modules/bursary/repositories/accounting-document/commands/create-accounting-document-command';
import { UpdateCustomerReceiptCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/update-customer-receipt-command';
import { UpdateIsPendingCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/update-isPending-command';
import { UpdateUpdateVoucherHeadIdCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/update-voucherHead-id-command';
import { DeleteCustomerReceiptCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receive/add-receive/commands/delete-customer-receipt-command';
import { UpdateStatusReceiptInsertedByCustomersCommand } from 'src/app/modules/bursary/repositories/financial-request/receipts-list-inserted-by-customers/commands/update-status-receipt-inserted-by-customers-command';
import { GetRequestPaymentsQuery } from 'src/app/modules/bursary/repositories/financial-request/request-payments/queries/get-request-payments-query';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';
import { DetailComponent } from './detail/detail.component';
import { ShowAttachmentComponent } from './show-attachment/show-attachment.component';
import { number } from 'echarts';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../../core/components/custom/table/models/table-column";
import * as moment from "jalali-moment";
import { GetAttachmentsQuery } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/queries/get-attachments-query';
import { PagesCommonService } from 'src/app/shared/services/pages/pages-common.service';
import { MoneyPipe } from 'src/app/core/pipes/money.pipe';
import { IdentityService } from 'src/app/modules/identity/repositories/identity.service';
import { TablePaginationOptions } from 'src/app/core/components/custom/table/models/table-pagination-options';
import { BankBalanceDialogComponent } from './bank-balance-dialog/bank-balance-dialog.component';
import { GetReqeustCountByStatusQuery } from 'src/app/modules/bursary/repositories/financial-request/request-payments/queries/get-request-status-status-query';

@Component({
  selector: 'app-bursary-documents',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends BaseComponent {

  @ViewChild('buttonShowDetails', { read: TemplateRef }) buttonShowDetails!: TemplateRef<any>;
  @ViewChild('bursaryDocumentsNumber', { read: TemplateRef }) bursaryDocumentsNumber!: TemplateRef<any>;
  //  @ViewChild('editDocument', { read: TemplateRef }) editDocument!: TemplateRef<any>;

  financialRequests: RequestPayment[] = [];
  checkDateDocuments: RequestPayment[] = [];
  pendingCount : number = 0;
  entryMode: string = "1";
  automateSate: number = 4;

  listType: number = 1;
  filterSum: number = 0;
  sum: number = 0;
  includedRows: any = [];
  excludedRows: any = [];


  public AccountReferencesGroupEnums = AccountReferencesGroupEnums;
  tableConfigurations!: TableConfigurations;

  constructor(private _mediator: Mediator, private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router, public dialog: MatDialog,
    public loaderService: LoaderService, public printService: PagesCommonService, private pipe: MoneyPipe, private identityService: IdentityService) {
    super(route, router);
  }

  async ngOnInit() {
  }

  async ngAfterViewInit() {
    await this.resolve();
  }

  async resolve() {

    this.formActions = [

      FormActionTypes.refresh,
      FormActionTypes.delete
    ];

    let colButtonShowDetails = new TableColumn(
      'showDetails',
      'مشاهده جزئیات',
      TableColumnDataType.Template,
      '30%',
      true,
    );

    // let colBursaryDocumentsNumber = new TableColumn(
    //   'documentNo',
    //   'شماره خزانه',
    //   TableColumnDataType.Template,
    //   '15%',
    //   true,
    //   new TableColumnFilter("documentNo", TableColumnFilterTypes.Number)
    // );

    // let colEditDocument = new TableColumn(
    //   'editDocument',
    //   'ویرایش',
    //   TableColumnDataType.Template,
    //   '15%',
    //   true,
    // );

    colButtonShowDetails.template = this.buttonShowDetails;

    // colEditDocument.template = this.editDocument;


    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),

      //colEditDocument,

      new TableColumn(
        'documentNo',
        'شماره خزانه',
        TableColumnDataType.Number,
        '15%',
        true,
        new TableColumnFilter("documentNo", TableColumnFilterTypes.Number)
      ),

      new TableColumn(
        'voucherHeadCode',
        'شماره سند ',
        TableColumnDataType.Number,
        '15%',
        true,
        new TableColumnFilter("voucherHeadCode", TableColumnFilterTypes.Number)
      ),


      new TableColumn(
        "documentDate",
        "تاریخ ",
        TableColumnDataType.Date,
        "15%",
        true,
        new TableColumnFilter("documentDate", TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        "detailDescription",
        "توضیحات",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("detailDescription", TableColumnFilterTypes.Text)
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
        "creditAccountReferenceTitle",
        "واریز کننده",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("creditAccountReferenceTitle", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "creditAccountReferenceGroupTitle",
        "نوع مشتری",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("creditAccountReferenceGroupTitle", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "creditAccountReferenceCode",
        "کد واریز کننده",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("creditAccountReferenceCode", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "debitAccountReferenceTitle",
        "دریافت کننده",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("debitAccountReferenceTitle", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "documentTypeBaseTitle",
        "نوع پرداخت",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("documentTypeBaseTitle", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "createName",
        "ثبت کننده",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("createName", TableColumnFilterTypes.Text)
      ),

      // new TableColumn(
      //   "voucherHeadCode",
      //   "شماره سند",
      //   TableColumnDataType.Number,
      //   "15%",
      //   true,
      //   new TableColumnFilter("voucherHeadCode", TableColumnFilterTypes.Number)
      // ),

      colButtonShowDetails,
    ];
    var config = new TablePaginationOptions();
    config.pageIndex = 0;
    config.pageSize = 200;
    config.totalItems = 0;
    config.pageSizeOptions = [200, 500, 1000];


    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, ''), config);
    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.options.exportOptions.customExportButtonTitle = 'خروجی تدبیر';
    this.tableConfigurations.options.exportOptions.customExportCallbackFn = ((rows: any[], columns: TableColumn[]) => {
      return rows.map((x: any) => {

        var bankCode = x.debitAccountReferenceCode.slice(1);
        bankCode = "02" + bankCode;
        //  let debitCode = x.financialRequestDetails[0].debitAccountReferenceCode.substring(0,5);

        let item: any = {};
        item['شماره پرداخت'] = x.documentNo;
        item['تاریخ پرداخت'] = moment(moment.utc(<Date>(new Date(x.documentDate))).toDate()).locale('fa').format('jYYYY/jMM/jDD');
        item['شرح پرداخت'] = "-" + x.description;
        item['مبلغ'] = x.amount;
        item['کد حساب پرداخت کننده'] = x.creditAccountReferenceGroupCode == "31" ? "04001" : x.creditAccountReferenceGroupCode;
        item['شناورپرداخت کننده'] = x.creditAccountReferenceCode
        item['مرکز هزینه دریافت کننده'] = 0;
        item['پروژه دریافت کننده'] = 0;
        item['کد حساب صندوق یا بانک'] = x.debitAccountReferenceGroupId == AccountReferencesGroupEnums.AccountReferencesGroupCode_06004 ? "06004" : bankCode;
        item['شناور بانک یا صندوق'] = bankCode;
        item['مرکز هزینه صندوق یا بانک'] = 0;
        item['پروژه صندوق یا بانک'] = 0;

        return item;
      });
    }).bind(this)

    await this.get()
  }

  initialize(params?: any) {

    
    throw new Error('Method not implemented.');
  }
  add(param?: any) {

  }

  async get(param?: any) {
    var me = this;
    me.isLoading = true;

    // به‌روزرسانی includedRows با استفاده از excludedRows
    function updateIncludedRows(includedRows: SearchQuery[], excludedRows: SearchQuery[]): SearchQuery[] {
      return includedRows.map(includedRow => {
        const excludedRow = excludedRows.find(excludedRow =>
          excludedRow.propertyName === includedRow.propertyName &&
          excludedRow.comparison === includedRow.comparison
        );

        if (excludedRow) {
          return {
            ...includedRow,
            values: includedRow.values.filter(value => !excludedRow.values.includes(value))
          };
        }

        return includedRow;
      }).filter(row => row.values.length > 0); // حذف ردیف‌های خالی
    }

    me.includedRows = updateIncludedRows(me.includedRows, me.excludedRows);

    let searchQueries: SearchQuery[] = JSON.parse(JSON.stringify(me.includedRows));
    searchQueries.push(...JSON.parse(JSON.stringify(me.excludedRows)));

    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
          comparison: filter.searchCondition,
          nextOperand: filter.nextOperand
        }));
      });
    }

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key;
      });
    }


    if (this.automateSate == 4) {
      searchQueries.push(new SearchQuery({
        propertyName: 'automateState',
        values: [0],
        comparison:'equal',
      nextOperand: 'or'
      }));

      searchQueries.push(new SearchQuery({
        propertyName: 'automateState',
        values: [1],
        comparison:'equal',
      nextOperand: 'and'
      }));

    } else {
      searchQueries.push(new SearchQuery({
        propertyName: 'automateState',
        values: [this.automateSate],
        comparison:'equal',
       nextOperand: 'and'
      }));
    }

   // 
    if (this.entryMode != "2" && this.automateSate != 3) {
      searchQueries.push(new SearchQuery({
        propertyName: 'voucherHeadId',
        values: ["null"],
        comparison: this.entryMode == "0" ? 'equal' : 'notEqual'
      }));
    }




    try {
      var response = await this._mediator.send(new GetRequestPaymentsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty));
      this.isLoading = false;
      this.financialRequests = response.data;
      this.sum = response.data.reduce((total, item) => {
        return total + item.amount;
      }, 0);

      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
      this.filterSum = response.totalSum;

    } catch {
      this.isLoading = false;
    }


    this.pendingCount = await this._mediator.send(new GetReqeustCountByStatusQuery(3));
    console.log(this.pendingCount);
  }

  handleincludeOnlySelectedItemsEvent(ids: number[]) {
    this.includedRows = [new SearchQuery({
      propertyName: 'id',
      values: ids,
      comparison: 'ofList',
      nextOperand: 'and'
    })];

    this.excludedRows = []; // اضافه کردن این خط برای اطمینان از پاک شدن excludedRows
    this.get();
  }

  handleClearSelectedItemsEvent() {
    this.includedRows = [];
    this.excludedRows = [];
    this.get();
  }

  handleExcludeSelectedItemsEvent(ids: number[]) {

    // اطمینان از اینکه مقادیر جدید به excludedRows اضافه می‌شوند
    this.excludedRows.push(new SearchQuery({
      propertyName: 'id',
      values: ids,
      comparison: 'notIn',
      nextOperand: 'and'
    }));

    this.get();
  }

  showFinancialDetails(item: any) {

    const dialogRef = this.dialog.open(DetailComponent, {
      width: '80%',
      height: '80%',
      data: { data: JSON.stringify(item.financialRequestDetails) },
    });

    dialogRef.afterClosed().subscribe(result => {
    });
  }

  update(param: FinancialRequest) {
  }

  navigateToAddReceipt(financialRequest?: RequestPayment) {

    let isPermission = this.identityService.applicationUser.id == 1509; // Nematolahi //---> TODO: Permission must set with roles


    if (isPermission) {
      this.notificationService.showFailureMessage("شما دسترسی به ویرایش این سند ندارید");
      return;
    }
    //TODO: IF voucherStateId == 2 --> Correction Request Else IF voucherStateId == 3 --> Document Is Close
    if (financialRequest?.voucherStateId != null && financialRequest?.voucherStateId > 1) {
      this.notificationService.showFailureMessage("سند حسابداری مورد نظر شما بسته شده است است لطفا با مسئول حسابداری تماس بگیرید", 0);
      return;
    }


    if (!financialRequest)
      financialRequest = this.financialRequests.filter((x: any) => x.id)[0];
    if (financialRequest)
      this.router.navigateByUrl(`bursary/customerReceive/customerReceipt?id=${financialRequest.id}`)

  }

  async delete(param?: any) {

    let isPermission = this.identityService.applicationUser.id == 1509; // Nematolahi //---> TODO: Permission must set with roles


    if (isPermission) {
      this.notificationService.showFailureMessage("شما دسترسی به حذف این سند ندارید");
      return;
    }


    let selectedDocuments = this.financialRequests.filter((x: any) => x.selected);

    if (selectedDocuments == null || selectedDocuments.length == 0)
      return this.notificationService.showFailureMessage("هیچ ایتمی برای حذف انتخاب نشده است", 0)

    const hasVoucherHead: boolean = selectedDocuments.some((item: RequestPayment) => {
      return item.voucherHeadId != null;
    });



    selectedDocuments.some((item: RequestPayment) => {

      if (item?.voucherStateId != null && item?.voucherStateId > 1) {
        this.notificationService.showFailureMessage(`سند حسابداری مورد نظر شما برای دریافت ${item.documentNo}بسته شده است است لطفا با مسئول حسابداری تماس بگیرید`, 0);
        return;
      }

    });

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف',
        message: 'آیا از حذف این سند مطمئن هستید ؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });



    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        let deleteDocument = new DeleteCustomerReceiptCommand();
        let numbers = selectedDocuments.map(item => item.id);
        deleteDocument.ids = ((numbers as number[]))
        this.isLoading = true;
        await this._mediator.send(<DeleteCustomerReceiptCommand>deleteDocument);
        await this.get()
        this.isLoading = false;
      }
    });

  }

  close() {
    throw new Error('Method not implemented.');
  }
 
  async getBankBalance(){

   

      const dialogRef = this.dialog.open(BankBalanceDialogComponent, {
        width: '90%',
        height: '90%',
      
      });
  
      dialogRef.afterClosed().subscribe(async item => {
  
  
      });
   
  }

  async addAccountingDocuments() {

    let selectedDocuments = this.financialRequests.filter((x: any) => x.selected);

    if (selectedDocuments == null || selectedDocuments.length == 0)
      return this.notificationService.showFailureMessage("هیچ ایتمی برای ثبت سند انتخاب نشده است", 0)

    const firstDocumentDate = this.stripTimeFromDate(selectedDocuments[0].documentDate as Date);
    let hasMismatch = false;

    for (const item of selectedDocuments) {
      if (this.stripTimeFromDate(item.documentDate as Date).getTime() !== firstDocumentDate.getTime()) {
        hasMismatch = true;
        break;
      }
    }

    if (hasMismatch) {
      this.notificationService.showFailureMessage("انتخاب تاریخ های متفاوت جهت ثبت سند مجاز نمی باشد", 0);
      hasMismatch = false;
      return;
    }



    let accountingDoc = new CreateAccountingDocumentCommand();

    selectedDocuments.forEach((document: any) => {


      let ad !: AccountingDocument;
      const obj = ad || {};

      obj.DocumentNo = document.documentNo;
      obj.CodeVoucherGroupId = document.codeVoucherGroupId;
      obj.Amount = document.amount ?? document.chequeSheet?.totalCost;
      obj.CreditAccountHeadId = document.creditAccountHeadId;
      obj.CreditAccountReferenceGroupId = document.creditAccountReferenceGroupId == undefined ? null : document.creditAccountReferenceGroupId;;
      obj.CreditAccountReferenceId = document.creditAccountReferenceId == undefined ? null : document.creditAccountReferenceId;
      obj.ReferenceName = " " + document.creditAccountReferenceTitle + " ",
        obj.ReferenceCode = document.creditAccountReferenceCode,
        obj.DebitAccountHeadId = document.debitAccountHeadId;
      obj.DebitAccountReferenceGroupId = document.debitAccountReferenceGroupId == undefined ? null : document.debitAccountReferenceGroupId;
      obj.DebitAccountReferenceId = document.debitAccountReferenceId == undefined ? null : document.debitAccountReferenceId;;
      obj.DocumentDate = document.documentDate;
      obj.DocumentId = document.id;
      obj.DocumentTypeBaseId = document.documentTypeBaseId;
      obj.SheetUniqueNumber = document?.sheetUniqueNumber;
      obj.CurrencyAmount = document.currencyAmount ?? 0;
      obj.IsRial = document.nonRialStatus != null && document.nonRialStatus > 0 ? false : true;
      obj.CurrencyFee = obj.IsRial == true ? 0 : document.currencyFee;
      obj.CurrencyTypeBaseId = document.currencyTypeBaseId;
      obj.NonRialStatus = document.nonRialStatus ?? 0;
      obj.SheetUniqueNumber = document.chequeSheetId != null && document.chequeSheetId != 0 ? document?.sheeSeqNumber : "0";
      obj.ChequeSheetId = document.chequeSheetId;
      obj.Description = document.description;
      obj.BesCurrencyStatus = document.besCurrencyStatus;
      obj.BedCurrencyStatus = document.bedCurrencyStatus;
      accountingDoc.dataList.push(obj);
    });



    this.isLoading = true;
    // pending
    //let isPendingRequest = new UpdateIsPendingCommand();
    let selectedList = this.financialRequests.filter((x: any) => x.selected);


    let result;
    try {
      result = await this._mediator.send(<CreateAccountingDocumentCommand>accountingDoc);
    } catch {
      this.isLoading = false;
    }
    let voucherHeadIdRequest = new UpdateUpdateVoucherHeadIdCommand();

    selectedList.forEach((item: any) => {
      voucherHeadIdRequest.receiveIds.push(item.id)
    });


    // get the voucherHeadId
    voucherHeadIdRequest.voucherHeadId = (<any>result)[0].voucherHeadId;

    //  update voucherHeadId and isPending = false

    var isVoucherHeadId;
    try {
      isVoucherHeadId = await this._mediator.send(<UpdateUpdateVoucherHeadIdCommand>voucherHeadIdRequest);
    } catch {
      this.isLoading = false;
    }
    if (isVoucherHeadId)
      this.get();



    this.isLoading = false;

    //  }

  }

  stripTimeFromDate(date: Date): Date {
    const strippedDate = new Date(date);
    strippedDate.setHours(0, 0, 0, 0);
    return strippedDate;
  }


  removeDocument(item: FormGroup) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف',
        message: 'آیا از حذف این سند مطمئن هستید ؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {
        let receipt = item.value;

        if (receipt.description.startsWith('Added By Receipt System')) {
          let receiptCustomerId = receipt.description.split(":")[1];
          var updateCustomerDocument = new UpdateStatusReceiptInsertedByCustomersCommand(<number>receiptCustomerId);
          updateCustomerDocument.status = ReceiptInsertedByCustomerStatus.Remove;
          updateCustomerDocument.description = "Removed By Sale Accounting";
          await this._mediator.send(<UpdateStatusReceiptInsertedByCustomersCommand>updateCustomerDocument);
        }

        var deleteFinancialRequest = new DeleteCustomerReceiptCommand();
        deleteFinancialRequest.ids.push(receipt.id);
        await this._mediator.send(<DeleteCustomerReceiptCommand>deleteFinancialRequest);
        this.get();
      }
    });
  }


  async showReceiptAttached(item: any) {

    var response = await this._mediator.send(new GetAttachmentsQuery(item.id));


    const dialogRef = this.dialog.open(ShowAttachmentComponent, {
      width: '80%',
      height: '80%',
      data: { data: JSON.stringify(response.data) },
    });

    dialogRef.afterClosed().subscribe(result => {
    });
  }

  getData(item: FormGroup) {

  }

  filterSelectedItem() {
    this.financialRequests = this.financialRequests.filter((x: any) => x.selected);



    this.sumSelected(this.financialRequests);
    // this.form = new FormArray(this.financialRequests.map((x) => this.createForm(x)));
    // this.calcSelectedSum();
  }


  filterSelectedInverseItem() {
    this.financialRequests = this.financialRequests.filter((x: any) => !x.selected);

    this.sumSelected(this.financialRequests);
    // this.form = new FormArray(this.financialRequests.map((x) => this.createForm(x)));
    // this.calcSelectedSum();
  }


  sumSelected(selectedItems: RequestPayment[]) {

    let count = selectedItems.length;

    // this.filterSum = selectedItems.reduce((total, item) => {
    //   return total + item.amount;
    // }, 0);

    count && (this.tableConfigurations.pagination.totalItems = count);
  }

  showRelatedDocument() {

    let isPermission = this.identityService.applicationUser.id == 1509; // Nematolahi //---> TODO: Permission must set with roles


    if (isPermission) {
      this.notificationService.showFailureMessage("شما دسترسی به مشاهده این سند ندارید");
      return;
    }

    let items = this.financialRequests.filter((x: any) => x.selected);

    if ((items[0].voucherHeadId as number) > 0)
      this.router.navigateByUrl(`/accounting/voucherHead/add?id=${items[0].voucherHeadId}`)
    else
      this.notificationService.showFailureMessage("سندی برای ایتم انتخاب شده وجود ندارد", 0);
  }


  async currencyPrint() {

    let printContents = '';
    let selectedDocuments = this.financialRequests.filter((x: any) => x.selected);
    if (selectedDocuments.length >  1)  
    {
      this.notificationService.showFailureMessage("انتخاب بیش از یک آرتیکل برای پرینت ارزی مجاز نمی باشد", 0);
      return;
    }
    if (selectedDocuments.length ==  0)  
      {
        this.notificationService.showFailureMessage("برای پرینت ارزی باید یک ایتم را انتخاب کنید", 0);
        return;
      }


      if (selectedDocuments[0].currencyAmount ==  undefined || selectedDocuments[0].currencyAmount ==  null || selectedDocuments[0].currencyAmount ==  0 )  
        {
          this.notificationService.showFailureMessage("برای پرینت ارزی باید یک آیتم ارزی را انتخاب کنید", 0);
          return;
        }


      const datetime = this.printService.toPersianDate(selectedDocuments[0].documentDate as Date)

      printContents += `
<div style="font-family: Tahoma, Arial, sans-serif; direction: rtl; font-size: 12px; border: 2px solid black; padding: 20px;">

  <div style="width: 100%; display: flex; justify-content: space-between; margin-bottom: 10px;">
    <div style="border: 1px solid black; width: 30%; padding: 15px; text-align: center;">
        شرکت ایفاسرام <br> سیستم مدیریت یکپارچه
    </div>
    <div style="border: 1px solid black; width: 40%; padding: 15px; text-align: center;">
        فرم دریافت ارز حاصل از صادرات
    </div>
    <div style="border: 1px solid black; width: 30%; padding: 15px; text-align: center;">
        تاریخ: ${datetime.split(' ')[1]} <br> شماره:  ${selectedDocuments[0].documentNo}  
    </div>
  </div>

  <div style="border: 1px solid black; padding: 15px; width: 95%; margin-bottom: 10px;">
    <div style="text-align: right; margin-bottom: 10px;">
          آقای / خانم / فروشگاه مشتریان صادراتی : <span style="border-bottom: 1px dashed black; padding: 0 20px;">${selectedDocuments[0].creditAccountReferenceTitle}</span> کد حساب : <span style="border-bottom: 1px dashed black; padding: 0 20px;"> ${selectedDocuments[0].creditAccountReferenceCode} </span>
    </div>
    <div style="text-align: right; margin-bottom: 10px;">
        احتراما مبلغ <span style="border-bottom: 1px dashed black; padding: 0 20px;">${this.pipe.transform(selectedDocuments[0].currencyAmount ?? 0)} </span>  <span style="border-bottom: 1px dashed black; padding: 0 20px;">${selectedDocuments[0].currencyTypeBaseTitle}</span>    به نرخ <span style="border-bottom: 1px dashed black; padding: 0 20px;">${this.pipe.transform(selectedDocuments[0].currencyFee ?? 0)}</span> ریال  معادل <span style="border-bottom: 1px dashed black; padding: 0 20px;">${this.pipe.transform(selectedDocuments[0].amount)} </span>  ریال
    </div>
    <div style="text-align: right; margin-bottom: 10px;">
        بابت دریافت ارز بر اساس مقررات بانک مرکزی (سامانه های اعلامی) به بستانکار حساب شما لحاظ گردید.
    </div>
    <div style="text-align: right; margin-bottom: 10px;">
        توضیحات:
        </br>
        نرخ ارز هنگام صدور رسید  توافق و بسته شده و به صورت ریالی در بستانکاری حساب شما اعمال گردیده است و به هیچ عنوان بع از این تاریخ به ارز تبدیل نمیشود و در صورت انصراف  معادل ریالی به پرداخت کننده عودت داده می شود . پرداخت کننده می بایست مطلع باشد که طبق قوانین جاری ایران ارز تبدیل شده به ریال دوباره به ارز تبدیل نمیشود و پرداخت با علم به این قوانین انجام گردیده است
    </div>
  </div>

  <div style="width: 100%; display: flex; justify-content: space-between; margin-top: 10px;">
    <div style="border: 1px solid black; padding: 15px; text-align: center; width: 30%;height:45px;">
        حسابداری فروش  
    </div>
    <div style="border: 1px solid black; padding: 15px; text-align: center; width: 40%;height:45px;">
        واحد مالی  
    </div>
    <div style="border: 1px solid black; padding: 15px; text-align: center; width: 30%;height: 45px;">
        مدیر سیستم  
    </div>
  </div>

</div>`;
    
      this.printService.onPrint(printContents, `  دریافت از مشتری`)
  
}



  async currencyExchangePrint() {

    let printContents = '';
    let selectedDocuments = this.financialRequests.filter((x: any) => x.selected);
    if (selectedDocuments.length >  1)  
    {
      this.notificationService.showFailureMessage("انتخاب بیش از یک آرتیکل برای پرینت ارزی مجاز نمی باشد", 0);
      return;
    }
    if (selectedDocuments.length ==  0)  
      {
        this.notificationService.showFailureMessage("برای پرینت ارزی باید یک ایتم را انتخاب کنید", 0);
        return;
      }

      if (selectedDocuments[0].currencyAmount ==  undefined || selectedDocuments[0].currencyAmount ==  null || selectedDocuments[0].currencyAmount ==  0 )  
        {
          this.notificationService.showFailureMessage("برای پرینت ارزی باید یک آیتم ارزی را انتخاب کنید", 0);
          return;
        }


      const datetime = this.printService.toPersianDate(selectedDocuments[0].documentDate as Date)

      printContents += `
<div style="font-family: Tahoma, Arial, sans-serif; direction: rtl; font-size: 12px; border: 2px solid black; padding: 20px;">

  <div style="width: 100%; display: flex; justify-content: space-between; margin-bottom: 10px;">
    <div style="border: 1px solid black; width: 30%; padding: 15px; text-align: center;">
        شرکت ایفاسرام <br> سیستم مدیریت یکپارچه
    </div>
    <div style="border: 1px solid black; width: 40%; padding: 15px; text-align: center;">
        فرم دریافت ارز حاصل از صادرات
    </div>
    <div style="border: 1px solid black; width: 30%; padding: 15px; text-align: center;">
        تاریخ: ${datetime.split(' ')[1]} <br> شماره:  ${selectedDocuments[0].documentNo}  
    </div>
  </div>

  <div style="border: 1px solid black; padding: 15px; width: 95%; margin-bottom: 10px;">
    <div style="text-align: right; margin-bottom: 10px;">
          آقای / خانم / فروشگاه مشتریان صادراتی : <span style="border-bottom: 1px dashed black; padding: 0 20px;">${selectedDocuments[0].creditAccountReferenceTitle}</span> کد حساب : <span style="border-bottom: 1px dashed black; padding: 0 20px;"> ${selectedDocuments[0].creditAccountReferenceCode} </span>
    </div>
    <div style="text-align: right; margin-bottom: 10px;">
        احتراما مبلغ <span style="border-bottom: 1px dashed black; padding: 0 20px;">${this.pipe.transform(selectedDocuments[0].currencyAmount ?? 0)} </span>  <span style="border-bottom: 1px dashed black; padding: 0 20px;">${selectedDocuments[0].currencyTypeBaseTitle}</span>    به نرخ <span style="border-bottom: 1px dashed black; padding: 0 20px;">---</span> ریال  معادل <span style="border-bottom: 1px dashed black; padding: 0 20px;">---</span>  ریال
    </div>
    <div style="text-align: right; margin-bottom: 10px;">
        بابت دریافت ارز بر اساس مقررات بانک مرکزی (سامانه های اعلامی) به بستانکار حساب شما لحاظ گردید.
    </div>
    <div style="text-align: right; margin-bottom: 10px;">
        توضیحات:
        </br>
        نرخ ارز هنگام صدور رسید  توافق و بسته شده و به صورت ریالی در بستانکاری حساب شما اعمال گردیده است و به هیچ عنوان بع از این تاریخ به ارز تبدیل نمیشود و در صورت انصراف  معادل ریالی به پرداخت کننده عودت داده می شود . پرداخت کننده می بایست مطلع باشد که طبق قوانین جاری ایران ارز تبدیل شده به ریال دوباره به ارز تبدیل نمیشود و پرداخت با علم به این قوانین انجام گردیده است
    </div>
  </div>

  <div style="width: 100%; display: flex; justify-content: space-between; margin-top: 10px;">
    <div style="border: 1px solid black; padding: 15px; text-align: center; width: 30%;height:45px;">
        حسابداری فروش  </br> خانم پناهی
    </div>
    <div style="border: 1px solid black; padding: 15px; text-align: center; width: 40%;height:45px;">
        واحد مالی  
    </div>
    <div style="border: 1px solid black; padding: 15px; text-align: center; width: 30%;height: 45px;">
        مدیر سیستم  
        </br> خانم پرنیان پور
    </div>
  </div>

</div>`;
    
      this.printService.onPrint(printContents, `  دریافت از مشتری`)
  }

  async print() {

    let printContents = '';
    if (this.financialRequests.length > 0) {
      const sum = this.financialRequests.reduce((acc, item) => acc + item.amount, 0);

      printContents += `<table><thead>
                     <tr>
                       <th>ردیف</th>
                       <th>شماره سند</th>
                       <th>تاریخ </th>
                       <th>مبلغ</th>
                       <th>واریز کننده</th>
                       <th>دریافت کننده</th>
                       <th>توضیحات</th>
                     </tr>
                   </thead><tbody>`;
      this.financialRequests.map(data => {

        const datetime = this.printService.toPersianDate(data.documentDate as Date)
        printContents += `<tr>
                            <td>${data.documentNo}</td>
                           <td>${data.voucherHeadCode}</td>
                           <td>${datetime.split(' ')[1]}</td>
                           <td>${this.pipe.transform(data.amount)}</td>
                           <td>${data.creditAccountReferenceTitle}</td>
                           <td>${data.debitAccountReferenceTitle}</td>
                           <td>${data.description}</td>

                        </tr>`;
      });
      const money = this.pipe.transform(sum);
      printContents += `</tbody></table> <label style="float:right;margin:16px"> مبلغ کل :${money}</label>`
      this.printService.onPrint(printContents, `  دریافت از مشتری`)
    }
  }

  addFinancialRequest() {
    this.router.navigateByUrl(`bursary/customerReceive/customerReceipt`)
  }


}

import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { TableColumn } from 'src/app/core/components/custom/table/models/table-column';
import { TableColumnDataType } from 'src/app/core/components/custom/table/models/table-column-data-type';
import { TableColumnFilter } from 'src/app/core/components/custom/table/models/table-column-filter';
import { TableColumnFilterTypes } from 'src/app/core/components/custom/table/models/table-column-filter-types';
import { FormActionTypes } from 'src/app/core/constants/form-action-types';
import { MoneyPipe } from 'src/app/core/pipes/money.pipe';
import { LoaderService } from 'src/app/core/services/loader.service';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { PagesCommonService } from 'src/app/shared/services/pages/pages-common.service';
import { Cheque } from '../../../entities/cheque';
import { TableConfigurations } from 'src/app/core/components/custom/table/models/table-configurations';
import { TableOptions } from 'src/app/core/components/custom/table/models/table-options';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';
import { GetChqueSheetsQuery } from '../../../repositories/cheque/queries/get-cheque-sheets-query';
import { ChequeSheet } from '../../../entities/cheque-sheet';
import { FormArray } from '@angular/forms';
import { GetUsedChequeSheetsQuery } from '../../../repositories/cheque/queries/get-used-cheque-sheets-query';
import { AccountingDocument } from '../../../entities/accounting-document';
import { CreateChequeSheetDocumentCommand } from '../../../repositories/cheque/commands/create-cheque-sheet-document-command';
import { AccountHeads, AccountReferencesGroupEnums, Documents } from '../../../entities/enums';
import { SelectReferenceComponent } from './select-reference/select-reference.component';
import { ChequeAttachmentsComponent } from './cheque-attachments/cheque-attachments.component';
import { ShowChequeSheetAttachmentsComponent } from './show-cheque-sheet-attachments/show-cheque-sheet-attachments.component';
import { ShowChequeDetailComponent } from './show-cheque-detail/show-cheque-detail.component';
import { DatePipe } from '@angular/common';
import { SelectDateComponent } from './select-date/select-date.component';
import { TablePaginationOptions } from 'src/app/core/components/custom/table/models/table-pagination-options';

@Component({
  selector: 'app-report-cheque',
  templateUrl: './report-cheque.component.html',
  styleUrls: ['./report-cheque.component.scss']
})
export class ReportChequeComponent extends BaseComponent {

  @ViewChild('buttonShowDetails', { read: TemplateRef }) buttonShowDetails!: TemplateRef<any>;



  tableConfigurations!: TableConfigurations;
  chequeSheets: ChequeSheet[] = [];
  entryMode: string = "0";
  chequeType: string = '0';
  filterSum: number = 0;
  selectedDebitReference !: number;
  selectedDebitReferenceGroup !: number;
  isMergeStatus: number = 1;
  documentDate !: Date;
  constructor(private _mediator: Mediator, private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router, public dialog: MatDialog,
    public loaderService: LoaderService, public printService: PagesCommonService, private pipe: MoneyPipe) {
    super(route, router);
  }


  async resolve(params?: any) {
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
    colButtonShowDetails.template = this.buttonShowDetails;

    let columns: TableColumn[] = [
      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
     
      new TableColumn(
        "sheetSeqNumber",
        "شماره چک",
        TableColumnDataType.Text,
        "12%",
        true,
        new TableColumnFilter("sheetSeqNumber", TableColumnFilterTypes.Text)
      ),
     
     
      new TableColumn(
        "totalCost",
        "مبلغ",
        TableColumnDataType.Money,
        "10%",
        true,
        new TableColumnFilter("totalCost", TableColumnFilterTypes.Money)
      ),




      // new TableColumn(
      //   "sheetUniqueNumber",
      //   "شماره یکتا",
      //   TableColumnDataType.Text,
      //   "12%",
      //   true,
      //   new TableColumnFilter("sheetUniqueNumber", TableColumnFilterTypes.Text)
      // ),



      new TableColumn(
        "debitAccountReferenceTitle",
        "اسناددریافتنی-تفصیل",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("debitAccountReferenceTitle", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "debitAccountReferenceGroupTitle",
        "اسناددریافتنی-گروه",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("debitAccountReferenceGroupTitle", TableColumnFilterTypes.Text)
      ),


      new TableColumn(
        "debitAccountHeadTitle",
        "اسناددریافتنی-نام حساب",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("debitAccountHeadTitle", TableColumnFilterTypes.Text)
      ),

      // new TableColumn(
      //   "debitAccountReferenceCode",
      //   "کد بدهکار",
      //   TableColumnDataType.Text,
      //   "15%",
      //   true,
      //   new TableColumnFilter("debitAccountReferenceCode", TableColumnFilterTypes.Text)
      // ),

      new TableColumn(
        "creditAccountReferenceTitle",
        "پرداخت کننده-تفصیل",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("creditAccountReferenceTitle", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "creditAccountReferenceGroupTitle",
        "پرداخت کننده-گروه",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("creditAccountReferenceGroupTitle", TableColumnFilterTypes.Text)
      ),


      new TableColumn(
        "creditAccountHeadTitle",
        "پرداخت کننده-نام حساب",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("creditAccountHeadTitle", TableColumnFilterTypes.Text)
      ),


      // new TableColumn(
      //   "creditAccountReferenceCode",
      //   "کد بستانکار",
      //   TableColumnDataType.Text,
      //   "15%",
      //   true,
      //   new TableColumnFilter("creditAccountReferenceCode", TableColumnFilterTypes.Text)
      // ),

      new TableColumn(
        "issueReferenceBankTitle",
        "بانک دریافت کننده چک",
        TableColumnDataType.Text,
        "10%",
        true,
        new TableColumnFilter("issueReferenceBankTitle", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "description",
        "توضیحات",
        TableColumnDataType.Text,
        "20%",
        true,
        new TableColumnFilter("description", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "chequeTypeTitle",
        "نوع چک",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("chequeTypeTitle", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "chequeDocumentStateTitle",
        "وضعیت",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("chequeDocumentStateTitle", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "issueDate",
        "دریافت ",
        TableColumnDataType.Date,
        "7%",
        true,
        new TableColumnFilter("issueDate", TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        "receiptDate",
        "سررسید ",
        TableColumnDataType.Date,
        "7%",
        true,
        new TableColumnFilter("receiptDate", TableColumnFilterTypes.Date)
      ),

      // new TableColumn(
      //   "createName",
      //   "ثبت کننده",
      //   TableColumnDataType.Text,
      //   "15%",
      //   true,
      //   new TableColumnFilter("createName", TableColumnFilterTypes.Text)
      // ),
      colButtonShowDetails,
    ];

    var config = new TablePaginationOptions();
    config.pageIndex = 0;
    config.pageSize = 200;
    config.totalItems = 0;
    config.pageSizeOptions = [200, 500, 1000];
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, ''));

    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.showTopSettingMenu = true;
  
    await this.get()


  }



  
  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
  async get(param?: any) {

    this.isLoading = true;
var me = this;
    try {



      let searchQueries: SearchQuery[] = []
      if (this.tableConfigurations.filters) {
        this.tableConfigurations.filters.forEach(filter => {
          searchQueries.push(new SearchQuery({
            propertyName: filter.columnName,
            values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
            comparison: filter.searchCondition,
            nextOperand: "and"
          }))
        })
      }

      let orderByProperty = '';
      if (this.tableConfigurations.sortKeys) {
        this.tableConfigurations.sortKeys.forEach((key, index) => {
          orderByProperty += index ? `,${key}` : key
        })
      }


      searchQueries.push(new SearchQuery({
        propertyName: 'IsUsed',
        values: [true],
        comparison: 'equal',
        nextOperand: "and"
      }));

      if (this.entryMode == "0")
        searchQueries.push(new SearchQuery({
          propertyName: 'chequeDocumentState',
          values: [0],
          comparison: 'equal'
        }))

      else if (this.entryMode == "1")
        searchQueries.push(new SearchQuery({
          propertyName: 'chequeDocumentState',
          values: [1],
          comparison: 'equal'
        }))

      else if (this.entryMode == "2")
        searchQueries.push(new SearchQuery({
          propertyName: 'chequeDocumentState',
          values: [2],
          comparison: 'equal'
        }))

      else if (this.entryMode == "3")
        searchQueries.push(new SearchQuery({
          propertyName: 'chequeDocumentState',
          values: [3],
          comparison: 'equal'
        }))

      else if (this.entryMode == "4")
        searchQueries.push(new SearchQuery({
          propertyName: 'chequeDocumentState',
          values: [4],
          comparison: 'equal'
        }))

      else if (this.entryMode == "6") {
        searchQueries.push(new SearchQuery({
          propertyName: 'chequeDocumentState',
          values: [0],
          comparison: 'equal'
        }))

        searchQueries.push(new SearchQuery({
          propertyName: 'chequeDocumentState',
          values: [1],
          comparison: 'equal'
        }))

      
      }

if(me.chequeType != "0"){
 
    searchQueries.push(new SearchQuery({
      propertyName: 'chequeTypeBaseId',
      values: [28543],
      comparison: me.chequeType == "1" ? 'equal' : 'notEqual'
    }));
}


      var response = await this._mediator.send(new GetUsedChequeSheetsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty));
      this.isLoading = false;
      this.chequeSheets = response.data;
      // this.form = new FormArray(this.financialRequests.map((x) => this.createForm(x)));

      // this.calcSelectedSum();
      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
      this.filterSum = response.totalSum;
    }
    catch {
      this.isLoading = false;
    }

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
  async ngOnInit() {
  }

  async ngAfterViewInit() {
    await this.resolve();
  }


  async print() {

    let printContents = '';
    if (this.chequeSheets.length > 0) {
      const sum = this.chequeSheets.reduce((acc, item) => acc + item.totalCost, 0);

      printContents += `<table><thead>
                     <tr>
                       <th>شماره چک</th>
                       <th>تاریخ </th>
                       <th>مبلغ</th>
                       <th>واریز کننده</th>
                       <th>دریافت کننده</th>
                       <th>توضیحات</th>

                     </tr>
                   </thead><tbody>`;
      this.chequeSheets.map(data => {

        const datetime = this.printService.toPersianDate(data.receiptDate as Date)
        printContents += `<tr>
                            <td>${data.sheetUniqueNumber}</td>
                           <td>${datetime.split(' ')[1]}</td>
                           <td>${this.pipe.transform(data.totalCost)}</td>
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



  async selectReference(value = 1) {
    const dialogRef = this.dialog.open(SelectReferenceComponent, {
      disableClose: false,
      width: '40%',
      height: '40%',
    });

    dialogRef.afterClosed().subscribe(async item => {

      this.selectedDebitReference = item.split(",")[0];
      this.documentDate = item.split(",")[1];
      this.selectedDebitReferenceGroup = item.split(",")[2];
      this.isMergeStatus = 2;
      await this.chequeDocument(value);

    });
  }


  async selectDate(value: number) {
    const dialogRef = this.dialog.open(SelectDateComponent, {
      disableClose: false,
      width: '40%',
      height: '40%',
    });

    dialogRef.afterClosed().subscribe(async item => {

      this.documentDate = item;
      this.isMergeStatus = value;
      await this.chequeDocument(value);

    });
  }

  async validateTypeChequeDocument() {
    let selectedDocuments = this.chequeSheets.filter((x: any) => x.selected);
    selectedDocuments.forEach(async (item: any) => {
      if (item.issueReferenceBankId == null || item.issueReferenceBankId == undefined) {
        const dialogRef = this.dialog.open(SelectReferenceComponent, {
          disableClose: false,
          width: '40%',
          height: '40%',
        });

        dialogRef.afterClosed().subscribe(async item => {

          this.selectedDebitReference = item.split(",")[0];
          this.documentDate = item.split(",")[1];
          this.selectedDebitReferenceGroup = item.split(",")[2];
          this.isMergeStatus = 2;
          await this.chequeDocument(2);
        });

      }
      else {


        
        this.isMergeStatus = 1;

        const dialogRef = this.dialog.open(SelectDateComponent, {
          disableClose: false,
          width: '40%',
          height: '40%',
        });

        dialogRef.afterClosed().subscribe(async item => {

        //  this.selectedDebitReference = item.split(",")[0];
          this.documentDate = item;//.split(",")[1];
          this.isMergeStatus = 1;
          await this.chequeDocument(2);
        });




       // await this.chequeDocument(2);
      }


    });
  }

  async chequeDocument(status: number) {

    let selectedDocuments = this.chequeSheets.filter((x: any) => x.selected);
    let accountingDoc = new CreateChequeSheetDocumentCommand();

    if (status == 1) { // ارسال به بانک

      selectedDocuments.forEach((item: any) => {
        let ad !: AccountingDocument;
        const obj = ad || {};

        const date = new Date(this.documentDate);
        //  let datetime = this.datePipe.transform(date, 'yyyy-MM-dd');

        obj.DocumentNo = item.sheetUniqueNumber;
        obj.CodeVoucherGroupId = 2233;
        obj.Amount = item.totalCost;
        obj.CreditAccountHeadId = AccountHeads.AccountHeadCode_2301;
        obj.CreditAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_31;
        obj.CreditAccountReferenceId = item.debitAccountReferenceId == undefined ? null : item.debitAccountReferenceId;
        obj.ReferenceName = " " + item.creditAccountReferenceTitle + " ",
        obj.ReferenceCode = item.creditAccountReferenceCode,
        obj.DebitAccountHeadId = AccountHeads.AccountHeadCode_2303;
        obj.DebitAccountReferenceGroupId =this.selectedDebitReferenceGroup;//AccountReferencesGroupEnums.AccountReferencesGroupCode_02;
        obj.DebitAccountReferenceId = this.selectedDebitReference;
        obj.DocumentDate = date;
        obj.DocumentId = item.id;
        obj.DocumentTypeBaseId = Documents.ChequeSheet;
        obj.SheetUniqueNumber = item?.sheetUniqueNumber;
        obj.CurrencyAmount = 0;
        obj.IsRial = true;
        obj.CurrencyFee = 0;
        obj.CurrencyTypeBaseId = 28306; // rial
        obj.NonRialStatus = 0;
        obj.SheetUniqueNumber = item.id != null && item.id != 0 ? item?.sheeSeqNumber : "0";
        obj.ChequeSheetId = item.id;
        obj.Description = "بابت به جریان گذاشتن چک شماره " + item?.sheetUniqueNumber + "به تاریخ " + this.printService.toPersianDate(item.receiptDate as Date) + "بانک " + item.bankTitle + " " + item.financialRequestDescription;
        accountingDoc.dataList.push(obj);
      })

      accountingDoc.status = status;
    }

    if (status == 2) { // وصول چک
      selectedDocuments.forEach(async (item: any) => {
        let ad !: AccountingDocument;
        var obj = ad || {};

        const date = new Date(this.documentDate);
        //  let datetime = this.datePipe.transform(date, 'yyyy-MM-dd');

        // if (this.isMergeStatus == 2) {



        //   obj.DocumentNo = item.sheetUniqueNumber;
        //   obj.CodeVoucherGroupId = 2233;
        //   obj.Amount = item.totalCost;
        //   obj.CreditAccountHeadId = AccountHeads.AccountHeadCode_2301;
        //   obj.CreditAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_31;
        //   obj.CreditAccountReferenceId = (item.debitAccountReferenceId == undefined || item.debitAccountReferenceId == null) ? item.creditAccountReferenceId: item.debitAccountReferenceId;
        //   obj.ReferenceName = " " + item.creditAccountReferenceTitle + " ",
        //   obj.ReferenceCode = item.creditAccountReferenceCode,
        //   obj.DebitAccountHeadId = AccountHeads.AccountHeadCode_2303;
        //   obj.DebitAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_02;
        //   obj.DebitAccountReferenceId = this.selectedDebitReference;
        //   obj.DocumentDate = date;
        //   obj.DocumentId = item.id;
        //   obj.DocumentTypeBaseId = Documents.ChequeSheet;
        //   obj.SheetUniqueNumber = item?.sheetUniqueNumber;
        //   obj.CurrencyAmount = 0;
        //   obj.IsRial = true;
        //   obj.CurrencyFee = 0;
        //   obj.CurrencyTypeBaseId = 28306; // rial
        //   obj.NonRialStatus = 0;
        //   obj.SheetUniqueNumber = item.id != null && item.id != 0 ? item?.sheeSeqNumber : "0";
        //   obj.ChequeSheetId = item.id;
        //   obj.Description = "بابت به جریان گذاشتن چک شماره " + item?.sheetUniqueNumber + "به تاریخ " + this.printService.toPersianDate(item.receiptDate as Date) + "بانک " + item.bankTitle + " " + item.financialRequestDescription;
        //   accountingDoc.dataList.push(obj);
        // };

        obj = ad || {};

        obj.DocumentNo = item.sheetUniqueNumber;
        obj.CodeVoucherGroupId = 2234;
        obj.Amount = item.totalCost;
        obj.CreditAccountHeadId = AccountHeads.AccountHeadCode_2301;
        obj.CreditAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_02;;
        obj.CreditAccountReferenceId = this.isMergeStatus == 2 ? this.selectedDebitReference : item.issueReferenceBankId;
        obj.ReferenceName = " " + item.creditAccountReferenceTitle + " ",
        obj.ReferenceCode = item.creditAccountReferenceCode,
        obj.DebitAccountHeadId = AccountHeads.AccountHeadCode_2601;
        obj.DebitAccountReferenceGroupId =   this.selectedDebitReferenceGroup ;
        obj.DebitAccountReferenceId = this.isMergeStatus == 2 ? this.selectedDebitReference : item.issueReferenceBankId;
        obj.DocumentDate = date;
        obj.DocumentId = item.id;
        obj.DocumentTypeBaseId = Documents.ChequeSheet;
        obj.SheetUniqueNumber = item?.sheetUniqueNumber;
        obj.CurrencyAmount = 0;
        obj.IsRial = true;
        obj.CurrencyFee = 0;
        obj.CurrencyTypeBaseId = 28306; // rial
        obj.NonRialStatus = 0;
        obj.SheetUniqueNumber = item.id != null && item.id != 0 ? item?.sheeSeqNumber : "0";
        obj.ChequeSheetId = item.id;
        obj.Description = "بابت وصول چک شماره " + item?.sheetUniqueNumber + "به تاریخ " + this.printService.toPersianDate(item.receiptDate as Date) + "بانک " + item.bankTitle + " " + item.financialRequestDescription;
        accountingDoc.dataList.push(obj);

      });

      accountingDoc.status = status;

    }

    if (status == 3) { // برگشت چک
      selectedDocuments.forEach((item: any) => {

        let ad !: AccountingDocument;
        var obj = ad || {};

        obj.DocumentNo = item.sheetUniqueNumber;
        obj.CodeVoucherGroupId = 2235;
        obj.Amount = item.totalCost;
        obj.CreditAccountHeadId = AccountHeads.AccountHeadCode_2303;
        obj.CreditAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_02;;
        obj.CreditAccountReferenceId = item.creditAccountReferenceId == undefined ? null : item.creditAccountReferenceId;
        obj.ReferenceName = " " + item.creditAccountReferenceTitle + " ",
        obj.ReferenceCode = item.creditAccountReferenceCode,
        obj.DebitAccountHeadId = AccountHeads.AccountHeadCode_2301;
        obj.DebitAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_31;
        obj.DebitAccountReferenceId = item.issueReferenceBankId;
        obj.DocumentDate = item.receiptDate;
        obj.DocumentId = item.id;
        obj.DocumentTypeBaseId = Documents.ChequeSheet;
        obj.SheetUniqueNumber = item?.sheetUniqueNumber;
        obj.CurrencyAmount = 0;
        obj.IsRial = true;
        obj.CurrencyFee = 0;
        obj.CurrencyTypeBaseId = 28306; // rial
        obj.NonRialStatus = 0;
        obj.SheetUniqueNumber = item.id != null && item.id != 0 ? item?.sheeSeqNumber : "0";
        obj.ChequeSheetId = item.id;
        obj.Description = "بابت برگشت چک شماره " + item?.sheetUniqueNumber + "به تاریخ " + this.printService.toPersianDate(item.receiptDate as Date) + "بانک " + item.bankTitle + " " + item.financialRequestDescription;
        accountingDoc.dataList.push(obj);

        ad: { };
        obj = ad || {};

        obj.DocumentNo = item.sheetUniqueNumber;
        obj.CodeVoucherGroupId = 2235;
        obj.Amount = item.totalCost;
        obj.CreditAccountHeadId = AccountHeads.AccountHeadCode_2301;
        obj.CreditAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_31;
        obj.CreditAccountReferenceId = item.debitAccountReferenceId == undefined ? null : item.debitAccountReferenceId;
        obj.ReferenceName = " " + item.creditAccountReferenceTitle + " ",
          obj.ReferenceCode = item.creditAccountReferenceCode == undefined ? null : item.creditAccountReferenceCode,
          obj.DebitAccountHeadId = AccountHeads.AccountHeadCode_2304;
        obj.DebitAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_31;
        obj.DebitAccountReferenceId = item.creditAccountReferenceId == undefined ? null : item.creditAccountReferenceId;;
        obj.DocumentDate = item.receiptDate;
        obj.DocumentId = item.id;
        obj.DocumentTypeBaseId = Documents.ChequeSheet;
        obj.SheetUniqueNumber = item?.sheetUniqueNumber;
        obj.CurrencyAmount = 0;
        obj.IsRial = true;
        obj.CurrencyFee = 0;
        obj.CurrencyTypeBaseId = 28306; // rial
        obj.NonRialStatus = 0;
        obj.SheetUniqueNumber = item.id != null && item.id != 0 ? item?.sheeSeqNumber : "0";
        obj.ChequeSheetId = item.id;
        obj.Description = "بابت برگشت چک شماره " + item?.sheetUniqueNumber + "به تاریخ " + this.printService.toPersianDate(item.receiptDate as Date) + "بانک " + item.bankTitle + " " + item.financialRequestDescription;
        accountingDoc.dataList.push(obj);

      })

      accountingDoc.status = status;

    }

    if (status == 4) { // عودت چک
      selectedDocuments.forEach((item: any) => {

        let ad !: AccountingDocument;
        const obj = ad || {};

        obj.DocumentNo = item.sheetUniqueNumber;
        obj.CodeVoucherGroupId = 2236;
        obj.Amount = item.totalCost;
        obj.CreditAccountHeadId = item.debitAccountHeadId;
        obj.CreditAccountReferenceGroupId = item.debitAccountReferenceGroupId;
        obj.CreditAccountReferenceId = item.debitAccountReferenceId;
        obj.ReferenceName = " " + item.creditAccountReferenceTitle + " ",
        obj.ReferenceCode = item.creditAccountReferenceCode,
        obj.DebitAccountHeadId = item.creditAccountHeadId
        obj.DebitAccountReferenceGroupId = item.creditAccountReferenceGroupId;
        obj.DebitAccountReferenceId = item.creditAccountReferenceId;
        obj.DocumentDate = this.documentDate;
        obj.DocumentId = item.id;
        obj.DocumentTypeBaseId = Documents.ChequeSheet;
        obj.SheetUniqueNumber = item?.sheetUniqueNumber;
        obj.CurrencyAmount = 0;
        obj.IsRial = true;
        obj.CurrencyFee = 0;
        obj.CurrencyTypeBaseId = 28306; // rial
        obj.NonRialStatus = 0;
        obj.SheetUniqueNumber = item.id != null && item.id != 0 ? item?.sheeSeqNumber : "0";
        obj.ChequeSheetId = item.id;
        obj.Description = "بابت عودت چک شماره " + item?.sheetUniqueNumber + "به تاریخ " + this.printService.toPersianDate(item.receiptDate as Date) + "بانک " + item.bankTitle + " " + item.financialRequestDescription;
        accountingDoc.dataList.push(obj);

      })

      accountingDoc.status = status;
    }

    this.isLoading = true;
    var response = await this._mediator.send(<CreateChequeSheetDocumentCommand>accountingDoc);
    await this.get();
    this.isLoading = false;

  }

  filterSelectedItem() {
    this.chequeSheets = this.chequeSheets.filter((x: any) => x.selected);;

    this.sumSelected(this.chequeSheets);
    // this.form = new FormArray(this.financialRequests.map((x) => this.createForm(x)));
    // this.calcSelectedSum();
  }

  sumSelected(selectedItems: ChequeSheet[]) {

    let count = selectedItems.length;

    // this.filterSum = selectedItems.reduce((total, item) => {
    //   return total + item.amount;
    // }, 0);

    count && (this.tableConfigurations.pagination.totalItems = count);
  }


  uploadAttachments(item: any) {
    const dialogRef = this.dialog.open(ChequeAttachmentsComponent, {
      width: '80%',
      height: '80%',
      data: { data: JSON.stringify(item) }
    });

    dialogRef.afterClosed().subscribe(async item => {


    });
  }


  showChequeAttachments(item: any) {
    const dialogRef = this.dialog.open(ShowChequeSheetAttachmentsComponent, {
      width: '80%',
      height: '80%',
      data: { data: JSON.stringify(item.id) }
    });

    dialogRef.afterClosed().subscribe(async item => {


    });
  }

  showDetails(item: any) {

    const dialogRef = this.dialog.open(ShowChequeDetailComponent, {
      width: '80%',
      height: '80%',
      data: { data: JSON.stringify(item) }
    });

    dialogRef.afterClosed().subscribe(async item => {


    });


  }

  showRelatedDocument(){
    let items = this.chequeSheets.filter((x: any) => x.selected);

    if((items[0].voucherHeadId as number) >0)
    this.router.navigateByUrl(`/accounting/voucherHead/add?id=${items[0].voucherHeadId}`)
  else
  this.notificationService.showFailureMessage("سند مورد نظر مربوط به سال های مالی قبل می باشد", 0);
  }




}

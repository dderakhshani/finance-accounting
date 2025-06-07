import { Component, OnInit, Optional } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { TableColumn } from 'src/app/core/components/custom/table/models/table-column';
import { TableColumnDataType } from 'src/app/core/components/custom/table/models/table-column-data-type';
import { TableColumnFilter } from 'src/app/core/components/custom/table/models/table-column-filter';
import { TableColumnFilterTypes } from 'src/app/core/components/custom/table/models/table-column-filter-types';
import { TableConfigurations } from 'src/app/core/components/custom/table/models/table-configurations';
import { TableOptions } from 'src/app/core/components/custom/table/models/table-options';
import { TablePaginationOptions } from 'src/app/core/components/custom/table/models/table-pagination-options';
import { FormActionTypes } from 'src/app/core/constants/form-action-types';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { ResponseTejaratModel } from 'src/app/modules/bursary/entities/response-tejarat-model';
import { GetTejaratBalanceQuery } from 'src/app/modules/bursary/repositories/tejarat-balance/queries/get-tejarat-balance-query';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';

@Component({
  selector: 'app-bank-balance-dialog',
  templateUrl: './bank-balance-dialog.component.html',
  styleUrls: ['./bank-balance-dialog.component.scss']
})
export class BankBalanceDialogComponent extends BaseComponent {
  

  tableConfigurations!: TableConfigurations;

  fromPersianDate:Date = new Date();
  toPersianDate:Date = new Date();
  accountNumber:string ="132924007";
  tejaratBalanceItems :ResponseTejaratModel | undefined = undefined;
  
  
  constructor(private _mediator: Mediator, @Optional() public dialogRef: MatDialogRef<BankBalanceDialogComponent>, public dialog: MatDialog) {
    super()
  }

   async ngOnInit()  {
    await this.resolve();
  }

  async ngAfterViewInit() {
    await this.resolve();
  }
  
  async resolve(params?: any) {


    this.formActions = [

      FormActionTypes.refresh,
      FormActionTypes.delete
    ];


    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),

      //colEditDocument,

      new TableColumn(
        'persianDateAndTime',
        'تاریخ',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter("persianDateAndTime", TableColumnFilterTypes.Text)
      ),
  
      new TableColumn(
        "transactionAmountCredit",
        "مبلغ واریزی",
        TableColumnDataType.Money,
        "15%",
        true,
        new TableColumnFilter("transactionAmountCredit", TableColumnFilterTypes.Money)
      ),
 
      new TableColumn(
        "description",
        "شرح",
        TableColumnDataType.Text,
        "40%",
        true,
        new TableColumnFilter("description", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "docNumber",
        "رهگیری",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("docNumber", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "payId1",
        "شناسه واریز",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("payId1", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "bursaryNo",
        "شماره خزانه داری",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("bursaryNo", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "voucherNo",
        "شماره سند",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("voucherNo", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "accountReferenceTitle",
        "نام طرف حساب",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("accountReferenceTitle", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "bankName",
        "بانک",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("bankName", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "branchCode",
        "کد شعبه",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("branchCode", TableColumnFilterTypes.Text)
      )];
      
    var config = new TablePaginationOptions();
    config.pageIndex = 0;
    config.pageSize = 1000;
    config.totalItems = 0;
    config.pageSizeOptions = [200, 500, 1000];


    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, ''), config);
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
   
  var me = this;
  me.isLoading = true;

 

  let searchQueries: SearchQuery[] =[];

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

  
  

  try {
    var response = await this._mediator.send(new GetTejaratBalanceQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty,this.fromPersianDate,this.toPersianDate,this.accountNumber));
    this.isLoading = false;
    this.tejaratBalanceItems = response;
  

  } catch {
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

 

}



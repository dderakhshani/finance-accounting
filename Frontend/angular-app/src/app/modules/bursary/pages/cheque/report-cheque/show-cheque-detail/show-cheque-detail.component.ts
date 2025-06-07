import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { TableColumn } from 'src/app/core/components/custom/table/models/table-column';
import { TableColumnDataType } from 'src/app/core/components/custom/table/models/table-column-data-type';
import { TableColumnFilter } from 'src/app/core/components/custom/table/models/table-column-filter';
import { TableColumnFilterTypes } from 'src/app/core/components/custom/table/models/table-column-filter-types';
import { TableConfigurations } from 'src/app/core/components/custom/table/models/table-configurations';
import { TableOptions } from 'src/app/core/components/custom/table/models/table-options';
import { FormActionTypes } from 'src/app/core/constants/form-action-types';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { VoucherDetail } from 'src/app/modules/accounting/entities/voucher-detail';
import { GetLedgerReportQuery } from 'src/app/modules/accounting/repositories/reporting/queries/get-ledger-report-query';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';

@Component({
  selector: 'app-show-cheque-detail',
  templateUrl: './show-cheque-detail.component.html',
  styleUrls: ['./show-cheque-detail.component.scss']
})
export class ShowChequeDetailComponent extends BaseComponent {

voucherDetails :VoucherDetail[] = [];
tableConfigurations!: TableConfigurations;
chequeSheetIds : number[] =[];

constructor(private _mediator: Mediator, @Optional() public dialogRef: MatDialogRef<ShowChequeDetailComponent>, @Inject(MAT_DIALOG_DATA) public value: any, public dialog: MatDialog) {
  super()
}

async ngOnInit() {

  if (this.value.data != null && this.value.data != "null")
  var chequeSheet = JSON.parse(this.value.data);
  this.chequeSheetIds.push(chequeSheet.id);


  await this.resolve();
}

 async resolve(params?: any) {

    this.formActions = [
      FormActionTypes.refresh
    ];

    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      
      new TableColumn(
        "voucherDate",
        "تاریخ سند",
        TableColumnDataType.Date,
        "15%",
        true,
        new TableColumnFilter("voucherDate", TableColumnFilterTypes.Date)
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
        "title",
        "معین",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("title", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "accountReferencesGroupTitle",
        "گروه",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("accountReferencesGroupTitle", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "referenceName_1",
        "تفصیل",
        TableColumnDataType.Text,
        "12%",
        true,
        new TableColumnFilter("referenceName_1", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "debit",
        "مبلغ بدهکار",
        TableColumnDataType.Money,
        "10%",
        true,
        new TableColumnFilter("debit", TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        "credit",
        "مبلغ بستانکار",
        TableColumnDataType.Money,
        "10%",
        true,
        new TableColumnFilter("credit", TableColumnFilterTypes.Money)
      ),

      new TableColumn(
        "voucherRowDescription",
        "توضیحات",
        TableColumnDataType.Text,
        "20%",
        true,
        new TableColumnFilter("voucherRowDescription", TableColumnFilterTypes.Text)
      ),
    ];


    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, ''));
    this.tableConfigurations.options.showSumRow = true;


    await this.get()



  }
  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
 async get(param?: any) {

  let model = new GetLedgerReportQuery();
  model.chequeSheetIds = this.chequeSheetIds;

  var response = await this._mediator.send(model);

  this.voucherDetails = response.data;

  response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
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

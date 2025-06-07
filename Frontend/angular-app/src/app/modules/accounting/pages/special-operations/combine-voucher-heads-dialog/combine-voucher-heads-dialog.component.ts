import {Component, Inject} from '@angular/core';
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {VoucherHead} from "../../../entities/voucher-head";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableConfigurations} from "../../../../../core/components/custom/table/models/table-configurations";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {FormControl} from "@angular/forms";
import {CombineVoucherHeadsCommand} from "../../../repositories/voucher-head/commands/combine-voucher-heads-command";

@Component({
  selector: 'app-combine-voucher-heads-dialog',
  templateUrl: './combine-voucher-heads-dialog.component.html',
  styleUrls: ['./combine-voucher-heads-dialog.component.scss']
})
export class CombineVoucherHeadsDialogComponent extends BaseComponent{

  public entities: VoucherHead[] = []
  tableConfigurations!: TableConfigurations;
  mainVoucherIdFormControl= new FormControl();

  constructor(
    private _mediator:Mediator,
    private dialogRef: MatDialogRef<CombineVoucherHeadsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data : any
  ) {
    super()
    this.entities = data.voucherHeads
  }

  ngOnInit() {
    this.resolve()
  }
  resolve() {
    let tableColumns = [
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, ''),
      new TableColumn(
        'voucherNo',
        'شماره سند',
        TableColumnDataType.Number,
        '',
        true,
        undefined, //new TableColumnFilter('voucherNo', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'voucherDate',
        'تاریخ',
        TableColumnDataType.Date,
        '',
        true,
        undefined, //new TableColumnFilter('voucherDate', TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'totalDebit',
        'بدهکار',
        TableColumnDataType.Money,
        '',
        true,
        undefined, //new TableColumnFilter('totalDebit', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'totalCredit',
        'بستانکار',
        TableColumnDataType.Money,
        '',
        true,
        undefined, //new TableColumnFilter('totalCredit', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'codeVoucherGroupTitle',
        'نوع سند',
        TableColumnDataType.Text,
        '',
        true,
        undefined, //new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'voucherStateName',
        'وضعیت سند',
        TableColumnDataType.Text,
        '',
        true,
        undefined, //new TableColumnFilter('voucherStateName', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'voucherDescription',
        'شرح سند',
        TableColumnDataType.Text,
        '',
        true,
        undefined, //new TableColumnFilter('voucherDescription', TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        'traceNo',
        'شماره پیگیری',
        TableColumnDataType.Text,
        '',
        true,
        undefined, //new TableColumnFilter('traceNo', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'creator',
        'صادر کننده',
        TableColumnDataType.Text,
        '',
        true,
        undefined, //new TableColumnFilter('creator', TableColumnFilterTypes.Text),

      ),
    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true,undefined,true));

    this.tableConfigurations.options.showSumRow = true;

  }

  async combineVouchers() {
    let request = new CombineVoucherHeadsCommand();
    request.mainVoucherId = this.mainVoucherIdFormControl.value
    request.mainVoucherNo = this.entities.find(x => x.id === request.mainVoucherId)?.voucherNo;
    request.voucherHeadIdsToCombine = this.entities.filter(x => x.id !== request.mainVoucherId).map(x => x.id)
    await this._mediator.send(request).then(() => {
      this.dialogRef.close()
    })

  }

  initialize(params?: any): any {
  }

  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }
  update(param?: any): any {
  }

}

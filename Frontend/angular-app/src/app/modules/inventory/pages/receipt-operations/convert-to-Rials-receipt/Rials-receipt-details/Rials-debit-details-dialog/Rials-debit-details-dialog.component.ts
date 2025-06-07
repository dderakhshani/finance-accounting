import {Component, EventEmitter, Inject, Input, OnInit, Output} from '@angular/core';
import {TableConfigurations} from "../../../../../../../core/components/custom/table/models/table-configurations";
import {SearchQuery} from "../../../../../../../shared/services/search/models/search-query";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {GetVoucherDetailsQuery} from "../../../../../../accounting/repositories/voucher-detail/queries/get-voucher-details-query";
import {VoucherDetail} from "../../../../../../accounting/entities/voucher-detail";
import {TableColumnDataType} from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../../../core/components/custom/table/models/table-column-filter";
import {  TableColumnFilterTypes} from "../../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableColumn} from "../../../../../../../core/components/custom/table/models/table-column";
import {TableOptions} from "../../../../../../../core/components/custom/table/models/table-options";
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";

@Component({
  selector: 'app-Rials-debit-details-dialog',
  templateUrl: './Rials-debit-details-dialog.component.html',
  styleUrls: ['./Rials-debit-details-dialog.component.scss']
})
export class RialsDebitDetailsDialogComponent extends BaseComponent {
  resolve(params?: any) {
    let tableColumns = [
      new TableColumn('selected', '', TableColumnDataType.Select, ''),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, ''),
      new TableColumn(
        'voucherRowDescription',
        'شرح آرتیکل',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('voucherRowDescription', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'voucherDate',
        'تاریخ',
        TableColumnDataType.Date,
        '',
        true,
        new TableColumnFilter('voucherDate', TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'debit',
        'بدهکار',
        TableColumnDataType.Money,
        '',
        true,
        new TableColumnFilter('debit', TableColumnFilterTypes.Money)
      )];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));
    this.get();
  }

  tableConfigurations!: TableConfigurations;
  @Output() totalSelectedDebit: number = 0;
  paramDialog: any;
  submitBtnTitle: string;
  debitSelected: VoucherDetail[] = [];

  constructor(
    private _mediator: Mediator,
    public dialogRef: MatDialogRef<RialsDebitDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
    super();
    this.paramDialog = data;
    this.submitBtnTitle = 'محاسبه';
  }

  ngOnInit(): void {
    this.resolve();
     }

  get() {
    this.entities = [];
    let searchQueries: SearchQuery[] = [];
    searchQueries.push(
      new SearchQuery({
        propertyName: 'ReferenceId1',
        comparison: 'equal',
        nextOperand: 'and',
        values: this.paramDialog.ReferenceId1
      }))

    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
          comparison: filter.searchCondition,
          nextOperand: filter.nextOperand
        }))
      })
    }
    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }
    this._mediator.send(new GetVoucherDetailsQuery(this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)).then(res => {
      console.log(res);
      this.entities = res.data.filter((s => s.debit > 0));
      res.totalCount && (this.tableConfigurations.pagination.totalItems = res.totalCount);
    });
  }
  calculateSumDebit() {
    this.debitSelected.forEach((x, i) => {
      this.totalSelectedDebit = this.totalSelectedDebit + x.debit;
    })
    this.dialogRef.close(this.totalSelectedDebit);
  }

  getDebit(event: any) {
    let existRow = this.debitSelected.filter(x => x.id == event.id);
    if (event.selected) {
      if (existRow.length == 0)
        this.debitSelected.push(event);
    } else {
      if (existRow.length > 0) { // @ts-ignore
        this.debitSelected.pop(event);
      }
    }
  }

  getAllDebit(entities: VoucherDetail[]) {
    this.debitSelected = [];
    this.debitSelected = entities;
  }

  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
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

import {Component, Inject} from '@angular/core';
import {Year} from "../../../../entities/year";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../../../core/components/custom/table/models/table-configurations";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-company-years-dialog',
  templateUrl: './company-years-dialog.component.html',
  styleUrls: ['./company-years-dialog.component.scss']
})
export class CompanyYearsDialogComponent extends BaseComponent {

  years: Year[] = [];
  tableConfigurations!:TableConfigurations;
pageModes = PageModes;
  constructor(private dialogRef: MatDialogRef<CompanyYearsDialogComponent>,
              @Inject(MAT_DIALOG_DATA) data: any)
  {
    super();
    this.years = data.years;
  }

  async ngOnInit() {
    await this.resolve()
  }

  resolve() {
    let columns : TableColumn[] = [
      new TableColumn(
        'selected',
        '',
        TableColumnDataType.Select,
        '2.5%'
      ),
      new TableColumn(
        'index',
        'ردیف',
        TableColumnDataType.Index,
        '2.5%'
      ),
      new TableColumn(
        'yearName',
        'سال مالی',
        TableColumnDataType.Number,
        '10%'
      ),
      new TableColumn(
        'firstDate',
        'تاریخ شروع سال مالی',
        TableColumnDataType.Date,
        '10%'
      ),
      new TableColumn(
        'lastDate',
        'تاریخ پایان سال مالی',
        TableColumnDataType.Date,
        '10%'
      ),
      new TableColumn(
        'isEditable',
        'قابل ویرایش',
        TableColumnDataType.CheckBox,
        '10%'
      ),
      new TableColumn(
        'isCalculable',
        'قابل محاسبه',
        TableColumnDataType.CheckBox,
        '10%'
      ),
      new TableColumn(
        'isCurrentYear',
        'سال مال جاری است',
        TableColumnDataType.CheckBox,
        '10%'
      ),
      new TableColumn(
        'lastEditableDate',
        'انقضا ویرایش',
        TableColumnDataType.Date,
        '10%'
      ),
    ]
    this.tableConfigurations = new TableConfigurations(columns,new TableOptions(false,true))
    this.tableConfigurations.options.useBuiltInPagination = true;
  }
  initialize() {

  }
  add(param?: any) {
      // @ts-ignore
    let selectedYears = this.years.filter(x => x.selected).map(x => x.id);
    this.years =  this.years.map(x => {
      // @ts-ignore
      x.selected = false;
      return x;
    })
    this.dialogRef.close(
      selectedYears
    )

  }
  get(param?: any) {
  }
  update(param?: any) {
  }
  delete(param?: any) {
  }
  close() {
  }


}

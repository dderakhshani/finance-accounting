import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {FormActionTypes} from "../../../../../../core/constants/form-action-types";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {GetYearsQuery} from "../../../../repositories/year/queris/get-years-query";
import {Year} from "../../../../entities/year";
import {FormArray, FormControl} from "@angular/forms";
import {CompanyYearsDialogComponent} from "../company-years-dialog/company-years-dialog.component";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {
  TableConfigurations
} from "../../../../../../core/components/custom/table/models/table-configurations";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-company-years-list',
  templateUrl: './company-years-list.component.html',
  styleUrls: ['./company-years-list.component.scss']
})
export class CompanyYearsListComponent extends BaseComponent {

  years!: Year[];

  companyYears: Year[] = []

  tableConfigurations!: TableConfigurations;

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog
  ) {
    super()
  }
  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.add().setTitle("افزودن سال مالی"),

    ]
  }

  async ngOnInit() {
    await this.resolve()
  }


  async resolve() {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.delete,
    ]
    let columns: TableColumn[] = [
      new TableColumn(
        'select',
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
        'سال مالی جاری است',
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
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    this.tableConfigurations.options.useBuiltInPagination = true;

    await this._mediator.send(new GetYearsQuery()).then((res) => {
      this.years = res.data;
    });
    await this.initialize()
  }

  async initialize() {
    this.form.controls.forEach((control:FormControl) => {
      let yearToAdd = this.years.find(x => x.id === +control.value);
      if (yearToAdd) this.companyYears.push(yearToAdd)
    })
      this.companyYears = [...this.companyYears];

    (this.form as FormArray).valueChanges.subscribe((newValue: number[]) => {
      this.companyYears = [];
      newValue.forEach(yearId => {
        let year = this.years.find(x => x.id === yearId)

        if (year)
          {
            this.companyYears.push(year)
          }
      })
    })
  }

  add(): any {
    let unSelectedYears: Year[] = [];
    this.years.forEach(year => {
      let control = this.form.controls.find((control: FormControl) => control.value === year.id)
      if (!control) {
        unSelectedYears.push(year);
      }
    })

    let dialogConfig = new MatDialogConfig()

    dialogConfig.data = {
      years: unSelectedYears
    }

    let dialogRef = this.dialog.open(CompanyYearsDialogComponent, dialogConfig)
    dialogRef.afterClosed().subscribe((yearIds: number[]) => {
      yearIds?.forEach(yearId => {
        this.form.controls.push(new FormControl(yearId))
      })
      this.form.patchValue(this.form.getRawValue())
      this.form.controls = [...this.form.controls]
    })
  }

  close(): any {
  }

  delete(): any {
    // @ts-ignore
    this.companyYears.filter(x => x.selected).forEach(year => {
      this.form.controls.splice(this.form.controls.indexOf(this.form.controls.find((x:FormControl) => x.value === year.id)),1)
    })
    // @ts-ignore
    this.years = this.years.map((x:Year) => {x.selected = false; return x} )
    this.form.patchValue(this.form.getRawValue())
  }

  get(id?: number): any {
  }


  update(entity?: any): any {
  }

}

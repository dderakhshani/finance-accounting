import {Component, Input, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {FormArray, FormControl} from "@angular/forms";
import {
  TableConfigurations
} from "../../../../../../core/components/custom/table/models/table-configurations";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {Year} from "../../../../entities/year";
import {forkJoin} from "rxjs";
import {GetYearsQuery} from "../../../../repositories/year/queris/get-years-query";
import {GetCompaniesQuery} from "../../../../repositories/company/queries/get-companies-query";
import {Company} from "../../../../entities/company";
import {FormActionTypes} from "../../../../../../core/constants/form-action-types";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {data} from "jquery";
import {AddUserYearDialogComponent} from "../add-user-year-dialog/add-user-year-dialog.component";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-user-year-list',
  templateUrl: './user-year-list.component.html',
  styleUrls: ['./user-year-list.component.scss']
})
export class UserYearListComponent extends BaseComponent {

  tableConfiguration!: TableConfigurations;

  years: Year[] = [];
  _userYears: Year[] = [];

  companies: Company[] = [];
  userCompanies: Company[] = [];

  @Input() set userYears(form: FormArray) {
    this.form = form;
    this.initialize()
  }

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog
  ) {
    super()
  }

  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.add().setTitle("افزودن شرکت"),
      PreDefinedActions.edit(),
      PreDefinedActions.refresh(),
      PreDefinedActions.delete(),
    ]
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {

    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.edit,
    ]
    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn('title', 'شرکت', TableColumnDataType.Text, '70%'),
    ]
    this.tableConfiguration = new TableConfigurations(columns, new TableOptions(false, true, undefined, true))

    forkJoin([
      this._mediator.send(new GetYearsQuery()),
      this._mediator.send(new GetCompaniesQuery())
    ]).subscribe(async ([
                          years,
                          companies
                        ]) => {
      this.years = years.data;
      this.companies = companies.data;
      await this.initialize()
    })
  }

  async initialize() {
    this.form.valueChanges.subscribe((yearIds: number[]) => {
      this._userYears = [];
      yearIds.forEach((yearId: number) => {
        let yearToAdd = this.years.find(x => x.id === yearId)
        if (yearToAdd) this._userYears.push(yearToAdd)
      })
      this.userCompanies = []
      this._userYears.forEach((year: Year) => {
        let companyToAdd = this.companies.find(x => x.id === year.companyId)
        if (companyToAdd && !this.userCompanies.includes(companyToAdd)) this.userCompanies.push(companyToAdd)
      })
    })
    this.form.patchValue(this.form.getRawValue())

  }


  add(param?: any): any {
    let dialogConfig = new MatDialogConfig()
    dialogConfig.data = {
      pageMode: PageModes.Add,
      selectableCompanies: this.companies.filter(x => !this.userCompanies.includes(x)),

      years: this.years,
      companies: this.companies
    }

    let dialogRef = this.dialog.open(AddUserYearDialogComponent, dialogConfig)

    dialogRef.afterClosed().subscribe(({companyId, yearIds}) => {
      let userYearIds: number[] = this.form.getRawValue();
      let insertedYearIds: number[] = yearIds.filter((x: number) => !userYearIds.includes(x));
      let deletedYearIds: number[] = userYearIds.filter((x: number) => !yearIds.includes(x));

      userYearIds = (userYearIds.filter((x) => !deletedYearIds.includes(x)) as number[]);
      userYearIds.push(...insertedYearIds)
      this.form.controls = userYearIds.map((x:number) => new FormControl(x))
      this.form.patchValue(this.form.getRawValue())
    })

  }


  update(param?: any): any {
    // @ts-ignore
    let companyToEdit = this.userCompanies.find(x => x.selected)
    if (companyToEdit) {
      let dialogConfig = new MatDialogConfig()
      dialogConfig.data = {
        pageMode: PageModes.Update,

        companyId: companyToEdit.id,
        userYears: this._userYears,

        years: this.years,
        companies: this.companies

      }

      let dialogRef = this.dialog.open(AddUserYearDialogComponent, dialogConfig);

      dialogRef.afterClosed().subscribe(({companyId, yearIds}) => {
        let userYearIds: number[] = this.form.getRawValue();
        let insertedYearIds: number[] = yearIds.filter((x: number) => !userYearIds.includes(x));
        let deletedYearIds: number[] = userYearIds.filter((x: number) => !yearIds.includes(x));

        userYearIds = (userYearIds.filter((x) => !deletedYearIds.includes(x)) as number[]);
        userYearIds.push(...insertedYearIds)
        this.form.controls = userYearIds.map((x:number) => new FormControl(x))
        this.form.patchValue(this.form.getRawValue())
      })

    }
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }


}

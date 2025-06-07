import {Component, Input} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../../../core/components/custom/table/models/table-configurations";
import {FormActionTypes} from "../../../../../../core/constants/form-action-types";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../repositories/base-value/queries/get-base-values-by-unique-name-query";
import {BaseValue} from "../../../../entities/base-value";
import {forkJoin} from "rxjs";
import {GetCountryDivisionsQuery} from "../../../../repositories/country-division/queries/get-country-divisions-query";
import {CountryDivision} from "../../../../entities/countryDivision";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {FormGroup} from "@angular/forms";
import {PersonAddressDialogComponent} from "./person-address-dialog/person-address-dialog.component";
import {PersonAddress} from "../../../../entities/person-address";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-person-addresses-list',
  templateUrl: './person-addresses-list.component.html',
  styleUrls: ['./person-addresses-list.component.scss']
})
export class PersonAddressesListComponent extends BaseComponent {

  addressTypes: BaseValue[] = [];
  countryDivisions: CountryDivision[] = [];
  tableConfigurations!: TableConfigurations;

  @Input() personId!:number;

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog
  ) {
    super();
  }
  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.add(),
      PreDefinedActions.edit(),
      PreDefinedActions.delete(),
    ]
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.edit,
      FormActionTypes.delete
    ]
    let columns = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),

      new TableColumn(
        'typeBaseId',
        'عنوان',
        TableColumnDataType.Text,
        '5%',
        false,
        undefined,
        (typeId: number) => {
          return this.addressTypes.find(x => x.id === typeId)?.title ?? ''
        }
      ),
      new TableColumn(
        'countryDivisionId',
        'شهر',
        TableColumnDataType.Text,
        '10%',
        false,
        undefined,
        (countryDivisionId: number) => {
          let countryDivision = this.countryDivisions.find(x => x.id === countryDivisionId)
          return countryDivision ? [countryDivision.ostanTitle, countryDivision.shahrestanTitle].join(' ، ') : ''
        }
      ),
      new TableColumn(
        'postalCode',
        'کد پستی',
        TableColumnDataType.Text,
        '5%'
      ),
      new TableColumn(
        'address',
        'آدرس',
        TableColumnDataType.Text,
        '25%'
      ),
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false,true,undefined,true))
    forkJoin([
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('AddressTitle')),
      this._mediator.send(new GetCountryDivisionsQuery())
    ]).subscribe(([
                    addressTypes,
                    countryDivisions
                  ]) => {
      this.addressTypes = addressTypes;
      this.countryDivisions = countryDivisions;
    })

  }

  async initialize() {

  }

  async add() {
    let dialogConfig = new MatDialogConfig()
    dialogConfig.data = {
      pageMode: PageModes.Add,
      personId: this.personId
    }

    let dialogRef = this.dialog.open(PersonAddressDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe((personAddress: PersonAddress) => {
      if (personAddress) {
        this.form.controls.push(this.createForm(personAddress))
        this.form.controls = [...this.form.controls]
      }
    })
  }

  async update(id?:number) {
    let personAddressToBeUpdated = id? this.form.getRawValue().find((x:any) => x.id === id) : this.form.getRawValue().find((x:any) => x?.selected)
    if (personAddressToBeUpdated) {
      let dialogConfig = new MatDialogConfig()
      dialogConfig.data = {
        pageMode: PageModes.Update,
        personAddress: personAddressToBeUpdated
      }

      let dialogRef = this.dialog.open(PersonAddressDialogComponent, dialogConfig);

      dialogRef.afterClosed().subscribe((personAddress: PersonAddress) => {
        if (personAddress) {
          this.form.controls.find((x:FormGroup) => x.getRawValue().id === personAddress.id).patchValue(personAddress)
          this.form.patchValue(this.form.getRawValue())
        }
      })
    }
  }

  async delete() {

  }

  get(): any {

  }

  close(): any {

  }
}

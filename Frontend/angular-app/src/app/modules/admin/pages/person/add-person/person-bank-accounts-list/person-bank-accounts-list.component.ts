import {Component, Input, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {BaseValue} from "../../../../entities/base-value";
import {
  TableConfigurations
} from "../../../../../../core/components/custom/table/models/table-configurations";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {FormActionTypes} from "../../../../../../core/constants/form-action-types";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../repositories/base-value/queries/get-base-values-by-unique-name-query";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {
  PersonAddressDialogComponent
} from "../person-addresses-list/person-address-dialog/person-address-dialog.component";
import {PersonAddress} from "../../../../entities/person-address";
import {FormGroup} from "@angular/forms";
import {PersonPhone} from "../../../../entities/person-phone";
import {PersonBankAccount} from "../../../../entities/person-bank-account";
import {PersonBankAccountDialogComponent} from "./person-bank-account-dialog/person-bank-account-dialog.component";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-person-bank-accounts-list',
  templateUrl: './person-bank-accounts-list.component.html',
  styleUrls: ['./person-bank-accounts-list.component.scss']
})
export class PersonBankAccountsListComponent extends BaseComponent {

  accountTypes: BaseValue[] = [];
  tableConfigurations!: TableConfigurations;
  @Input() personId!:number;

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog
  ) {
    super()
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
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
        'bankId',
        'بانک',
        TableColumnDataType.Text,
        '5%',
        false,
        undefined,
      ),
      new TableColumn(
        'bankBranchName',
        'شعبه بانک',
        TableColumnDataType.Text,
        '5%',
        false,
        undefined,
      ),
      new TableColumn(
        'accountTypeBaseId',
        'نوع حساب',
        TableColumnDataType.Text,
        '5%',
        false,
        undefined,
        (typeId: number) => {
          return this.accountTypes.find(x => x.id === typeId)?.title ?? ''
        }
      ),

      new TableColumn(
        'accountNumber',
        'شماره حساب',
        TableColumnDataType.Text,
        '5%'
      ),
      new TableColumn(
        'description',
        'توضیحات',
        TableColumnDataType.Text,
        '25%'
      ),
      new TableColumn(
        'isDefault',
        'پیش فرض',
        TableColumnDataType.CheckBox,
        '25%'
      ),
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, undefined, true))
    this.accountTypes = await this._mediator.send(new GetBaseValuesByUniqueNameQuery('AccountTypeBaseId'));

  }


  async add() {
    let dialogConfig = new MatDialogConfig()
    dialogConfig.data = {
      pageMode: PageModes.Add,
      personId: this.personId,
      accountTypes: this.accountTypes
    }

    let dialogRef = this.dialog.open(PersonBankAccountDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe((personBankAccount: PersonBankAccount) => {
      if (personBankAccount) {
        this.form.controls.push(this.createForm(personBankAccount))
        this.form.controls = [...this.form.controls]
      }
    })
  }

  async update(id?:number) {
    let personBankAccountToBeUpdated = id? this.form.getRawValue().find((x:any) => x.id === id) : this.form.getRawValue().find((x:any) => x?.selected)
    if (personBankAccountToBeUpdated) {
      let dialogConfig = new MatDialogConfig()
      dialogConfig.data = {
        pageMode: PageModes.Update,
        personAddress: personBankAccountToBeUpdated,
        accountTypes: this.accountTypes
      }

      let dialogRef = this.dialog.open(PersonBankAccountDialogComponent, dialogConfig);

      dialogRef.afterClosed().subscribe((personBankAccount: PersonBankAccount) => {
        if (personBankAccount) {
          this.form.controls.find((x:FormGroup) => x.getRawValue().id === personBankAccount.id).patchValue(personBankAccount)
          this.form.patchValue(this.form.getRawValue())
        }
      })
    }
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

  initialize(params?: any): any {
  }



}

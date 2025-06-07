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
import {forkJoin} from "rxjs";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../repositories/base-value/queries/get-base-values-by-unique-name-query";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {FormGroup} from "@angular/forms";
import {PersonPhoneDialogComponent} from "./person-phone-dialog/person-phone-dialog.component";
import {PersonPhone} from "../../../../entities/person-phone";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-person-phones-list',
  templateUrl: './person-phones-list.component.html',
  styleUrls: ['./person-phones-list.component.scss']
})
export class PersonPhonesListComponent extends BaseComponent {

  phoneTypes: BaseValue[] = [];
  tableConfigurations!: TableConfigurations;
  @Input() personId!:number;

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog
  ) {
    super()
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
        'phoneTypeBaseId',
        'عنوان',
        TableColumnDataType.Text,
        '5%',
        false,
        undefined,
        (typeId: number) => {
          return this.phoneTypes.find(x => x.id === typeId)?.title ?? ''
        }
      ),

      new TableColumn(
        'phoneNumber',
        'شماره',
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
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false,true,undefined,true))
    this.phoneTypes = await this._mediator.send(new GetBaseValuesByUniqueNameQuery('PhoneTitle'));

  }


  async add() {
    let dialogConfig = new MatDialogConfig()
    dialogConfig.data = {
      pageMode: PageModes.Add,
      personId: this.personId,
      phoneTypes: this.phoneTypes
    }

    let dialogRef = this.dialog.open(PersonPhoneDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe((personPhone: PersonPhone) => {
      if (personPhone) {
        this.form.controls.push(this.createForm(personPhone))
        this.form.controls = [...this.form.controls]
      }
    })
  }

  async update(id?:number) {
    let personPhoneToBeUpdated = id? this.form.getRawValue().find((x:any) => x.id === id) : this.form.getRawValue().find((x:any) => x?.selected)
    if (personPhoneToBeUpdated) {
      let dialogConfig = new MatDialogConfig()
      dialogConfig.data = {
        pageMode: PageModes.Update,
        phoneNumber: personPhoneToBeUpdated,
        phoneTypes: this.phoneTypes
      }

      let dialogRef = this.dialog.open(PersonPhoneDialogComponent, dialogConfig);

      dialogRef.afterClosed().subscribe((personPhone: PersonPhone) => {
        if (personPhone) {
          this.form.controls.find((x:FormGroup) => x.getRawValue().id === personPhone.id).patchValue(personPhone)
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

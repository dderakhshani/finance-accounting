import {Component, Inject, Input, Optional} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {BaseValue} from "../../../../../admin/entities/base-value";
import {forkJoin} from "rxjs";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {AccountHead} from "../../../../entities/account-head";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CreateAccountHeadCommand} from "../../../../repositories/account-head/commands/create-account-head-command";
import {UpdateAccountHeadCommand} from "../../../../repositories/account-head/commands/update-account-head-command";
import {TableConfigurations} from "../../../../../../core/components/custom/table/models/table-configurations";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {AccountHeadRelReferencesGroup} from "../../../../entities/account-head-rel-references-group";
import {AccountManagerService} from "../../../../services/account-manager.service";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {FormControl} from "@angular/forms";
import {
  AddAccountReferenceGroupToAccountHeadCommand
} from "../../../../repositories/account-head/commands/add-account-reference-group-to-account-head-command";
import {AccountReferencesGroup} from "../../../../entities/account-references-group";
import {group} from "@angular/animations";

@Component({
  selector: 'app-account-head-modal',
  templateUrl: './account-head-modal.component.html',
  styleUrls: ['./account-head-modal.component.scss']
})
export class AccountHeadModalComponent extends BaseComponent {
  @Input() set accountHead(accountHead:AccountHead) {
    this.entity = accountHead;
    this.initialize()
  }
  pageModes = PageModes;
  balanceTypes = [
    {
      id: 0,
      title: 'بدهکار - بستانکار'
    },
    {
      id: 1,
      title: 'بدهکار'
    },
    {
      id: 2,
      title: 'بستانکار'
    },
  ];
  transferTypes = [
    {
      id: 1,
      title: 'موقت'
    },
    {
      id: 2,
      title: 'دائم'
    },
  ];
  balances: BaseValue[] = [];
  currencies: BaseValue[] = [];
  tableConfigurations!: TableConfigurations;

  groupLevels = [
    {
      id: 1,
      title: 'چهارم'
    },
    {
      id: 2,
      title: 'پنجم'
    },
    {
      id: 3,
      title: 'ششم'
    }
  ]

  newGroupIdFormControl = new FormControl();
  newGroupLevelFormControl = new FormControl(this.groupLevels[0]?.id);

  accountReferencesGroups: AccountReferencesGroup[] = []

  constructor(
    private _mediator: Mediator,
    @Optional() private dialogRef: MatDialogRef<AccountHeadModalComponent>,
    public accountManagerService: AccountManagerService,
    @Optional() @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super()

    this.request = new CreateAccountHeadCommand();
    if (data) this.entity = data;
  }


  ngAfterViewInit() {
    if (this.actionBar) {
      this.actionBar.actions = [
        PreDefinedActions.add(),
        PreDefinedActions.delete()
      ]
    }
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {

    let tableColumns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn(
        'code',
        'کد',
        TableColumnDataType.Text,
        '20%',
        false,
        undefined,
        (rel: AccountHeadRelReferencesGroup) => {
          return this.accountManagerService.accountReferenceGroups.value.find(x => x.id === rel.referenceGroupId)?.code
        }
      ),
      new TableColumn(
        'referenceGroupId',
        'گروه',
        TableColumnDataType.Text,
        '20%',
        false,
        undefined,
        (rel: AccountHeadRelReferencesGroup) => {
          return this.accountManagerService.accountReferenceGroups.value.find(x => x.id === rel.referenceGroupId)?.title
        }
      ),
      new TableColumn(
        'referenceNo',
        'سطح',
        TableColumnDataType.Text,
        '20%',
        false,
        undefined,
        (rel: AccountHeadRelReferencesGroup) => {
          switch (rel.referenceNo) {
            case 1:
              return "چهارم"
            case 2:
              return "پنجم"
            case 3:
              return "ششم"
            default:
              return ""
          }
        }
      ),
    ]
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true, undefined, true))

    this.tableConfigurations.options.defaultSortKey = 'code'
    this.tableConfigurations.options.defaultSortDirection = 'ASC'
    this.accountManagerService.accountReferenceGroups.subscribe(newValue => {
      this.accountReferencesGroups = newValue.filter(x => x.isEditable && x.isVisible)
    })

    forkJoin([
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('CurrencyType')),
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('BalanceBase'))
    ]).subscribe(async ([
                          currencies,
                          balances
                        ]) => {
      this.currencies = currencies
      this.balances = balances
      await this.initialize()
    })
  }

  async initialize() {
    if (this.pageMode === this.pageModes.Read) {
      this.request = new UpdateAccountHeadCommand().mapFrom(this.entity)

      this.tableConfigurations.columns = [...this.tableConfigurations.columns.filter(x => x.name !== 'selected')]
      this.form.disable();
    }
    else if (this.entity.id) {
      this.request = new UpdateAccountHeadCommand().mapFrom(this.entity)
      this.pageMode = this.pageModes.Update
    } else {
      let request = new CreateAccountHeadCommand();
      request.balanceId = 0;
      request.transferId = 2;
      request.isActive = true;
      request.currencyBaseTypeId = this.currencies.find(x => x.uniqueName === 'IRR')?.id;
      request.balanceBaseId = this.balances.find(x => x.uniqueName === 'NonBalance')?.id;
      request.parentId = this.entity.parentId;
      request.parentCode = this.entity.parentCode;
      request.parentTitle = this.entity.parentTitle;
      this.request = request;
      this.pageMode = this.pageModes.Add
    }
    this.form.controls['parentCode']?.disable();
    this.form.controls['parentTitle']?.disable();
    if (this.pageMode === this.pageModes.Read) this.form.disable()
  }


  async add() {
    await this._mediator.send(<CreateAccountHeadCommand>this.request).then(res => {
      this.dialogRef.close(res)
    })
  }

  async update() {
    await this._mediator.send(<UpdateAccountHeadCommand>this.request).then(res => {
      this.dialogRef.close(res)
    })
  }

  removeGroups() {
    let groupsFormArray = this.form.controls['groups'];

    let groupsToRemove = groupsFormArray.controls.filter((x: any) => x.value.selected)
    groupsToRemove.forEach((group: AddAccountReferenceGroupToAccountHeadCommand) => {
      groupsFormArray.controls.indexOf(group)
      groupsFormArray.controls.splice(groupsFormArray.controls.indexOf(group), 1)
    })

    this.form.controls['groups'].patchValue(this.form.controls['groups'].getRawValue())
  }

  addGroup() {
    if (this.newGroupIdFormControl.value && this.newGroupLevelFormControl.value) {

      let groupsFormArray = this.form.controls['groups'];

      groupsFormArray.controls.push(this.createForm(new AddAccountReferenceGroupToAccountHeadCommand().mapFrom(<AccountHeadRelReferencesGroup>{
        referenceGroupId: this.newGroupIdFormControl.value,
        referenceNo: this.newGroupLevelFormControl.value
      })));
      groupsFormArray.patchValue(groupsFormArray.getRawValue())

      this.newGroupIdFormControl.setValue(null);
      this.newGroupLevelFormControl.setValue(this.groupLevels[0]?.id);
    }
  }

  groupDisplayFn(groupId: number) {
    return this.accountReferencesGroups.find(x => x.id === groupId)?.title
  };

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

  close(): any {
  }

  protected readonly PageModes = PageModes;
}

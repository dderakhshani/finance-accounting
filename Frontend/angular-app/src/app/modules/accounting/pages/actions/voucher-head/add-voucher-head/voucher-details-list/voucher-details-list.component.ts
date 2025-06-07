import {
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Input,
  OnDestroy,
  Output,
  Renderer2,
  ViewChild
} from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {
  CreateVoucherDetailCommand
} from "../../../../../repositories/voucher-detail/commands/create-voucher-detail-command";
import {TableConfigurations,} from "../../../../../../../core/components/custom/table/models/table-configurations";
import {AbstractControl, FormArray, FormGroup} from "@angular/forms";
import {VoucherDetail} from "../../../../../entities/voucher-detail";
import {AccountHead} from "../../../../../entities/account-head";
import {AccountReference} from "../../../../../entities/account-reference";

import {CodeRowDescription} from "../../../../../entities/code-row-description";
import {AccountReferencesGroup} from "../../../../../entities/account-references-group";
import {forkJoin} from "rxjs";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {
  GetCodeRowDescriptionsQuery
} from "../../../../../repositories/code-row-description/queries/code-row-descriptions-query";
import {BaseValue} from "../../../../../../admin/entities/base-value";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import {
  Action,
  ActionTypes,
  PreDefinedActions
} from "../../../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import {
  TableColumnFilterTypes
} from "../../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../../core/components/custom/table/models/table-column";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {
  CodeRowDescriptionDialogComponent
} from "../../../../base-values/code-row-description/code-row-description-dialog/code-row-description-dialog.component";
import {CodeVoucherGroup} from "../../../../../entities/code-voucher-group";
import {ActivatedRoute, Router} from "@angular/router";
import {GetMenuItemQuery} from "../../../../../../admin/repositories/menu-item/queries/get-menu-item-query";
import {TableComponent} from "../../../../../../../core/components/custom/table/table.component";
import {AccountManagerService} from "../../../../../services/account-manager.service";
import {
  MoveVoucherDetailsComponent
} from "../../../../special-operations/move-voucher-details/move-voucher-details.component";
import {de, el} from "date-fns/locale";
import {
  FilesByPaymentNumberComponent
} from "../../../../../../inventory/pages/component/files-by-payment-number/files-by-payment-number.component";
import {IdentityService} from "../../../../../../identity/repositories/identity.service";

@Component({
  selector: 'app-voucher-details-list',
  templateUrl: './voucher-details-list.component.html',
  styleUrls: ['./voucher-details-list.component.scss']
})
export class VoucherDetailsListComponent extends BaseComponent implements OnDestroy {
  @ViewChild(TableComponent) tableComponent!: TableComponent;
  @ViewChild('voucherDetailsTableWrapper') private voucherDetailsTableWrapper!: ElementRef;
  tableConfigurations!: TableConfigurations;
  currencyTypes: BaseValue[] = [];
  isCurrencyFormat: boolean = false;
  isCalculateByCurrency: boolean = false;
  codeRowDescriptions: CodeRowDescription[] = [];
  @Input() voucherHeadForm!: FormGroup;


  @Input() codeVoucherGroups: CodeVoucherGroup[] = [];
  @Input() codeVoucherGroupId!: number;
  autoFillNextArticle: boolean = true;
  @Output() voucherDetailSelected: EventEmitter<any | FormArray> = new EventEmitter<any | FormArray>();
  focusOrder = [
    'referenceId1',
    'accountReferencesGroupId',
    'accountHeadId',
    'voucherRowDescription',
    'debit',
    'credit'
  ]

  isVoucherDescriptionMinimized: boolean = true;

  focusFormInterval: any;
  tablePaginationChangeInterval: any;

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog,
    private router: Router,
    private accountManagerService: AccountManagerService,
    private route: ActivatedRoute,
    private renderer: Renderer2,
    public identityService: IdentityService
  ) {
    super(route, router);
  }


  ngAfterViewInit() {
    this.actionBar.actions = [
      //PreDefinedActions.add(),

      PreDefinedActions.add().setTitle('درج').setDisable(!this.identityService.doesHavePermission('VoucherHeadsList-Edit')),
      PreDefinedActions.delete().setDisable(!this.identityService.doesHavePermission('VoucherHeadsList-Edit')),
      new Action('افزودن شرح استاندارد', 'add', ActionTypes.custom, 'addCodeRowDescription'),
      new Action('سند مرتبط', 'visibility', ActionTypes.custom, 'showDocument'),
      new Action('نمایش مستندات', 'visibility', ActionTypes.custom, 'showDocumentFiles'),
      new Action('ریز گردش', 'money', ActionTypes.custom, 'ledger'),
      new Action('انتقال', 'arrow_back', ActionTypes.custom, 'moveVoucherDetails'),
      new Action('', 'arrow_downward', ActionTypes.custom, 'changeRowIndexDown').setDisable(!this.identityService.doesHavePermission('VoucherHeadsList-Edit')),
      new Action('', 'arrow_upward', ActionTypes.custom, 'changeRowIndexUp').setDisable(!this.identityService.doesHavePermission('VoucherHeadsList-Edit')),

    ]
  }

  initialize(): any {
  }

  @Input() set formSetter(form: FormArray) {
    this.form = form;

    if (this.getQueryParam('vdId') && this.form.controls.length > 0 && this.pageMode === PageModes.Update) {
      this.navigateToVoucherDetailPageAndFocusItsForm(+this.getQueryParam('vdId'));
    }
  }

  @Output() voucherDetailAdded = new EventEmitter<any>();
  @Output() voucherDetailUpdated = new EventEmitter<any>();
  @Output() voucherDetailsDeleted = new EventEmitter<any>();

  async ngOnInit() {


    await this.resolve();

  }
  ngOnDestroy() {
    super.ngOnDestroy()
  }


  async resolve() {

    this.addShortKeys();
    this.accountManagerService.accountHeads.subscribe(() => {
      if (this.tableConfigurations?.rowToEdit) this.handleColumnOptions(this.tableConfigurations.rowToEdit)
    })
    this.accountManagerService.accountReferences.subscribe(() => {
      if (this.tableConfigurations?.rowToEdit) this.handleColumnOptions(this.tableConfigurations.rowToEdit)
    })
    this.accountManagerService.accountReferenceGroups.subscribe(() => {
      if (this.tableConfigurations?.rowToEdit) this.handleColumnOptions(this.tableConfigurations.rowToEdit)
    })
    return forkJoin(
      [
        this._mediator.send(new GetBaseValuesByUniqueNameQuery('currencyType')),
        this.getCodeRowDescriptions()
      ]
    ).subscribe(([
                   currencyTypes,
                 ]) => {
      this.currencyTypes = currencyTypes;


      this.configTable()
    });
  };

  addShortKeys() {

    let windowRenderer = this.renderer.listen('window', 'keyup', (event) => {
      if (this.tabManagerService.activeTab.guid !== this.componentId) return;

      // Check if the Alt key is pressed
      if (event.altKey) {
        switch (event.code.toLowerCase()) {
          case 'keym':
            let voucherToEdit = this.form.controls.filter((x: any) => x.value.selected == true)[0] ?? this.form.controls[0];

            if (voucherToEdit) this.navigateToVoucherDetailPageAndFocusItsForm(0, voucherToEdit);
            else this.add();
            event.preventDefault();
            break;
          case 'keyn':
            this.add();
            break;
          case 'keyv':
            let selectedVoucherDetail = this.form.controls.find((x: any) => x.value.selected);
            if (selectedVoucherDetail) {
              let command = new CreateVoucherDetailCommand().mapFrom(selectedVoucherDetail.getRawValue())
              let newForm = <FormGroup>this.createForm(command, true)
              newForm.controls['rowIndex'].setValue(this.form.controls.length + 1);

              this.tableConfigurations.pagination.totalItems = this.form.controls.length;

              this.voucherDetailAdded.emit({form: newForm, command: command});
            }
            break;
          case 'delete':
            this.delete();
            break;
          case 'arrowup':
            if (this.tableConfigurations.rowToEdit) {
              let currentVoucherDetailIndex = this.form.controls.indexOf(this.tableConfigurations.rowToEdit)
              if (currentVoucherDetailIndex > 0) {

                let previousVoucherDetail = this.form.controls[currentVoucherDetailIndex - 1]
                this.navigateToVoucherDetailPageAndFocusItsForm(0, previousVoucherDetail, 'up')

              }
            }
            break;
          case 'arrowdown':
            if (this.tableConfigurations.rowToEdit) {
              let currentVoucherDetailIndex = this.form.controls.indexOf(this.tableConfigurations.rowToEdit)
              if (this.form.controls.length > currentVoucherDetailIndex + 1) {

                let nextVoucherDetail = this.form.controls[currentVoucherDetailIndex + 1]
                this.navigateToVoucherDetailPageAndFocusItsForm(0, nextVoucherDetail, 'down')
              }
            }
            break;
          case 'numpadadd':
            if (this.tableConfigurations.rowToEdit) {
              this.tableConfigurations.rowToEdit.patchValue({
                selected: true
              }, {emitEvent: false})
              let currentVoucherDetailIndex = this.form.controls.indexOf(this.tableConfigurations.rowToEdit)
              if (this.form.controls.length > currentVoucherDetailIndex + 1) {

                let nextVoucherDetail = this.form.controls[currentVoucherDetailIndex + 1]
                this.navigateToVoucherDetailPageAndFocusItsForm(0, nextVoucherDetail, 'down')
              }
            }
            break;
          case 'numpadsubtract':
            if (this.tableConfigurations.rowToEdit) {
              this.tableConfigurations.rowToEdit.patchValue({
                selected: false
              }, {emitEvent: false})
              let currentVoucherDetailIndex = this.form.controls.indexOf(this.tableConfigurations.rowToEdit)
              if (currentVoucherDetailIndex > 0) {

                let previousVoucherDetail = this.form.controls[currentVoucherDetailIndex - 1]
                this.navigateToVoucherDetailPageAndFocusItsForm(0, previousVoucherDetail, 'up')

              }
            }
            break;
          case 'keyu':
            if (this.tableConfigurations.rowToEdit != null) {
              this.tableConfigurations.rowToEdit.patchValue({
                credit: this.tableConfigurations.rowToEdit.value.debit,
                debit: this.tableConfigurations.rowToEdit.value.credit
              })
            }
            break;
          default:
            break;
        }

      }

    })

    this.windowEventListeners.push(windowRenderer)
  }

  async getCodeRowDescriptions() {
    await this._mediator.send(new GetCodeRowDescriptionsQuery()).then(res => {
      this.codeRowDescriptions = res.data;
    })
  }

  configTable() {
    let columns = [

      <TableColumn>{
        name: 'selected',
        title: '',
        type: TableColumnDataType.Select,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'rowIndex',
        title: 'ردیف',
        type: TableColumnDataType.Index,
        width: '2.5%',
        displayFn: (voucherDetail: FormGroup) => {
          return voucherDetail.getRawValue().rowIndex;
        }
      },

      new TableColumn(
        'referenceId1',
        'تفصیل',
        TableColumnDataType.AutoComplete,
        '5%',
        true,
        new TableColumnFilter('referenceId1', TableColumnFilterTypes.Text, true),
        (referenceId: number) => {
          let accountReference = this.accountManagerService.accountReferences.value?.find(x => x.id === referenceId);
          return accountReference ? accountReference.code : ''
        },
        false,
        true,
        // this.accountManagerService.accountReferences.value.filter(x => x.isActive),
        [],
        'id',
        ['code', 'title', 'employeeTitle'],
        (value: string, column: TableColumn) => {
          column.filteredOptions = (value && typeof value === 'string') ? column.options.filter(x => x.isActive)?.filter(x => x.title.includes(value) || x.code.includes(value) || x.employeeTitle?.includes(value)) : [];
        },
      ),
      new TableColumn(
        'accountReferencesGroupId',
        'گروه تفصیل',
        TableColumnDataType.AutoComplete,
        '5%',
        true,
        new TableColumnFilter('accountReferencesGroupId', TableColumnFilterTypes.Text, true),
        (accountReferencesGroupId: number) => {
          let accountReferencesGroup = this.accountManagerService.accountReferenceGroups.value?.find(x => x.id === accountReferencesGroupId);
          return accountReferencesGroup ? accountReferencesGroup.code : ''
        },
        false,
        true,
        this.accountManagerService.accountReferenceGroups.value,
        'id',
        ['code', 'title'],
        (value: string, column: TableColumn) => {
          column.filteredOptions = (value && typeof value === 'string') ? column.options?.filter(x => x.title.includes(value) || x.code.includes(value)) : column.options;
        },
      ),
      new TableColumn(
        'accountHeadId',
        'کد حساب',
        TableColumnDataType.AutoComplete,
        '5%',
        true,
        new TableColumnFilter('accountHeadId', TableColumnFilterTypes.Text, true),
        (accountHeadId: number) => {
          let selectedAccountHead = this.accountManagerService.accountHeads.value?.find(x => x.id === accountHeadId);
          return selectedAccountHead ? selectedAccountHead?.fullCode : '';
        },
        false,
        true,
        this.accountManagerService.accountHeads.value?.filter(x => x.lastLevel),
        'id',
        ['fullCode', 'title'],
        (value: string, column: TableColumn) => {
          column.filteredOptions = (value && typeof value === 'string') ? column.options?.filter(x => x.lastLevel)?.filter(x => x.title.includes(value) || x.fullCode.includes(value)) : column.options;
        },
      ),


      new TableColumn(
        'voucherRowDescription',
        'شرح',
        TableColumnDataType.AutoComplete,
        '30%',
        true,
        new TableColumnFilter('voucherRowDescription', TableColumnFilterTypes.Text),
        (voucherDetail: FormGroup) => {
          return voucherDetail?.value?.voucherRowDescription ?? (typeof voucherDetail == "string" ? voucherDetail : "");
        },
        false,
        true,
        this.codeRowDescriptions,
        'title',
        ['title'],
        (value: string, column: TableColumn) => {
          column.filteredOptions = (value && typeof value === 'string') ? column.options?.filter(x => x.title.includes(value)) : [];
        },
        [],
        undefined,
        <any>(localStorage.getItem('accounting.settings.voucherRowDescriptionLineStyle')) ?? 'onlyShowFirstLine',
        false,
      ),

      new TableColumn(
        'debit',
        'بدهکار',
        TableColumnDataType.Money,
        '7.5%',
        true,
        new TableColumnFilter('debit', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'credit',
        'بستانکار',
        TableColumnDataType.Money,
        '7.5%',
        true,
        new TableColumnFilter('credit', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'createdBy',
        'صادر کننده',
        TableColumnDataType.Text,
        '2%',
        true,
        new TableColumnFilter('createdBy', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'modifiedBy',
        'ویرایش کننده',
        TableColumnDataType.Text,
        '2%',
        true,
        new TableColumnFilter('modifiedBy', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'currencyAmount',
        'مقدار ارز',
        TableColumnDataType.Money,
        '7.5%',
        undefined,
        undefined,
        undefined,
        undefined,
        false,
      ),
      new TableColumn(
        'currencyFee',
        'نرخ ارز',
        TableColumnDataType.Money,
        '7.5%'
        ,
        undefined,
        undefined,
        undefined,
        undefined,
        false,
      ),

      new TableColumn(
        'currencyTypeBaseId',
        'نوع ارز',
        TableColumnDataType.DropDown,
        '5%',
        false,
        undefined,
        (id: number) => {
          return this.currencyTypes.find(x => x.id === id)?.title ?? ''
        },
        false,
        false,
        this.currencyTypes,
        'id',
        ['title'],
      ),
      new TableColumn(
        'quantity',
        'تعداد',
        TableColumnDataType.Number,
        '5%',
        undefined,
        undefined,
        undefined,
        undefined,
        false
      ),
      new TableColumn(
        'weight',
        'وزن',
        TableColumnDataType.Number,
        '5%',
        undefined,
        undefined,
        undefined,
        undefined,
        false
      ),
      new TableColumn(
        'traceNumber',
        'کد پیگیری',
        TableColumnDataType.Number,
        '5%',
        undefined,
        undefined,
        undefined,
        undefined,
        false)


    ]

    this.isVoucherDescriptionMinimized = localStorage.getItem('accounting.settings.voucherRowDescriptionLineStyle') == 'onlyShowFirstLine'
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(true, true));
    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.useBuiltInFilters = true;
    this.tableConfigurations.options.useBuiltInSorting = true;
    this.tableConfigurations.options.hasDefaultSortKey = true;
    this.tableConfigurations.options.defaultSortKey = 'rowIndex';
    this.tableConfigurations.options.defaultSortDirection = 'ASC';
    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.sumLabel = 'جمع کل';

    this.tableConfigurations.options.exportOptions.customExportButtonTitle = 'دانلود همه '
    this.tableConfigurations.options.exportOptions.customExportCallbackFn = (voucherDetails: FormGroup[]) => {
      return voucherDetails.map((x) => {
        let rawValue = x.getRawValue()
        return {
          'کد حساب': this.accountManagerService.accountHeads.value.find(y => y.id == rawValue.accountHeadId)?.code,
          'عنوان حساب': this.accountManagerService.accountHeads.value.find(y => y.id == rawValue.accountHeadId)?.title,
          'گروه حساب': this.accountManagerService.accountReferenceGroups.value.find(y => y.id == rawValue.accountReferencesGroupId)?.code,
          'عنوان گروه حساب': this.accountManagerService.accountReferenceGroups.value.find(y => y.id == rawValue.accountReferencesGroupId)?.title,
          'تفصیل شناور': this.accountManagerService.accountReferences.value.find(y => y.id == rawValue.referenceId1)?.code,
          'عنوان تفصیل شناور': this.accountManagerService.accountReferences.value.find(y => y.id == rawValue.referenceId1)?.title,
          'شرح': rawValue.voucherRowDescription,
          'بدهکار': rawValue.debit,
          'بستانکار': rawValue.credit,
        }
      })
    }
  }

  add(): any {
    this.tableComponent.removeAllFiltersAndSorts()

    let createCommand = new CreateVoucherDetailCommand();

    let newForm: any = this.createForm(createCommand, true);
    newForm.controls['rowIndex'].setValue(this.form.controls.length + 1);


    this.tableConfigurations.pagination.totalItems = this.form.controls.length;


    let that = this;
    setTimeout(() => {
      that.navigateToVoucherDetailPageAndFocusItsForm(0, newForm)
    }, 50)
    this.voucherDetailAdded.emit({form: newForm, command: createCommand});

  }

  delete(): any {
    let voucherDetailsToDelete = this.form.controls.filter((x: AbstractControl) => x.get('selected')?.value === true);
    this.voucherDetailsDeleted.emit(voucherDetailsToDelete)
  }


  handleCreditDebit(row: FormGroup) {
    let debit = row.controls['debit'];
    let credit = row.controls['credit'];
    if (credit?.value && !debit?.value) {
      debit?.disable({emitEvent: false});
      debit.setValue(undefined, {emitEvent: false})
    } else {
      debit?.enable({emitEvent: false})
    }

    if (debit?.value && !credit?.value) {
      credit?.disable({emitEvent: false})
      credit?.setValue(undefined, {emitEvent: false})
    } else {
      credit?.enable({emitEvent: false})
    }

    if (!debit?.value && !credit?.value) {
      credit?.enable({emitEvent: false})
      debit?.enable({emitEvent: false})
    }

    let accountHead = this.accountManagerService.accountHeads.value.find(x => x.id === row.controls['accountHeadId'].value)
    if (accountHead?.currencyFlag === true) {

      let currencyFee = row.controls['currencyFee'];
      let currencyAmount = row.controls['currencyAmount']
      if (!this.isCalculateByCurrency) {
        currencyFee?.disable({emitEvent: false})
        currencyAmount?.enable({emitEvent: false})
        if ((credit?.value || debit?.value) && currencyAmount?.value) {
          let currencyFeeNewValue = ((credit?.value ?? debit?.value) / currencyAmount?.value).toFixed(2)
          currencyFee?.setValue(currencyFeeNewValue, {emitEvent: false})
        }

      } else {
        if (credit?.value || debit?.value) {
          if (currencyAmount?.value || currencyFee?.value) {
            credit?.disable({emitEvent: false})
            debit?.disable({emitEvent: false})
          } else {
            credit?.enable({emitEvent: false})
            debit?.enable({emitEvent: false})
          }
          if (currencyAmount?.value && currencyFee?.value) {
            let newCreditOrDebitValue = Math.ceil(currencyAmount?.value * currencyFee?.value)
            if (credit?.value) credit?.setValue(newCreditOrDebitValue, {emitEvent: false});
            if (debit?.value) debit?.setValue(newCreditOrDebitValue, {emitEvent: false});
          }
        }
        currencyFee?.enable({emitEvent: false})
        currencyAmount?.enable({emitEvent: false})


      }
    }
    if(!(this.identityService.doesHavePermission('VoucherHeadsList-Add') || this.identityService.doesHavePermission('VoucherHeadsList-Add'))) {
      row.disable({emitEvent: false})
      row.controls['selected'].enable({emitEvent: false})
    }
  }

  handleAccountHeadSelection(row: FormGroup) {

    this.setVoucherDetailAccountHeadLevels(row);
    this.setCurrencyTypeBaseId(row)
    this.handleAutoFill(row);
    this.handleColumnOptions(row);
    this.handleCurrencyAllowance(row)
    if(!(this.identityService.doesHavePermission('VoucherHeadsList-Add') || this.identityService.doesHavePermission('VoucherHeadsList-Add'))) {
      row.disable({emitEvent: false})
      row.controls['selected'].enable({emitEvent: false})
    }
  }

  setVoucherDetailAccountHeadLevels(row: FormGroup) {
    let level1: any;
    let level2: any;
    let level3: any = this.accountManagerService.accountHeads.value.find(x => x.id === row.getRawValue().accountHeadId);
    if (level3 && level3?.parentId) level2 = this.accountManagerService.accountHeads.value.find(x => x.id === level3?.parentId);
    if (level2 && level2?.parentId) level1 = this.accountManagerService.accountHeads.value.find(x => x.id === level2?.parentId);

    row.patchValue({
      level1: level1?.id,
      level2: level2?.id,
      level3: level3?.id,
    }, {emitEvent: false})
  }

  setCurrencyTypeBaseId(row: FormGroup) {
    if (!row.controls['currencyTypeBaseId']?.value) {

      let accountHead = this.accountManagerService.accountHeads.value.find(x => x.id === row.getRawValue()?.accountHeadId);
      if (accountHead) {
        if (!this.isCurrencyFormat) {
          row.controls['currencyTypeBaseId']?.setValue(this.currencyTypes.find(x => x.uniqueName === 'IRR')?.id, {emitEvent: false});
        } else {
          row.controls['currencyTypeBaseId']?.setValue(accountHead.currencyBaseTypeId, {emitEvent: false});
        }
      }
    }
  }

  handleAutoFill(row: FormGroup) {
    let accountHead = <AccountHead>(this.accountManagerService.accountHeads.value.find(x => x.id === row.getRawValue()?.accountHeadId));

    let currentArticleDataIsNotAlreadyFilledButAccountHeadIsSelected = accountHead && !row.getRawValue()?.voucherRowDescription && !row.getRawValue()?.debit && !row.getRawValue()?.credit
    let voucherDetails: VoucherDetail[] = (<FormArray>this.form).getRawValue();
    let previousVoucherDetail = voucherDetails[voucherDetails.length - 2];
    if (this.autoFillNextArticle && currentArticleDataIsNotAlreadyFilledButAccountHeadIsSelected && previousVoucherDetail) {

      row.get('voucherRowDescription')?.setValue(previousVoucherDetail.voucherRowDescription, {emitEvent: false});
      row.controls['credit'].setValue(previousVoucherDetail.debit, {emitEvent: false});
      row.controls['debit'].setValue(previousVoucherDetail.credit, {emitEvent: false});
      previousVoucherDetail.debit ? row.controls['debit'].disable({emitEvent: false}) : row.controls['credit'].disable({emitEvent: false});
    }
  }

  handleColumnOptions(row: FormGroup) {

    let accountHead = <AccountHead>(this.accountManagerService.accountHeads.value.find(x => x.id === row.getRawValue()?.accountHeadId));
    let accountReference = <AccountReference>(this.accountManagerService.accountReferences.value.find(x => x.id === row.getRawValue()?.referenceId1));
    let accountReferencesGroup = <AccountReferencesGroup>(this.accountManagerService.accountReferenceGroups.value.find(x => x.id === row.getRawValue()?.accountReferencesGroupId));


    let accountHeadColumn = <TableColumn>(this.tableConfigurations.columns.find(x => x.name === 'accountHeadId'));
    let accountReferenceColumn = <TableColumn>(this.tableConfigurations.columns.find(x => x.name === 'referenceId1'));
    let accountReferencesGroupColumn = <TableColumn>(this.tableConfigurations.columns.find(x => x.name === 'accountReferencesGroupId'));

    let allOfTheFieldsAreSelected = (!!accountHead && !!accountReference && !!accountReferencesGroup);

    if (allOfTheFieldsAreSelected) {
      accountHeadColumn.options = [];
      accountReferenceColumn.options = [];
      accountReferencesGroupColumn.options = [];
      return;
    }
    let noneOfTheFieldsAreSelected = !accountHead && !accountReference && !accountReferencesGroup;
    if (noneOfTheFieldsAreSelected) {
      accountHeadColumn.options = this.accountManagerService.accountHeads.value.filter(x => x.lastLevel);
      accountReferenceColumn.options = this.accountManagerService.accountReferences.value.filter(x => x.isActive);
      accountReferencesGroupColumn.options = this.accountManagerService.accountReferenceGroups.value;

      accountHeadColumn.filterOptionsFn(undefined, accountHeadColumn)
      accountReferenceColumn.filterOptionsFn(undefined, accountReferenceColumn)

      row.controls['accountHeadId']?.enable({emitEvent: false})
      row.controls['accountReferencesGroupId']?.disable({emitEvent: false})
      row.controls['referenceId1']?.enable({emitEvent: false})
      return;
    }


    let onlyAccountHeadSelected = accountHead && !accountReference && !accountReferencesGroup;
    let accountHeadAndGroupSelected = accountHead && accountReferencesGroup && !accountReference;

    let onlyAccountReferenceSelected = accountReference && !accountHead && !accountReferencesGroup;
    let accountReferenceAndGroupSelected = accountReference && !accountHead && accountReferencesGroup;


    if (onlyAccountHeadSelected) {
      accountReferencesGroupColumn.options = this.accountManagerService.getGroupsRelatedToAccountHead(accountHead.id);

      row.controls['referenceId1']?.disable({emitEvent: false})
      if (accountReferencesGroupColumn.options.length > 0) row.controls['accountReferencesGroupId']?.enable({emitEvent: false})
      else row.controls['accountReferencesGroupId']?.disable({emitEvent: false})
    }

    if (accountHeadAndGroupSelected) {
      accountReferenceColumn.options = this.accountManagerService.getAccountReferencesRelatedToGroup(accountReferencesGroup.id).filter(x => x.isActive)
      row.controls['referenceId1']?.enable({emitEvent: false});
    }

    if (onlyAccountReferenceSelected) {
      accountReferencesGroupColumn.options = this.accountManagerService.getGroupsRelatedToAccountReference(accountReference.id)
      row.controls['accountReferencesGroupId']?.enable({emitEvent: false})
      row.controls['accountHeadId']?.disable({emitEvent: false})
      if (accountReferencesGroupColumn.options.length === 0) row.controls['accountReferencesGroupId']?.disable({emitEvent: false})
    }

    if (accountReferenceAndGroupSelected) {
      accountHeadColumn.options = this.accountManagerService.getAccountHeadsRelatedToGroup(accountReferencesGroup.id).filter(x => x.lastLevel)
      row.controls['accountHeadId']?.enable({emitEvent: false});
    }
  }

  handleCurrencyAllowance(row: FormGroup) {
    let accountHead = <AccountHead>(this.accountManagerService.accountHeads.value.find(x => x.id === row.getRawValue()?.accountHeadId));

    let currencyFeeControl = row.controls['currencyFee'];
    let currencyAmountControl = row.controls['currencyAmount'];
    let currencyTypeBaseIdControl = row.controls['currencyTypeBaseId'];

    if (accountHead && accountHead?.currencyFlag === true) {
      currencyFeeControl.enable({emitEvent: false})
      currencyAmountControl.enable({emitEvent: false})
      currencyTypeBaseIdControl.enable({emitEvent: false})
    } else {
      currencyFeeControl.disable({emitEvent: false})
      currencyAmountControl.disable({emitEvent: false})
      currencyTypeBaseIdControl.disable({emitEvent: false})
    }
  }


  handleFormKeydown(event: any) {
    if (event.key === 'Enter' || event.key === 'NumpadEnter') {
      let form = this.tableConfigurations.rowToEdit;
      if (event.columnName !== 'credit' && event.columnName !== 'debit') {
        let fieldToFocusIndex = this.focusOrder.indexOf(event.columnName) + 1

        while (form.controls[this.focusOrder[fieldToFocusIndex]].nativeElement?.disabled === true) {
          fieldToFocusIndex++;
        }
        form.controls[this.focusOrder[fieldToFocusIndex]].nativeElement.focus();
      } else {
        if (event.columnName === 'debit') {
          if (form.controls['debit'].value > 0) {
            console.log(this.form.controls.length - 1 !== this.form.controls.indexOf(form))
            if (this.form.controls.length - 1 !== this.form.controls.indexOf(form)) {
              let formToFocus = this.form.controls[this.form.controls.indexOf(form) + 1];
              console.log(formToFocus);
              return this.navigateToVoucherDetailPageAndFocusItsForm(0, formToFocus, 'down')
            } else return this.add()
          } else form.controls['credit'].nativeElement.focus();

        } else if (event.columnName === 'credit') {
          if (this.form.controls.length - 1 !== this.form.controls.indexOf(form)) {
            let formToFocus = this.form.controls[this.form.controls.indexOf(form) + 1];
            console.log(formToFocus);
            return this.navigateToVoucherDetailPageAndFocusItsForm(0, formToFocus, 'down')
          } else return this.add()
        }
      }
    }
  }

  handleEscape(row: FormGroup) {
    if (!row.value.accountHeadId && !row.value.id) {

      this.voucherDetailsDeleted.emit([row]);

      // for (let i = this.form.controls.length - 1; i >= 0; i--) {
      //   this.form.controls[i].controls['rowIndex'].setValue(i + 1);
      // }
      this.form.controls = [...this.form.controls]
    }
  }

  async handleCustomActions(action: Action) {
    if (action.uniqueName === 'addCodeRowDescription') {
      this.addCodeRowDescription();
    }
    if (action.uniqueName === 'showDocument') {
      await this.showDocument()
    }
    if (action.uniqueName === 'showDocumentFiles') {
      let selectedVoucherDetails = this.form.value.filter((x: any) => x.selected && x.id);
      if (selectedVoucherDetails[0]?.financialOperationNumber) {
        let dialogConfig = new MatDialogConfig();
        dialogConfig.data = selectedVoucherDetails[0]?.financialOperationNumber;
        this.dialog.open(FilesByPaymentNumberComponent, dialogConfig)
      }

    }
    if (action.uniqueName === 'ledger') {
      let articleToShowLedger = this.form.getRawValue().filter((x: any) => x.selected === true && x.accountHeadId)[0];
      this.router.navigateByUrl('/accounting/reporting/ledgerReport?' + `accountHeadId=${articleToShowLedger.accountHeadId}&` + `accountReferenceId=${articleToShowLedger.referenceId1}&` + `accountReferenceGroupId=${articleToShowLedger.accountReferencesGroupId}`)
    }
    if (action.uniqueName === 'moveVoucherDetails') {
      let dialogConfig = new MatDialogConfig();
      let selectedVoucherDetails = this.form.value.filter((x: any) => x.selected && x.id);

      if (selectedVoucherDetails?.length > 0) {
        dialogConfig.data = selectedVoucherDetails;

        let dialogRef = this.dialog.open(MoveVoucherDetailsComponent, dialogConfig)

        dialogRef.afterClosed().subscribe((isSuccess) => {
          if (isSuccess === true) {
            selectedVoucherDetails.forEach((voucher: VoucherDetail) => {
              let formControlToRemove = this.form.controls.find((x: FormGroup) => x.value.id == voucher.id)
              this.form.controls.splice(this.form.controls.indexOf(formControlToRemove), 1)
            })
            this.form.controls = [...this.form.controls]
            this.tableComponent.selectAllControl.setValue(false)
          }
        })
      }
    }

    if (action.uniqueName === 'changeRowIndexDown') this.changeVoucherDetailIndex('down')
    if (action.uniqueName === 'changeRowIndexUp') this.changeVoucherDetailIndex('up')
  }

  addCodeRowDescription() {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(CodeRowDescriptionDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(async () => {
      await this.getCodeRowDescriptions();
      let voucherRowDescriptionColumn = this.tableConfigurations.columns.find(x => x.name === 'voucherRowDescription')
      if (voucherRowDescriptionColumn) voucherRowDescriptionColumn.options = this.codeRowDescriptions;
    })
  }

  async showDocument() {
    let documentListToShow = this.form.getRawValue().filter((x: any) => x.selected === true && x.documentId);
    let codeVoucherGroup = this.codeVoucherGroups.find(x => x.id === this.codeVoucherGroupId);
    let menu = await this._mediator.send(new GetMenuItemQuery(codeVoucherGroup?.menuId ?? 0))
    if (menu) {
      let queryMappings = JSON.parse(menu.queryParameterMappings)
      for (let i = 0; i < documentListToShow.length; i++) {
        const voucherDetail: any = documentListToShow[i];
        let params = "?";
        queryMappings.forEach((x: string) => {
          let entityName = x.split(':')[0].split('.')[0];
          let entityPropertyName = x.split(':')[0].split('.')[1];
          let queryParamName = x.split(':')[1];
          let queryParamValue = ""
          if (entityName.toLowerCase() === 'voucherhead') {
            queryParamValue = `${this.voucherHeadForm.value[entityPropertyName]}`
          }
          if (entityName.toLowerCase() === 'voucherdetail') {
            queryParamValue = `${voucherDetail[entityPropertyName]}`
          }
          params += `${queryParamName}=${queryParamValue}&`;
        })

        if (params?.trim()?.endsWith('&')) params = params.substring(0, params.length - 1)

        if (!menu.formUrl.includes('http')) await this.router.navigateByUrl(menu?.formUrl + params);
        else window.open(menu.formUrl + params, '_blank');
      }
    }
  }

  handleRowUpdate(row: any,src?:string) {
    if (row) {
      row.controls['createdBy'].disable({emitEvent: false})
      row.controls['modifiedBy'].disable({emitEvent: false})
      if (this.tableConfigurations.rowToEdit !== row) {
        this.navigateToVoucherDetailPageAndFocusItsForm(0, row,'down')
      }
      this.handleAccountHeadSelection(row);
      this.handleCreditDebit(row);
      if (src !== 'table' && src !== 'auto-navigate')  this.voucherDetailUpdated.emit(row)

      this.tableConfigurations.changeRowToEdit(null)
      row.updateValueAndValidity({onlySelf: false, emitEvent: true})
      this.tableConfigurations.changeRowToEdit(row,this.handleRowUpdate.bind(this))
    }
  }

  showCurrencyRelatedFields(show: boolean) {
    let currencyColumns = this.tableConfigurations.columns.filter(x => x.name === 'currencyTypeBaseId' || x.name === 'currencyAmount' || x.name === 'currencyFee')
    currencyColumns.forEach((x) => {
      x.show = show
    })
    this.isCurrencyFormat = show;
  }

  changeVoucherDetailIndex(direction: "up" | "down") {
    let selectedRow = this.form.controls.find((x: any) => x.value.selected);
    //@ts-ignore
    let selectedIndex = this.form.controls.findIndex(a => a.value.selected == true);
    let length = this.form.controls.length;
    let temp = this.form.controls[selectedIndex];
    if (direction == 'up' && selectedIndex > 0) {
      let i;
      for (i = selectedIndex; i >= selectedIndex - 1; i--) {
        this.form.controls[i] = this.form.controls[i - 1];
      }
      this.form.controls[selectedIndex - 1] = temp;
      this.form.controls = [...this.form.controls]
    } else if (direction == 'down' && selectedIndex < length - 1) {
      let i;
      for (i = selectedIndex; i <= selectedIndex + 1; i++) {
        //this.form.controls[i].controls['rowIndex'].setValue(selectedRow.value.rowIndex)
        this.form.controls[i] = this.form.controls[i + 1];
      }
      this.form.controls[selectedIndex + 1] = temp;
      this.form.controls = [...this.form.controls]
    }

    let otherRow = this.form.controls.find((x: any) => x.value.rowIndex == selectedRow.value.rowIndex + (direction == "up" ? -1 : 1))
    if (otherRow) {
      let selectedRowNewIndex = otherRow.value.rowIndex;
      let otherRowNewIndex = selectedRow.value.rowIndex;
      selectedRow.controls['rowIndex'].setValue(selectedRowNewIndex)
      otherRow.controls['rowIndex'].setValue(otherRowNewIndex)
      this.form.controls = [...this.form.controls]
      this.voucherDetailUpdated.emit(selectedRow)
      this.voucherDetailUpdated.emit(otherRow)
    }
  }

  update(entity: any): any {
  }

  updateChanges(entity: any, request: any) {
  }

  get(id: number): any {
  }

  close(): any {
  }


  changeDescriptionColumnStyle() {
    let descriptionColumn = <TableColumn>this.tableConfigurations.columns.find(x => x.name === 'voucherRowDescription')
    descriptionColumn.lineStyle = descriptionColumn?.lineStyle === 'default' ? 'onlyShowFirstLine' : 'default';
    localStorage.setItem('accounting.settings.voucherRowDescriptionLineStyle', descriptionColumn.lineStyle)
  }


  navigateToVoucherDetailPageAndFocusItsForm(voucherDetailId: number, voucherDetailForm?: FormGroup, direction?: 'up' | 'down') {
    clearInterval(this.tablePaginationChangeInterval);
    clearInterval(this.focusFormInterval);


    let voucherDetailToFocus = voucherDetailForm ?? this.form.controls.find((x: FormGroup) => x.value.id === voucherDetailId);

    this.tableConfigurations.changeRowToEdit(voucherDetailToFocus, this.handleRowUpdate.bind(this));
    this.handleRowUpdate(voucherDetailForm,'auto-navigate')


    this.tablePaginationChangeInterval = setInterval(this.checkAndChangePagination.bind(this), 250, voucherDetailToFocus);
    this.focusFormInterval = setInterval(this.checkAndFocus.bind(this), 350, voucherDetailToFocus, direction);


  }

  checkAndChangePagination(voucherDetailToFocus: any) {

    if (this.tableConfigurations) {
      let voucherDetailPageIndex = Math.ceil(this.form.controls.indexOf(voucherDetailToFocus) / this.tableConfigurations.pagination.pageSize);
      this.tableConfigurations.pagination.pageIndex = voucherDetailPageIndex > 0 ? voucherDetailPageIndex - 1 : voucherDetailPageIndex;
      if (!voucherDetailToFocus.controls['id'].value) this.tableComponent.scrollToBottom()
      // Clear the interval once the value is found and focus is set
      clearInterval(this.tablePaginationChangeInterval);

      this.tableComponent.setRows();

      this.voucherDetailSelected.emit(voucherDetailToFocus);

    }
  };

  checkAndFocus(voucherDetailToFocus: any, direction: 'up' | 'down') {
    const element = voucherDetailToFocus.controls[this.focusOrder[0]].nativeElement;

    if (element) {
      element.scrollIntoView({
        behavior: 'auto',
        block: 'center',
        inline: 'center'
      })
      element.focus();
      // Clear the interval once the value is found and focus is set
      clearInterval(this.focusFormInterval);

    }
  };


}


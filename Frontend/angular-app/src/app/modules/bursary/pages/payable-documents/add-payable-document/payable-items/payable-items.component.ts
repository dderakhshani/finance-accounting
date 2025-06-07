import {
  ChangeDetectorRef,
  Component, EventEmitter,
  Input,
  OnChanges,
 Output,
  SimpleChanges,
  TemplateRef,
  ViewChild
} from '@angular/core';
import {BaseTable} from "../../../../../../core/abstraction/base-table";
import {Column,} from "../../../../../../core/components/custom/table/models/column";
import {
  TableScrollingConfigurations
} from "../../../../../../core/components/custom/table/models/table-scrolling-configurations";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {DecimalFormat} from "../../../../../../core/components/custom/table/models/decimal-format";
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {
  TableColumnFilterTypes
} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {
  TablePaginationOptions
} from "../../../../../../core/components/custom/table/models/table-pagination-options";
import {BehaviorSubject} from "rxjs";

import {AccountHead} from "../../../../../accounting/entities/account-head";
import {AccountReference} from "../../../../../accounting/entities/account-reference";
import {AccountReferencesGroup} from "../../../../../accounting/entities/account-references-group";
import {AccountManagerService} from "../../../../../accounting/services/account-manager.service";
import {Toastr_Service} from "../../../../../../shared/services/toastrService/toastr_.service";
import {
  Action, ActionBarComponent,
  ActionTypes,
  PreDefinedActions
} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {Router} from "@angular/router";
import { FormArray, FormControl} from "@angular/forms";
import {documentPayOrders} from "../../../../entities/documentPayOrders";
import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {
  GetDocumentPayOrdersListQuery
} from "../../../../repositories/payables-documents/queries/get-document-payOrders-list";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {ToPersianDatePipe} from "../../../../../../core/pipes/to-persian-date.pipe";

@Component({
  selector: 'app-payable-items',
  templateUrl: './payable-items.component.html',
  styleUrls: ['./payable-items.component.scss']
})
export class PayableItemsComponent extends BaseTable implements OnChanges {




  @ViewChild('actionBarPayOrders') actionBarPayOrders!: ActionBarComponent
  @ViewChild('actionBarG') actionBarG!: ActionBarComponent
  @Output() rowChanged: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('inputAmount', {read: TemplateRef}) inputAmount!: TemplateRef<any>;

  @ViewChild('inputReferenceId', {read: TemplateRef}) inputReferenceId!: TemplateRef<any>;
  @ViewChild('inputReferenceGroupId', {read: TemplateRef}) inputReferenceGroupId!: TemplateRef<any>;
  @ViewChild('inputAccountHeadId', {read: TemplateRef}) inputAccountHeadId!: TemplateRef<any>;

  payOrdersFormControl = new FormControl(null)
  tableConfigurationsPayOrders!: TableScrollingConfigurations;
  columnsPayOrders!: Column[]
  documentPayOrdersList: documentPayOrders[] = [];
  rowDataPayOrders: documentPayOrders[] = []
  customNoItemsText: string = 'موردی یافت نشد'
  accountHeads$: BehaviorSubject<AccountHead[]> = new BehaviorSubject<AccountHead[]>([]);
  accountReferenceGroups$: BehaviorSubject<AccountReferencesGroup[]> = new BehaviorSubject<AccountReferencesGroup[]>([]);
  accountReferences$: BehaviorSubject<AccountReference[]> = new BehaviorSubject<AccountReference[]>([]);
  private isHandlingInvalid: boolean = false;

  @Input() set formSetter(form: FormArray) {
    this.form = form;
  }
  @Input() formAccounts!: any;
  @Input()  formPayOrders!: any;

  constructor(private cdr: ChangeDetectorRef,
              protected accountManagerService: AccountManagerService,
              private router: Router,
              private _mediator: Mediator,
              private toPersianDate: ToPersianDatePipe,
              private toastr: Toastr_Service,) {
    super();
  }

  async ngOnInit(): Promise<void> {
    this.initialize();
    await this.resolve();
    this.accountManagerService.init().catch(error => {
      console.error('Error initializing account data:', error);
    });
    this.updateDataAutoComplete()

  }

  async ngOnChanges(changes: SimpleChanges) {
    if (changes['formAccounts'] && this.formAccounts) {
      this.rowData = this.formAccounts.value;
    }
    if (changes['formPayOrders'] && this.formPayOrders) {
    }
  }

  ngAfterViewInit() {
    super.ngAfterViewInit();
    this.actionBarG.actions = [
      PreDefinedActions.add().setTitle('درج'),
      PreDefinedActions.delete(),
      new Action('ریز گردش', 'money', ActionTypes.custom, 'ledger'),
    ]
    this.actionBarPayOrders.actions = [

      PreDefinedActions.delete(),

    ]
    const columnAccountHeadId = this.tableConfigurations.columns.find((col: any) => col.field === 'accountHeadId');
    if (columnAccountHeadId) {
      columnAccountHeadId.template = this.inputAccountHeadId
    }
    const columnReferenceGroupId = this.tableConfigurations.columns.find((col: any) => col.field === 'referenceGroupId');
    if (columnReferenceGroupId) {
      columnReferenceGroupId.template = this.inputReferenceGroupId
    }
    const columnReferenceId = this.tableConfigurations.columns.find((col: any) => col.field === 'referenceId');
    if (columnReferenceId) {
      columnReferenceId.template = this.inputReferenceId
    }
    const columnAmount = this.tableConfigurations.columns.find((col: any) => col.field === 'amount');
    if (columnAmount) {
      columnAmount.template = this.inputAmount
    }


  }

  initialize(params?: any): any {

    this.columns = [
      {
        ...this.defaultColumnSettings,
        index: 0,
        field: 'selected',
        title: '#',
        width: 1,
        type: TableColumnDataType.Select,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
        digitsInfo: DecimalFormat.Default,
        groupRemainingNameOrFn: 'جمع',
        groupRemainingId: 'title',
      },
      {
        ...this.defaultColumnSettings,
        index: 1,
        field: 'rowIndex',
        title: 'ردیف',
        width: 1,
        type: TableColumnDataType.Index,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
        groupRemainingNameOrFn: 'جمع',
        groupRemainingId: 'title',

      },
      {
        ...this.defaultColumnSettings,
        index: 2,
        field: 'accountHeadId',
        title: ' حساب',
        width: 4,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('accountHeadId', TableColumnFilterTypes.Text),
        type: TableColumnDataType.Template,
        template: this.inputAccountHeadId,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'title',
        optionsTitleKey: ['fullCode', 'title'],
        groupRemainingNameOrFn: 'جمع',
        groupRemainingId: 'title',
      }, {
        ...this.defaultColumnSettings,
        index: 3,
        field: 'referenceGroupId',
        title: 'گروه تفضیل',
        width: 4,

        lineStyle: 'default',
        filter: new TableColumnFilter('referenceGroupId', TableColumnFilterTypes.Text),
        type: TableColumnDataType.Template,
        template: this.inputReferenceGroupId,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'title',
        optionsTitleKey: ['code', 'title'],
        groupRemainingNameOrFn: 'جمع',
        groupRemainingId: 'title',
      },
      {
        ...this.defaultColumnSettings,
        index: 4,
        field: 'referenceId',
        title: ' تفصیل',
        width: 4,

        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('referenceId', TableColumnFilterTypes.Text),
        type: TableColumnDataType.Template,
        template: this.inputReferenceId,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'title',
        optionsTitleKey: ['code', 'title'],
        groupRemainingNameOrFn: 'جمع',
        groupRemainingId: 'title',
      },
      {
        ...this.defaultColumnSettings,
        index: 5,
        field: 'amount',
        title: ' مبلغ',
        width: 4,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('amount', TableColumnFilterTypes.Text),
        type: TableColumnDataType.Template,
        template: this.inputAmount,
        groupRemainingNameOrFn: () => {
          return this.calculateSumColumn('amount')
        },
        groupRemainingId: 'sum',
        sumColumnValue: 0,
        showSum: true,
      },
    ]
    this.columnsPayOrders = [
      {
        ...this.defaultColumnSettings,
        index: 0,
        field: 'selected',
        title: '#',
        width: 1,
        type: TableColumnDataType.Select,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
        digitsInfo: DecimalFormat.Default,
      },
      {
        ...this.defaultColumnSettings,
        index: 1,
        field: 'rowIndex',
        title: 'ردیف',
        width: 1,
        type: TableColumnDataType.Index,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',

      },
      {
        ...this.defaultColumnSettings,
        index: 2,
        field: 'bankAccountName',
        title: 'بانک',
        width: 4,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('accountHeadId', TableColumnFilterTypes.Text),
        type: TableColumnDataType.Text,


      }, {
        ...this.defaultColumnSettings,
        index: 3,
        field: 'documentId',
        title: 'شناسه سند',
        width: 4,
        lineStyle: 'default',
        filter: new TableColumnFilter('documentId', TableColumnFilterTypes.Number),
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.None,
      },
      {
        ...this.defaultColumnSettings,
        index: 4,
        field: 'payOrderId',
        title: 'شناسه دستور پرداخت',
        width: 4,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('payOrderId', TableColumnFilterTypes.Number),
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.None,

      },
      {
        ...this.defaultColumnSettings,
        index: 5,
        field: 'payOrderAmount',
        title: ' مبلغ',
        width: 4,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('payOrderAmount', TableColumnFilterTypes.Money),
        type: TableColumnDataType.Money,
        showSum: true,
      },
      {
        ...this.defaultColumnSettings,
        index: 6,
        field: 'payOrderDate',
        title: ' تاریخ',
        width: 4,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('payOrderDate', TableColumnFilterTypes.Date),
        type: TableColumnDataType.Date,
      },
    ]
    this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(), new TablePaginationOptions(),
      this.toolBar);

    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.useBuiltInFilters = true;
    this.tableConfigurations.options.useBuiltInSorting = true;
    this.tableConfigurations.options.showGroupRemainingRow = true;
    this.tableConfigurationsPayOrders = new TableScrollingConfigurations(this.columnsPayOrders, new TableOptions(), new TablePaginationOptions(),
      this.toolBar);
    this.tableConfigurationsPayOrders.options.useBuiltInPagination = true;
    this.tableConfigurationsPayOrders.options.useBuiltInFilters = true;
    this.tableConfigurationsPayOrders.options.useBuiltInSorting = true;
    this.tableConfigurationsPayOrders.options.showSumRow = true;


  }

  async resolve(params?: any): Promise<any> {
    this.documentPayOrdersList = await this.getDocumentPayOrdersList()
    this.documentPayOrdersList = this.documentPayOrdersList.map((item: any, index: number) => ({
      ...item,
      payOrderDate: this.toPersianDate.transform(item.payOrderDate),
    }))

    this.rowDataPayOrders = this.formPayOrders.value.map((item: any) => {
      if (item.payOrderId) {
        const foundItem = this.documentPayOrdersList.find((doc: any) => doc.id === item.payOrderId);
        if (foundItem) {
          return {
            ...item,
            ...foundItem,
          }
        }
      }
    })
    this.tableConfigurationsPayOrders.options.isExternalChange = true;
  }

  async getDocumentPayOrdersList(value?: any): Promise<any[]> {
    const searchQueries: SearchQuery[] = [
      new SearchQuery({
        propertyName: 'PayOrders',
        values: [value],
        comparison: 'equal',
        nextOperand: 'and'
      })
    ];

    const request = new GetDocumentPayOrdersListQuery(0, 0);

    try {
      return await this._mediator.send(request);
    } catch (error) {
      console.error('خطا در دریافت دستور پرداخت:', error);
      return [];
    }
  }

  async get(id?: number, action?: string) {

  }

  getSelectedItems(): any[] {
    return this.rowData.filter(item => item.selected === true)
  }

  calculateSumColumn(field: string): string {
    const parseNumber = (value: any): number => {
      if (typeof value === 'number') return value;
      if (!value) return 0;
      const cleaned = String(value)
        .replace(/[^\d.-]/g, '')
        .replace(/(\..*)\./g, '$1');
      return parseFloat(cleaned) || 0;
    };
    if (!this.rowData || this.rowData.length === 0) return '0';
    const selectRows = this.getSelectedItems();
    const dataSource = selectRows.length > 0 ? selectRows : this.rowData;
    const sum = dataSource.reduce((acc, row) => {
      const value = parseNumber(row[field]);
      return acc + value;
    }, 0);
    return sum.toString()
  }


  handleTableConfigurationsChange(config: TableScrollingConfigurations) {
    this.tableConfigurations.columns = config.columns;
    this.columns = config.columns;
    this.tableConfigurations.options = config.options;
  }

  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }


  update(param?: any): any {
  }


  changeRow($event: Event, row: any) {
    this.formAccounts.clear()
    this.rowData.forEach(item => {
      this.formAccounts.push(new FormControl(item));
    });
  }


  trackByFnNgSelect(index: number, item: any) {
    return item ? item.id || item : index;
  }

  onChange(obj: any, context: { row: any }) {
    const {row} = context;



    if (!row) {
      this.handleNewRow();
      return;
    }
    const accountHead = this.accountManagerService.accountHeads.value.find(x => x.id === row.accountHeadId) ?? null;
    const accountReference = this.accountManagerService.accountReferences.value.find(x => x.id === row.referenceId) ?? null;
    const accountReferencesGroup = this.accountManagerService.accountReferenceGroups.value.find(x => x.id === row.referenceGroupId) ?? null;
    const allSelected = !!accountHead && !!accountReference && !!accountReferencesGroup;
    const noneSelected = !accountHead && !accountReference && !accountReferencesGroup;

    if (allSelected) {
      this.handleAllSelectedState(row);
      return;
    }

    if (noneSelected) {
      this.handleNoneSelectedState(row);
      return;
    }
    this.handleIntermediateStates(accountHead, accountReference, accountReferencesGroup, row);
    this.formAccounts.clear()
    this.rowData.forEach(item => {
      this.formAccounts.push(new FormControl(item));
    });

  }

  private handleAllSelectedState(row: any) {

    const accountHead = this.accountManagerService.accountHeads.value.filter(x => x.id === row.accountHeadId)?? [];
    const accountReference = this.accountManagerService.accountReferences.value.filter(x => x.id === row.referenceId) ?? [];
    const accountReferencesGroup = this.accountManagerService.accountReferenceGroups.value.filter(x => x.id === row.referenceGroupId) ?? [];
    this.accountReferenceGroups$.next(accountReferencesGroup);
    this.accountReferences$.next(accountReference);
    this.accountHeads$.next(accountHead);
    row.isEditableAccountHeads = this.accountHeads$.value.length > 0;
    row.isEditableAccountReferences = this.accountReferences$.value.length > 0;
    row.isEditableAccountReferenceGroups = this.accountReferenceGroups$.value.length > 0;

    this.tableConfigurations.options.isExternalChange = true
  }


  private handleNewRow() {
    this.accountHeads$.next(this.accountManagerService.accountHeads.value.filter(x => x.lastLevel));
    this.accountReferences$.next(this.accountManagerService.accountReferences.value.filter(x => x.isActive));
    this.accountReferenceGroups$.next(this.accountManagerService.accountReferenceGroups.value);
  }

  private handleNoneSelectedState(row: any) {

    this.accountHeads$.next(this.accountManagerService.accountHeads.value.filter(x => x.lastLevel));
    this.accountReferences$.next(this.accountManagerService.accountReferences.value.filter(x => x.isActive));
    this.accountReferenceGroups$.next(this.accountManagerService.accountReferenceGroups.value);
    if (!row) return
    row.isEditableAccountHeads = this.accountManagerService.accountHeads.value.filter(x => x.lastLevel).length > 0;
    row.isEditableAccountReferences = this.accountManagerService.accountReferences.value.filter(x => x.isActive).length > 0;
    row.isEditableAccountReferenceGroups = false;
  }

  private handleIntermediateStates(
    accountHead: AccountHead | null,
    accountReference: AccountReference | null,
    accountReferencesGroup: AccountReferencesGroup | null,
    row: any
  ) {
    if (accountHead && accountReference && !accountReferencesGroup) {
      const relatedGroups = this.accountManagerService.getGroupsRelatedToBoth(
        accountHead.id,
        accountReference.id
      );
      this.accountReferenceGroups$.next(relatedGroups);
      if (relatedGroups.length === 1) {
        if (row.referenceGroupId !== relatedGroups[0].id) {
          row.referenceGroupId = relatedGroups[0].id;
          this.onChange({}, {row});
        }
      }
      row.isEditableAccountReferenceGroups = true;
      return;
    }
    if (accountHead && !accountReference && !accountReferencesGroup) {
      const groups = this.accountManagerService.getGroupsRelatedToAccountHead(+accountHead.id);
      this.accountReferenceGroups$.next(groups);
      row.isEditableAccountReferenceGroups = groups.length > 0;
      if (groups.length > 0) row.isEditableAccountReferences = true;
      row.isEditableAccountReferences = false;
      return;
    }

    if (accountHead && accountReferencesGroup && !accountReference) {
      const refs = this.accountManagerService.getAccountReferencesRelatedToGroup(accountReferencesGroup.id)
        .filter(x => x.isActive);
      this.accountReferences$.next(refs);
      row.isEditableAccountReferences = refs.length > 0;
      return;
    }

    if (accountReference && !accountHead && !accountReferencesGroup) {
      const groups = this.accountManagerService.getGroupsRelatedToAccountReference(accountReference.id);
      this.accountReferenceGroups$.next(groups);
      row.isEditableAccountReferenceGroups = groups.length > 0;
      row.isEditableAccountHeads = false;

      return;
    }

    if (accountReference && accountReferencesGroup && !accountHead) {
      const heads = this.accountManagerService.getAccountHeadsRelatedToGroup(accountReferencesGroup.id).filter(x => x.lastLevel);
      if (heads.length === 0) {
        this.toastr.showToast({title: "خطا", message: "هیچ سرفصل حسابی مرتبط با این گروه یافت نشد.", type: "error"});
        return;
      }
      this.accountHeads$.next(heads);
      row.isEditableAccountHeads = true;

      return;
    }

    console.warn('Unexpected state combination:', {accountHead, accountReference, accountReferencesGroup});
  }


  onClear(context: { row: any, column: Column }) {
    const {row, column} = context;
    this.updateRelatedFields(column);
    this.onChange({}, {row})
  }

  searchWithColumnFn(column: any) {
    return (term: string, item: any) => {
      return this.customSearchFn(term, item, column);
    };
  }

  customSearchFn(term: string, item: any, col: Column): boolean {
    if (!term || !item || !col.optionsTitleKey) {
      return false;
    }
    const normalizedTerm = term.trim().toLowerCase();
    return col.optionsTitleKey.some((key: string) => {
      const value = this.getValueByKey(item, key);
      return value && value.toLowerCase().includes(normalizedTerm);
    });
  }

  getValueByKey(item: any, key: string): string {
    return key.split('.').reduce((obj, keyPart) => obj?.[keyPart], item) || '';
  }


  getOptionTitleParts(option: any, titleKeys: string[]): { mainTitle: string, subTitle: string } {

    const mainTitle = titleKeys[0] && option[titleKeys[0]] ? option[titleKeys[0]] : '';
    const subTitle = titleKeys[1] && option[titleKeys[1]] ? option[titleKeys[1]] : '';
    return {mainTitle, subTitle};
  }


  addRowDebit() {
    let newRow = {
      accountHeadId: null,
      referenceGroupId: null,
      referenceId: null,
      amount: null,
      isEditableAccountHeads: true,
      isEditableAccountReferences: true,
      isEditableAccountReferenceGroups: false,
      isEditableAmount: true,
      isEditableRow: true,
    }
    // غیرفعال کردن ردیف‌های قبلی
    this.rowData = this.rowData.map(row => ({
      ...row,
      isEditableAccountHeads: false,
      isEditableAccountReferences: false,
      isEditableAccountReferenceGroups: false,
      isEditableAmount: false,
      isEditableRow: false,
    }));
    this.rowData = [...this.rowData, newRow];
    this.updateDataAutoComplete();
    // @ts-ignore
    this.onChange({}, {newRow});
    this.formAccounts.clear()
    this.rowData.forEach(item => {
      this.formAccounts.push(new FormControl(item));
    });
    this.tableConfigurations.options.isExternalChange = true;

  }

  deleteRowDebit() {

    const selectedRows = this.rowData.filter((row: any) => row.selected);
    if (selectedRows.length > 0) {

      this.rowData = this.rowData.filter((row: any) => !row.selected);
      this.toastr.showToast({title: "موفق", message: "ردیف های انتخاب شده با موفقیت حذف شدند", type: "success"});
      this.tableConfigurations.options.isExternalChange = true;
      this.formAccounts.clear()
      this.rowData.forEach(item => {
        this.formAccounts.push(new FormControl(item));
      });
    } else {
      this.toastr.showToast({title: "خطا", message: "هیچ ردیفی انتخاب نشده است", type: "error"});
    }
  }

  async handleCustomActions(action: Action) {
    if (action.uniqueName === 'ledger') {
      let articleToShowLedger = this.rowData.find((row: any) => row.selected);
      if (articleToShowLedger) {
        await this.router.navigateByUrl('/accounting/reporting/ledgerReport2?' + `accountHeadId=${articleToShowLedger.accountHeadId}&` + `accountReferenceId=${articleToShowLedger.referenceId}&` + `accountReferenceGroupId=${articleToShowLedger.referenceGroupId}`)
      } else {
        this.toastr.showToast({title: "خطا", message: "هیچ ردیفی انتخاب نشده است", type: "error"});
      }
    }

  }


  editableSelectRow(row: any) {
    if (!row) return;
    const selectedRowId = row.rowIndex;

    this.rowData = this.rowData.map(r => {
      const isSelectedRow = r.rowIndex === selectedRowId;


      const hasNoIds = !r.accountHeadId && !r.referenceId && !r.referenceGroupId;

      let isEditableHeads, isEditableRefs, isEditableGroups;

      if (isSelectedRow && hasNoIds) {

        isEditableHeads = true;
        isEditableRefs = true;
        isEditableGroups = false;
      } else {

        isEditableHeads = !!r.accountHeadId;
        isEditableRefs = !!r.referenceId;
        isEditableGroups = !!r.referenceGroupId;
      }

      return {
        ...r,
        isEditableAccountHeads: isEditableHeads,
        isEditableAccountReferences: isEditableRefs,
        isEditableAccountReferenceGroups: isEditableGroups,
        isEditableAmount: isSelectedRow,
        isEditableRow: isSelectedRow,
      };
    });

    this.updateDataAutoComplete();
    this.onChange({}, {row});
    this.tableConfigurations.options.isExternalChange = true;
  }

  private updateRelatedFields(col: Column) {
    if (col.field == 'accountHeadId') {

      this.accountHeads$.next(this.accountManagerService.accountHeads.value);
    } else if (col.field == 'referenceGroupId') {
      this.accountReferenceGroups$.next(this.accountManagerService.accountReferenceGroups.value);
    } else if (col.field == 'referenceId') {
      this.accountReferences$.next(this.accountManagerService.accountReferences.value);
    } else {
      this.updateDataAutoComplete()
    }

  }

  private updateDataAutoComplete() {
    this.accountHeads$ = new BehaviorSubject([...this.accountManagerService.accountHeads.value]);
    this.accountReferenceGroups$ = new BehaviorSubject([...this.accountManagerService.accountReferenceGroups.value]);
    this.accountReferences$ = new BehaviorSubject([...this.accountManagerService.accountReferences.value]);
  }

  parseFloat(val: any): any {
    if (typeof val == 'number') {
      return val;
    }
    if (val != null)
      return parseFloat(val.replace(/,/g, ''));
    else
      return null;
  }

  ngOnDestroy() {
    this.accountHeads$.complete();
    this.accountReferenceGroups$.complete();
    this.accountReferences$.complete();

  }

  addRowPayOrder() {


  }

  deletePayOrder() {
    this.resetPayOrderField();
    if (this.rowDataPayOrders.length === 0) {
      this.toastr.showToast({type: 'warning', message: 'هیچ دستور پرداختی برای حذف وجود ندارد'});
      return;
    }

    const selectedPayOrders = this.rowDataPayOrders.filter((row: any) => row.selected === true);

    if (selectedPayOrders.length === 0) {
      this.toastr.showToast({type: 'warning', message: 'هیچ دستور پرداختی انتخاب نکردید'});
      return;
    }

    this.rowDataPayOrders = this.rowDataPayOrders.filter((row: any) => !row.selected);
    this.tableConfigurationsPayOrders.options.isExternalChange = true;
    const payOrderIds = this.rowDataPayOrders.map((row: any) => row.payOrderId);
    this.formPayOrders.clear()
    payOrderIds.forEach(id => {
      this.formPayOrders.push(new FormControl(id));
    });

    this.toastr.showToast({type: 'success', message: `${selectedPayOrders.length} دستور پرداخت حذف شد`});

  }

  resetPayOrderField() {
    this.payOrdersFormControl.reset();
  }


  handleFieldChange(documentId: any) {
    const foundItem = this.documentPayOrdersList.find(x => x.id === documentId);

    if (!foundItem) return;

    const entity: documentPayOrders & { selected: boolean; rowIndex: number } = {
      selected: false,
      rowIndex: this.rowDataPayOrders.length + 1,
      ...foundItem,
    };
    this.rowDataPayOrders = [...this.rowDataPayOrders, entity];
    const payOrderIds = this.rowDataPayOrders.map((row: any) => row.payOrderId);
    this.formPayOrders.clear()
    payOrderIds.forEach(id => {
      this.formPayOrders.push(new FormControl(id));
    });
    this.tableConfigurationsPayOrders.options.isExternalChange = true;

  }

  documentPayOrdersDisplayFn(id: number): string {
    if (!id) return '';

    const payOrder = this.documentPayOrdersList.find(x => x.id === id);
    if (!payOrder) return '';

    const codePart = payOrder.bankAccountName ?? `(${payOrder.payOrderNo})`;
    return [codePart]
      .filter(part => part)
      .join(' ')
      .trim();
  }
}

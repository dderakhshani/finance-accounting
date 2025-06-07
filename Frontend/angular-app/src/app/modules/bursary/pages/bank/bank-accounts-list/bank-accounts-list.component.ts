import {AfterViewInit, Component, OnInit, TemplateRef, ViewChild} from '@angular/core';
import {ToolBar} from "../../../../../core/components/custom/table/models/tool-bar";
import {FontFamilies} from "../../../../../core/components/custom/table/models/font-families";
import {FontWeights} from "../../../../../core/components/custom/table/models/font-weights";
import {Column, FilterCondition, TypeFilterOptions} from "../../../../../core/components/custom/table/models/column";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {DecimalFormat} from "../../../../../core/components/custom/table/models/decimal-format";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {
  TableScrollingConfigurations
} from "../../../../../core/components/custom/table/models/table-scrolling-configurations";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TablePaginationOptions} from "../../../../../core/components/custom/table/models/table-pagination-options";
import {PrintOptions} from "../../../../../core/components/custom/table/models/print_options";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";

import {
  Action, ActionBarComponent,
  ActionTypes,
  PreDefinedActions
} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {BankAccounts} from "../../../entities/bank-accounts";
import {GetBankAccounts} from "../../../repositories/bank-accounts/queries/get-bank-accounts";

import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {number} from "echarts";
import {filter} from "rxjs/operators";
import {AccountReference} from "../../../../accounting/entities/account-reference";
import {
  GetAccountReferencesQuery
} from "../../../../accounting/repositories/account-reference/queries/get-account-references-query";
import {forkJoin} from "rxjs";
import {GetBanksQuery} from "../../../../chocolate-factory-bursary/repositories/bank/queries/get-banks-query";
import {Bank} from "../../../../chocolate-factory-bursary/entities/bank";
import {
  GetBankBranchesQuery
} from "../../../../chocolate-factory-bursary/repositories/bank-branch/queries/get-bank-branches-query";
import {BankBranch} from "../../../../chocolate-factory-bursary/entities/bank-branch";
import {
  GetBankAccountTypesQuery
} from "../../../../chocolate-factory-bursary/repositories/bank-account/queries/get-bank-account-types-query";
import {BankAccountTypes} from "../../../../chocolate-factory-bursary/entities/bank-account-type";
import {GetLedgerReportQuery} from "../../../../accounting/repositories/reporting/queries/get-ledger-report-query";
import {BaseTable} from "../../../../../core/abstraction/base-table";

@Component({
  selector: 'app-bank-accounts-list',
  templateUrl: './bank-accounts-list.component.html',
  styleUrls: ['./bank-accounts-list.component.scss']
})
export class BankAccountsListComponent extends BaseTable {

  @ViewChild('rowBtn1', {read: TemplateRef}) rowBtn1!: TemplateRef<any>;

  requestsIndex: number = -1;
  bankAccounts: BankAccounts[] = [];


  accountReferences: AccountReference[] = [];
  banks: Bank[] = [];
  bankBranches: BankBranch[] = [];
  bankAccountTypes: BankAccountTypes[] = [];

  constructor(
    private _mediator: Mediator,
    private router: Router, public dialog: MatDialog
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve()
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.add().setTitle('افزودن بانک'),
    ];
    const column = this.tableConfigurations.columns.find((col: any) => col.field === 'btn');
    if (column) {
      column.template = this.rowBtn1
    }

  }

  async resolve() {

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
        field: 'accountNumber',
        title: 'حساب',
        width: 4.5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('accountNumber', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      }

      , {
        ...this.defaultColumnSettings,
        index: 4,
        field: 'accountTypeBaseTitle',
        title: 'نوع حساب',
        width: 3.5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('accountTypeBaseTitle', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',

        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        filteredOptions: this.bankAccountTypes,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'title',
        optionsTitleKey: ['title'],
      }, {
        ...this.defaultColumnSettings,
        index: 5,
        field: 'sheba',
        title: 'شبا',
        width: 3.5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('sheba', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,

      },
      {
        ...this.defaultColumnSettings,
        index: 6,
        field: 'bankTitle',
        title: 'بانک ',
        width: 4,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('bankTitle', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        filteredOptions: this.banks,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'title',
        optionsTitleKey: ['code', 'title'],


      },
      {
        ...this.defaultColumnSettings,
        index: 7,
        field: 'bankBranchTitle',
        title: 'شعبه',
        width: 4,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('bankBranchTitle', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        filteredOptions: this.bankBranches,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'title',
        optionsTitleKey: ['code', 'title'],
      },

      {
        ...this.defaultColumnSettings,
        index: 8,
        field: 'referenceId',
        title: 'کد تفصیل',
        width: 3,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.None,
        filter: new TableColumnFilter('referenceId', TableColumnFilterTypes.Number),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
        filteredOptions: this.accountReferences,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'code',
        optionsTitleKey: ['code', 'title'],
        filterOptionsFn: async (query: string) => {
          try {
            const accountReferences = await this.getAccountReferences(query);
            const column = this.tableConfigurations.columns.find((col: any) => col.field === 'referenceId');
            if (column) {
              column.filteredOptions = accountReferences;
            }
            return accountReferences;
          } catch (error) {
            console.error('Error fetching account references groups:', error);
            return [];
          }
        },

      },
      {
        ...this.defaultColumnSettings,
        index: 9,
        field: 'title',
        title: 'عنوان تفصیل',
        width: 8,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('title', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        filteredOptions: this.accountReferences,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'title',
        optionsTitleKey: ['code', 'title'],
        filterOptionsFn: async (query: string) => {
          try {
            const accountReferences = await this.getAccountReferences(query);
            const column = this.tableConfigurations.columns.find((col: any) => col.field === 'title');
            if (column) {
              column.filteredOptions = accountReferences;
            }
            return accountReferences;
          } catch (error) {
            console.error('Error fetching account references groups:', error);
            return [];
          }
        },

      },

      {
        ...this.defaultColumnSettings,
        index: 10,
        field: 'btn',
        title: 'عملیات',
        width: 6,
        type: TableColumnDataType.Template,
        template: this.rowBtn1,
        sortable: false,
        lineStyle: 'onlyShowFirstLine',
        print: false
      }

    ]
    this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(false, true), new TablePaginationOptions(), this.toolBar, new PrintOptions('گزارش حساب های بانکی'))
    this.tableConfigurations.options.usePagination = true;

    this.tableConfigurations.options.showFilterRow = true;
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.options.itemSize = 40;
    this.tableConfigurations.options.exportOptions.showExportButton = true;
    this.tableConfigurations.pagination.pageSize = 500;

    forkJoin([
      this.getAccountReferences(),
      this.getBanks(),
      this.getBankBranch(),
      this.getBankAccountTypes()
    ]).subscribe(async ([accountReferences, banks, bankBranches, bankAccountTypes]) => {
      this.updateColumnFilteredOptions('accountTypeBaseTitle', bankAccountTypes);
      this.updateColumnFilteredOptions('bankBranchTitle', bankBranches);
      this.updateColumnFilteredOptions('bankTitle', banks);
      this.updateColumnFilteredOptions('title', accountReferences);
      this.updateColumnFilteredOptions('referenceId', accountReferences);

      await this.get();
    })
    // this.isLoading = false;

  }

  updateColumnFilteredOptions(columnField: string, data: any[]) {
    const column = this.tableConfigurations.columns.find((col: any) => col.field === columnField);
    if (column) {
      column.filteredOptions = [...data];

    }
  }

  async get(id?: number, action?: string) {
    let searchQueries: SearchQuery[] = JSON.parse(JSON.stringify(this.excludedRows));
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
    if (this.filterConditionsInputSearch) {
      Object.keys(this.filterConditionsInputSearch).forEach(key => {
        const filter = this.filterConditionsInputSearch[key];
        if (filter && filter.propertyNames && filter.searchValues && filter.searchValues[0]) {
          filter.propertyNames.forEach((propertyName: string) => {
            searchQueries.push(new SearchQuery({
              propertyName: propertyName,
              values: filter.searchValues,
              comparison: filter.searchCondition,
              nextOperand: filter.nextOperand
            }));
          });
        }
      });
    }
    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }
    let request = new GetBankAccounts(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    if (action == 'back') {

      this.requestsList.pop();
      this.requestsIndex--;
      if (this.requestsList.length > 1) {

        let temp = <GetBankAccounts>JSON.parse(this.requestsList[this.requestsList.length - 1]);
        request = this.createRequest(temp);
        this.request = request;


      } else {

        this.resetRequest();
        return;
      }
    } else {
      this.requestsList.push(JSON.stringify(request));
      this.requestsIndex++;
    }
    this.tableConfigurations.options.isLoadingTable = true;
    await this._mediator.send(request).then((response: any) => {

      this.bankAccounts = response.objResult.data;
      response.objResult.totalCount && (this.tableConfigurations.pagination.totalItems = response.objResult.totalCount);
      this.tableConfigurations.options.isLoadingTable = false;

    }).catch(() => {
      this.tableConfigurations.options.isLoadingTable = false;
    });
  }


  createRequest(temp: GetBankAccounts) {
    let request = new GetBankAccounts();
    request.pageIndex = temp.pageIndex;
    request.pageSize = temp.pageSize;
    request.conditions = temp.conditions;
    request.orderByProperty = temp.orderByProperty;


    return request
  }

  resetRequest() {
    const temp = new GetBankAccounts();
    const request = this.createRequest(temp);
    this.request = request;

    this.selectedItemsFilterForPrint = [];
    this.excludedRows = []
    this.filterConditionsInputSearch = {};
    this.requestsList = [];
    this.requestsIndex = -1;

    setTimeout(() => {
      this.get().then();
    }, 0);
  }

  async getAccountReferences(query?: string) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries = [
        new SearchQuery({
          propertyName: 'code',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'title',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        })
      ]
    }
    return await this._mediator.send(new GetAccountReferencesQuery(1, 50, searchQueries, "code")).then(res => res.data)
  }

  async getBanks(query?: string) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries = [
        new SearchQuery({
          propertyName: 'code',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'title',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        })
      ]
    }
    return await this._mediator.send(new GetBanksQuery(0, 0, searchQueries, "title")).then(res => res)
  }

  async getBankBranch(query?: string) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries = [
        new SearchQuery({
          propertyName: 'code',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'title',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        })
      ]
    }
    return await this._mediator.send(new GetBankBranchesQuery(0, 0, searchQueries, "title")).then(res => res)
  }

  async getBankAccountTypes(query?: string) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries = [
        new SearchQuery({
          propertyName: 'code',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'title',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        })
      ]
    }
    return await this._mediator.send(new GetBankAccountTypesQuery(0, 0, searchQueries, "title")).then(res => res)
  }

  add() {

  }

  close(): any {
  }

  delete(param?: any): any {
  }

  initialize(params?: any): any {
  }

  update(param?: any): any {
  }

  async showDetail(row: any) {
    await this.router.navigateByUrl(`bursary/bank/chequeBook?bankAccountId=${row.id}`)
  }

  navigateToChequeBooks($event: any) {

  }

  handleRowsSelected($event: any) {

  }

  handleOptionSelected(event: { typeFilterOptions: any, query: any }) {

    this.tableConfigurations.pagination.pageIndex = 0;
    if (event.typeFilterOptions == TypeFilterOptions.NgSelect) {
      this.updateNgSelectFilters(event.query).then()
    }
    if (event.typeFilterOptions == TypeFilterOptions.NumberInputSearch || event.typeFilterOptions == TypeFilterOptions.TextInputSearch) {
      this.updateInputSearchFilters(event.query).then()
    }


  }

  async updateInputSearchFilters(filterConditions: { [key: string]: any }) {

    this.filterConditionsInputSearch = {...this.filterConditionsInputSearch, ...filterConditions};
    await this.get()

  }

  async updateNgSelectFilters(filterConditions: { [key: string]: FilterCondition }) {
    let filterCondition = Object.values(filterConditions).map((item: FilterCondition) => ({
      ...item,
      searchValues: Array.isArray(item.searchValues) ? item.searchValues : [item.searchValues],
      propertyNames: Array.isArray(item.propertyNames) ? item.propertyNames : [item.propertyNames],
    }));
    Object.values(filterCondition).forEach(filter => {
      if (filter.searchValues[0] === 'clear') {
        filter.searchValues = [null];
      }

    });
    this.filterConditionsInputSearch = {...this.filterConditionsInputSearch, ...filterCondition};
    await this.get()


  }

  async handleRestorePreviousFilter(event: any) {
    if (event) {
      if (event.typeFilterOptions == TypeFilterOptions.NumberInputSearch || event.typeFilterOptions == TypeFilterOptions.TextInputSearch) {
        this.filterConditionsInputSearch = {
          ...this.filterConditionsInputSearch,
          ...event.filterConditions,
        };
      }
    }


    this.selectedItemsFilterForPrint = [];
    this.excludedRows = []
    await this.get(undefined, 'back')
  }

  async handleRemoveAllFiltersAndSorts(config: TableScrollingConfigurations) {

    this.tableConfigurations.columns = config.columns;
    this.columns = config.columns;
    this.tableConfigurations.options = config.options;

    this.selectedItemsFilterForPrint = [];
    this.excludedRows = []

    this.requestsList = this.requestsList.slice(0, 1);
    this.filterConditionsInputSearch = {};
    await this.get(undefined, 'back')
  }

  handleTableConfigurationsChange(config: TableScrollingConfigurations) {

    this.tableConfigurations.columns = config.columns;
    this.columns = config.columns;
    this.tableConfigurations.options = config.options;
  }
}

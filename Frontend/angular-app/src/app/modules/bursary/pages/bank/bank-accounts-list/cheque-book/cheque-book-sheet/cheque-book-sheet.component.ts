import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../../core/abstraction/base.component";
import { ChequeBook } from "../../../../../entities/cheque-book";
import { ChequeBooksSheet } from "../../../../../entities/cheque-books-sheet";
import { BankAccounts } from "../../../../../entities/bank-accounts";
import { SearchQuery } from "../../../../../../../shared/services/search/models/search-query";
import {
    TableScrollingConfigurations
} from "../../../../../../../core/components/custom/table/models/table-scrolling-configurations";
import { ToolBar } from "../../../../../../../core/components/custom/table/models/tool-bar";
import { FontFamilies } from "../../../../../../../core/components/custom/table/models/font-families";
import { FontWeights } from "../../../../../../../core/components/custom/table/models/font-weights";
import {
    Column,
    FilterCondition,
    TypeFilterOptions
} from "../../../../../../../core/components/custom/table/models/column";
import { AccountReference } from "../../../../../../accounting/entities/account-reference";
import { Mediator } from "../../../../../../../core/services/mediator/mediator.service";
import { Router } from "@angular/router";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { PreDefinedActions } from "../../../../../../../core/components/custom/action-bar/action-bar.component";
import { TableColumnDataType } from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import { DecimalFormat } from "../../../../../../../core/components/custom/table/models/decimal-format";
import { TableColumnFilter } from "../../../../../../../core/components/custom/table/models/table-column-filter";
import {
    TableColumnFilterTypes
} from "../../../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../../../core/components/custom/table/models/table-options";
import {
    TablePaginationOptions
} from "../../../../../../../core/components/custom/table/models/table-pagination-options";
import { PrintOptions } from "../../../../../../../core/components/custom/table/models/print_options";
import {
    PayablesChequeBooksQuery
} from "../../../../../repositories/payables_cheque_books/queries/payables-cheque-books-query";
import {
    GetAccountReferencesQuery
} from "../../../../../../accounting/repositories/account-reference/queries/get-account-references-query";

import { PageModes } from "../../../../../../../core/enums/page-modes";
import { ChequeBookDialogComponent } from "../cheque-book-dialog/cheque-book-dialog.component";
import {
    CancelChequeBooksSheetsDialogComponent
} from "../cheque-books-sheets/cancel-cheque-books-sheets-dialog/cancel-cheque-books-sheets-dialog.component";
import {
    ConfirmDialogComponent,
    ConfirmDialogIcons
} from "../../../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";
import {
    UnCancelPayablesChequeBooksSheetCommand
} from "../../../../../repositories/payables_cheque_book-sheets/commands/un-cancel-payables-cheque-books-sheet-command";
import {
    AddSayyadNoChequeBooksSheetsDialogComponent
} from "../cheque-books-sheets/add-sayyad-no-cheque-books-sheets-dialog/add-sayyad-no-cheque-books-sheets-dialog.component";
import {
    PayablesChequeBooksSheetsQuery
} from "../../../../../repositories/payables_cheque_book-sheets/queries/payables-cheque-books-sheets-query";
import { GetBankAccounts } from "../../../../../repositories/bank-accounts/queries/get-bank-accounts";
import { TaxType } from "../../../../../../accounting/pages/reporting/ledger-report2/ledger-report2.component";
import { GetBankBranchesQuery } from 'src/app/modules/bursary/repositories/bank-branch/queries/get-bank-branches-query';
import { GetBanksQuery } from 'src/app/modules/bursary/repositories/bank/queries/get-banks-query';
import { GetBankAccountTypesQuery } from 'src/app/modules/bursary/repositories/bank-account/queries/get-bank-account-types-query';
import { Bank } from 'src/app/modules/logistics/entities/bank';

@Component({
    selector: 'app-cheque-book-sheet',
    templateUrl: './cheque-book-sheet.component.html',
    styleUrls: ['./cheque-book-sheet.component.scss']
})
export class ChequeBookSheetComponent extends BaseComponent {

    @ViewChild('rowBtn1', { read: TemplateRef }) rowBtn1!: TemplateRef<any>;
    @ViewChild('expandRowTable', { read: TemplateRef }) expandRowTable!: TemplateRef<any>;
    requestsList: string[] = [];
    requestsIndex: number = -1;
    excludedRows: any = [];
    chequeBooks: ChequeBook[] = [];
    chequeBook!: ChequeBook;
    chequeBookSheet: ChequeBooksSheet[] = [];
    bankAccount!: BankAccounts;
    filterConditionsInputSearch: { [key: string]: any } = {};
    selectedItemsFilterForPrint: any = [new SearchQuery({
        propertyName: 'id',
        values: [],
        comparison: 'in',
        nextOperand: 'and'
    })];
    tableConfigurations!: TableScrollingConfigurations;
    toolBar: ToolBar = {
        showTools: {
            tableSettings: true,
            includeOnlySelectedItemsLocal: false,
            excludeSelectedItemsLocal: false,
            includeOnlySelectedItemsFilter: false,
            excludeSelectedItemsFilter: false,
            undoLocal: false,
            deleteLocal: false,
            restorePreviousFilter: true,
            refresh: true,
            exportExcel: false,
            fullScreen: true,
            printFile: true,
            removeAllFiltersAndSorts: true,
            showAll: true
        },
        isLargeSize: false
    }
    defaultColumnSettings = {
        class: '',
        style: {},
        display: true,
        sortable: true,
        filter: undefined,
        displayFunction: undefined,
        disabled: false,
        options: [],
        optionsValueKey: 'id',
        optionsTitleKey: [],
        filterOptionsFunction: undefined,
        filteredOptions: [],
        asyncOptions: undefined,
        showSum: false,
        sumColumnValue: 0,
        matTooltipDisabled: true,
        fontSize: 14,
        fontFamily: FontFamilies.IranYekanBold,
        fontWeight: FontWeights.medium,
        isCurrencyField: false,
        isDisableDrop: false,
        typeFilterOptions: TypeFilterOptions.None,
        lineStyle: 'onlyShowFirstLine',


    };
    columns!: Column[];
    accountReferences: AccountReference[] = [];
    banks: Bank[] = [];
    chequeBookId !: number;
    bankAccountId !: number;
    typeListCancel: any[] = [
        {
            id: 1,
            label: 'بله',
            value: true
        },
        {
            id: 2,
            label: 'خیر',
            value: false
        },
        {
            id: 3,
            label: 'همه',
            value: null

        }
    ]
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

        const column = this.tableConfigurations.columns.find((col: any) => col.field === 'btn');
        if (column) {
            column.template = this.rowBtn1
        }
        const columnEexpand = this.tableConfigurations.columns.find((col: any) => col.field === 'payables_ChequeBooksSheets');

        if (columnEexpand) {
            columnEexpand.expandRowWithTemplate = this.expandRowTable;
        }
    }


    async resolve() {
        this.chequeBookId = this.getQueryParam('chequeBookId') as number;
        this.bankAccountId = this.getQueryParam('bankAccountId') as number;
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
                field: 'payables_ChequeBooksSheets',
                title: '',
                width: 1,

                type: TableColumnDataType.ExpandRowWithTemplate,

                display: true,
                lineStyle: 'onlyShowFirstLine',
                expandRowWithTemplate: this.expandRowTable

            },

            {
                ...this.defaultColumnSettings,
                index: 2,
                field: 'id',
                title: 'شناسه چک',
                width: 4.5,
                type: TableColumnDataType.Number,
                filter: new TableColumnFilter('id', TableColumnFilterTypes.Number),
                lineStyle: 'onlyShowFirstLine',
                typeFilterOptions: TypeFilterOptions.NumberInputSearch,
            }

            , {
                ...this.defaultColumnSettings,
                index: 3,
                field: 'chequeSheetNo',
                title: 'ش.چک',
                width: 3.5,
                type: TableColumnDataType.Text,
                filter: new TableColumnFilter('chequeSheetNo', TableColumnFilterTypes.Text),
                lineStyle: 'onlyShowFirstLine',

                typeFilterOptions: TypeFilterOptions.TextInputSearch,

            }

            , {
                ...this.defaultColumnSettings,
                index: 3,
                field: 'sayyadNo',
                title: 'ش.صیادی',
                width: 3.5,
                type: TableColumnDataType.Text,
                filter: new TableColumnFilter('sayyadNo', TableColumnFilterTypes.Text),
                lineStyle: 'onlyShowFirstLine',

                typeFilterOptions: TypeFilterOptions.TextInputSearch,

            }
            , {
                ...this.defaultColumnSettings,
                index: 3,
                field: 'statusName',
                title: 'وضعیت',
                width: 3.5,
                type: TableColumnDataType.Text,
                filter: new TableColumnFilter('statusName', TableColumnFilterTypes.Text),
                lineStyle: 'onlyShowFirstLine',

                typeFilterOptions: TypeFilterOptions.TextInputSearch,

            }
            , {
                ...this.defaultColumnSettings,
                index: 3,
                field: 'amount',
                title: 'مبلغ',
                width: 3.5,
                type: TableColumnDataType.Money,
                filter: new TableColumnFilter('amount', TableColumnFilterTypes.Money),
                lineStyle: 'onlyShowFirstLine',

                typeFilterOptions: TypeFilterOptions.NumberInputSearch,

            }
            , {
                ...this.defaultColumnSettings,
                index: 3,
                field: 'isCanceled',
                title: 'باطل شده',
                width: 5,
                type: TableColumnDataType.Text,
                filter: new TableColumnFilter('isCanceled', TableColumnFilterTypes.CheckBox, false, this.typeListCancel),
                lineStyle: 'default',
                typeFilterOptions: TypeFilterOptions.NgSelect,
                filteredOptions: this.typeListCancel,
                optionsValueKey: 'value',
                optionsSelectTitleKey: 'label',
                optionsTitleKey: ['label'],
                displayFn: (value: any) => {
                    return value.isCanceled ? 'بله' : 'خیر';
                },
                displayPrintFun: (value: any) => {
                    return value ? 'بله' : 'خیر';
                }

            }
            ,
            {
                ...this.defaultColumnSettings,
                index: 4,
                field: 'cancelDate',
                title: 'تاریخ ابطال',
                width: 4,
                type: TableColumnDataType.Date,
                filter: new TableColumnFilter('cancelDate', TableColumnFilterTypes.Date),
                lineStyle: 'onlyShowFirstLine',
                typeFilterOptions: TypeFilterOptions.Date,

            },



            {
                ...this.defaultColumnSettings,
                index: 7,
                field: 'cancelDescp',
                title: 'دلیل ابطال',
                width: 8,
                type: TableColumnDataType.Text,
                filter: new TableColumnFilter('cancelDescp', TableColumnFilterTypes.Text),
                lineStyle: 'onlyShowFirstLine',
                typeFilterOptions: TypeFilterOptions.TextInputSearch,
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
        if (this.bankAccountId) {

            await this.getBankAccountDetail();
        }
        if (this.chequeBookId) {
            await this.getChequeBookDetail();
        }
        await this.get();


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
        if (this.chequeBookId) {
            searchQueries.push(new SearchQuery({
                propertyName: 'chequeBookId',
                values: [this.chequeBookId],
                comparison: 'equal',
                nextOperand: 'and'
            }))
        }
        let orderByProperty = '';
        if (this.tableConfigurations.sortKeys) {
            this.tableConfigurations.sortKeys.forEach((key, index) => {
                orderByProperty += index ? `,${key}` : key
            })
        }
        let request = new PayablesChequeBooksSheetsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
        if (action == 'back') {

            this.requestsList.pop();
            this.requestsIndex--;
            if (this.requestsList.length > 1) {

                let temp = <PayablesChequeBooksSheetsQuery>JSON.parse(this.requestsList[this.requestsList.length - 1]);
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

            this.chequeBooks = response;
            response.objResult.totalCount && (this.tableConfigurations.pagination.totalItems = response.objResult.totalCount);
            this.tableConfigurations.options.isLoadingTable = false;

        }).catch(() => {
            this.tableConfigurations.options.isLoadingTable = false;
        });
    }


    createRequest(temp: PayablesChequeBooksSheetsQuery) {
        let request = new PayablesChequeBooksSheetsQuery();
        request.pageIndex = temp.pageIndex;
        request.pageSize = temp.pageSize;
        request.conditions = temp.conditions;
        request.orderByProperty = temp.orderByProperty;


        return request
    }

    resetRequest() {
        const temp = new PayablesChequeBooksSheetsQuery();
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

    }

    navigateToChequeBooks(event: any) {

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

        this.filterConditionsInputSearch = { ...this.filterConditionsInputSearch, ...filterConditions };
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
        this.filterConditionsInputSearch = { ...this.filterConditionsInputSearch, ...filterCondition };
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


    protected readonly fieldTypes = TableColumnDataType;

    cancelChequeSheet(chequeSheet: ChequeBooksSheet) {

        let dialogConfig = new MatDialogConfig();
        dialogConfig.data = {
            pageMode: PageModes.Update,
            chequeSheet: chequeSheet
        };
        dialogConfig.width = '30vw'
        let dialogReference = this.dialog.open(CancelChequeBooksSheetsDialogComponent, dialogConfig);
        dialogReference.afterClosed().subscribe(async (response) => {
            if (response) {
                await this.getChequeBooksSheet(response.chequeBookId)
            }
        })
    }
    unCancelChequeSheet(ChequeSheet: ChequeBooksSheet) {
        let chequeSheetId = ChequeSheet.id;


        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            data: {
                title: ' تغییر وضعیت چک ابطال شده ',
                message: 'آیا از رفع ابطال چک انتخاب شده مطمئن هستید ؟',
                icon: ConfirmDialogIcons.warning,
                actions: {
                    confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
                }
            }
        });


        dialogRef.afterClosed().subscribe(async result => {
            if (result == true) {

                let request = new UnCancelPayablesChequeBooksSheetCommand();

                request.id = chequeSheetId;
                this._mediator.send(request)
                    .then((response) => {
                        this.getChequeBooksSheet(response.chequeBookId)
                    }).catch(() => {

                    });
            }
        });
    }

    updateChequeSheet(chequeSheet: ChequeBooksSheet) {
        let dialogConfig = new MatDialogConfig();
        dialogConfig.data = {
            pageMode: PageModes.Update,
            chequeSheet: chequeSheet
        };
        dialogConfig.width = '30vw'
        let dialogReference = this.dialog.open(AddSayyadNoChequeBooksSheetsDialogComponent, dialogConfig);

        dialogReference.afterClosed().subscribe(async (response) => {
            if (response) {
                await this.getChequeBooksSheet(response.chequeBookId)
            }
        })
    }

    async handleExpandedRowIndex(row: ChequeBook) {
        let id = row.id;
        // if (id)
        //   await this.getChequeBooksSheet(id)
    }

    async getChequeBooksSheet(id: number) {
        let searchQueries: SearchQuery[] = [
            new SearchQuery({
                propertyName: 'chequeBookId',
                values: [id],
                comparison: 'equal',
                nextOperand: 'and'
            })
        ]
        let request = new PayablesChequeBooksSheetsQuery(0, 0, searchQueries)

        await this._mediator.send(request).then((response: any) => {

            this.chequeBookSheet = response;



        }).catch(() => {

        });
    }
    private async getBankAccountDetail() {
        let searchQueries: SearchQuery[] = [
            new SearchQuery({
                propertyName: 'id',
                values: [this.bankAccountId],
                comparison: 'equal',
                nextOperand: 'and'
            })
        ]
        let request = new GetBankAccounts(0, 0, searchQueries)

        await this._mediator.send(request).then((response: any) => {

            this.bankAccount = response.objResult.data[0];



        }).catch(() => {

        });
    }
    private async getChequeBookDetail() {
        let searchQueries: SearchQuery[] = [
            new SearchQuery({
                propertyName: 'id',
                values: [this.chequeBookId],
                comparison: 'equal',
                nextOperand: 'and'
            })
        ]
        let request = new PayablesChequeBooksQuery(0, 0, searchQueries)

        await this._mediator.send(request).then((response: any) => {

            this.chequeBook = response[0];

        }).catch(() => {

        });
    }

}

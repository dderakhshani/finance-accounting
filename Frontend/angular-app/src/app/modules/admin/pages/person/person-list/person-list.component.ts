import {Component} from '@angular/core';
import {Person} from "../../../entities/person";
import {Router} from "@angular/router";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetPersonsQuery} from "../../../repositories/person/queries/get-persons-query";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {
  Action,
  ActionTypes,
  PreDefinedActions
} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {ToPersianDatePipe} from "../../../../../core/pipes/to-persian-date.pipe";
import {
  TableScrollingConfigurations
} from "../../../../../core/components/custom/table/models/table-scrolling-configurations";
import {ToolBar} from "../../../../../core/components/custom/table/models/tool-bar";
import {FontFamilies} from "../../../../../core/components/custom/table/models/font-families";
import {FontWeights} from "../../../../../core/components/custom/table/models/font-weights";
import {Column, TypeFilterOptions} from "../../../../../core/components/custom/table/models/column";
import {DecimalFormat} from "../../../../../core/components/custom/table/models/decimal-format";
import {TablePaginationOptions} from "../../../../../core/components/custom/table/models/table-pagination-options";
import {GetLedgerReportQuery} from "../../../../accounting/repositories/reporting/queries/get-ledger-report-query";
import {NotificationService} from 'src/app/shared/services/notification/notification.service';
import {MatDialog} from '@angular/material/dialog';
import {
  ConfirmDialogComponent,
  ConfirmDialogIcons
} from 'src/app/core/components/material-design/confirm-dialog/confirm-dialog.component';
import {UpdateDepositPersonCommand} from '../../../repositories/person/commands/update-deposit-person-command';
import {CentralBankReportDialogComponent} from "./central-bank-report-dialog/central-bank-report-dialog.component";
import {PrintOptions} from "../../../../../core/components/custom/table/models/print_options";
import {IdentityService} from "../../../../identity/repositories/identity.service";
import {BaseTable} from "../../../../../core/abstraction/base-table";

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss']
})
export class PersonListComponent extends BaseTable {
  selectedItemsFilterForPrint: any = [new SearchQuery({
    propertyName: 'id',
    values: [],
    comparison: 'in',
    nextOperand: 'and'
  })];
  people: Person[] = [];
  selectPerson: Person[] = [];

  constructor(
    private _mediator: Mediator,
    private router: Router, public dialog: MatDialog, private notificationService: NotificationService,
    public identityService: IdentityService
  ) {
    super();

  }

  ngAfterViewInit() {

    this.actionBar.actions = [
      PreDefinedActions.add().setTitle('افزودن شخص').setShow(this.identityService.doesHavePermission("AddPerson")),
      new Action('گزارش بانک مرکزی', 'assured_workload', ActionTypes.custom, 'centralBankReport').setShow(this.identityService.doesHavePermission("PersonCentralBankReport"))
    ];
  }

  async ngOnInit() {
    this.requestsIndex = -1;
    await this.resolve()
  }

  async resolve() {
    // this.isLoading = true;
    let columns: Column[] = [
      {
        ...this.defaultColumnSettings,
        index: 0,
        field: 'selected',
        title: '#',
        width: 2,
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
        width: 2,
        type: TableColumnDataType.Index,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',


      },


      {
        ...this.defaultColumnSettings,
        index: 2,
        field: 'nationalNumber',
        title: 'کد ملی',
        width: 4,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('nationalNumber', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },

      {
        ...this.defaultColumnSettings,
        index: 3,
        field: 'economicCode',
        title: 'کد اقتصادی',
        width: 4,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('economicCode', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 4,
        field: 'accountReferenceCode',
        title: 'کد حسابداری',
        width: 4,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('accountReferenceCode', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,

      },
      {
        ...this.defaultColumnSettings,
        index: 5,
        field: 'employeeCode',
        title: 'کد پرسنلی',
        width: 4,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('employeeCode', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },

      {
        ...this.defaultColumnSettings,
        index: 6,
        field: 'fullName',
        title: 'نام',
        width: 5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('fullName', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,

      },
      {
        ...this.defaultColumnSettings,
        index: 7,
        field: 'depositId',
        title: 'شناسه پرداخت',
        width: 4.5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('depositId', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,

      },
      {
        ...this.defaultColumnSettings,
        index: 8,
        field: 'legalBaseTitle',
        title: 'نوع شخص',
        width: 4.5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('legalBaseTitle', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        display: false,
        print: false,

      },
      {
        ...this.defaultColumnSettings,
        index: 9,
        field: 'governmentalBaseTitle',
        title: 'ماهیت شخص',
        width: 4.5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('governmentalBaseTitle', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        display: false,
        print: false,

      },
      {
        ...this.defaultColumnSettings,
        index: 10,
        field: 'accountReferenceGroupTitle',
        title: 'گروه تفصیل',
        width: 5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('accountReferenceGroupTitle', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 11,
        field: 'phoneNumber',
        title: 'تلفن',
        width: 6,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('phoneNumber', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,

        // displayFn: (person: Person) => {
        //   return person.phoneNumber ? person.phoneNumber : 'شماره ای ثبت نشده';
        // }
      },
      {
        ...this.defaultColumnSettings,
        index: 12,
        field: 'address',
        title: 'آدرس',
        width: 7,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('address', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,

        // displayFn: (person: Person) => {
        //   return person.address ? person.address : 'آدرسی ثبت نشده';
        // }
      },
      {
        ...this.defaultColumnSettings,
        index: 13,
        field: 'postalCode',
        title: 'کد پستی',
        width: 4,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('postalCode', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        display: false,
        print: false,
        // displayFn: (person: Person) => {
        //   return person.postalCode ? person.postalCode : 'کد پستی ثبت نشده';
        // }

      },
      {
        ...this.defaultColumnSettings,
        index: 14,
        field: 'createdBy',
        title: 'ثبت کننده',
        width: 6,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('createdBy', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        // displayFn: (person: Person) => {
        //   return person.createdBy + " - " + new ToPersianDatePipe().transform(person.createdAt);
        // }
      }, {
        ...this.defaultColumnSettings,
        index: 15,
        field: 'createdAt',
        title: 'تاریخ ثبت',
        width: 6,
        type: TableColumnDataType.Date,
        filter: new TableColumnFilter('createdAt', TableColumnFilterTypes.Date),
        lineStyle: 'default',
        display: false,
        print: false,
        typeFilterOptions: TypeFilterOptions.Date,
      },
      {
        ...this.defaultColumnSettings,
        index: 16,
        field: 'modifiedBy',
        title: 'آخرین ویرایش کننده',
        width: 6,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('modifiedBy', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        // displayFn: (person: Person) => {
        //   return person.modifiedBy.trim() ? person.modifiedBy + " - " + new ToPersianDatePipe().transform(person.modifiedAt) : '';
        // }
      }, {
        ...this.defaultColumnSettings,
        index: 17,
        field: 'modifiedAt',
        title: 'تاریخ ویرایش',
        width: 6,
        type: TableColumnDataType.Date,
        filter: new TableColumnFilter('modifiedAt', TableColumnFilterTypes.Date),
        lineStyle: 'default',
        display: false,
        print: false,
        typeFilterOptions: TypeFilterOptions.Date,
      },

    ];
    this.toolBar ={
      showTools: {
        tableSettings: true,
        includeOnlySelectedItemsLocal: true,
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
    };
    this.tableConfigurations = new TableScrollingConfigurations(columns, new TableOptions(false, true), new TablePaginationOptions(), this.toolBar, new PrintOptions('گزارش فهرست اشخاص'))
    this.tableConfigurations.options.usePagination = true;
    this.tableConfigurations.options.showFilterRow = true;
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.options.exportOptions.showExportButton = true;
    this.tableConfigurations.pagination.pageSize = 500;


    await this.get();
    // this.isLoading = false;
  }

  initialize() {
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
    let request = new GetPersonsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    if (action == 'back') {

      this.requestsList.pop();
      this.requestsIndex--;
      if (this.requestsList.length > 1) {

        let temp = <GetPersonsQuery>JSON.parse(this.requestsList[this.requestsList.length - 1]);
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
    await this._mediator.send(request).then(response => {
      this.people = response.data;
      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
      this.tableConfigurations.options.isLoadingTable = false;

    }).catch(() => {
      this.tableConfigurations.options.isLoadingTable = false;
    });

  }

  createRequest(temp: GetPersonsQuery) {
    let request = new GetPersonsQuery();
    request.pageIndex = temp.pageIndex;
    request.pageSize = temp.pageSize;
    request.conditions = temp.conditions;
    request.orderByProperty = temp.orderByProperty;


    return request
  }

  resetRequest() {
    const temp = new GetPersonsQuery();
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

  async update() {
    // @ts-ignore
    let person = this.selectPerson.filter(x => x.selected)[0]
    if (person) {
      await this.navigateToPerson(person)
    }
  }

  async add() {
    await this.router.navigateByUrl(`admin/person/add`)
  }

  async navigateToPerson(person: Person) {
    if (this.identityService.doesHavePermission("EditPerson")) {
      await this.router.navigateByUrl(`admin/person/add?id=${person.id}`)
    }
  }

  close(): any {
  }

  delete(): any {
  }


  async handleOptionSelected(event: { typeFilterOptions: any, query: any }) {
    this.tableConfigurations.pagination.pageIndex = 0;
    if (event.typeFilterOptions == TypeFilterOptions.NgSelect) {
      await this.updateNgSelectFilters(event.query).then()
    }
    if (event.typeFilterOptions == TypeFilterOptions.NumberInputSearch || event.typeFilterOptions == TypeFilterOptions.TextInputSearch) {
      await this.updateInputSearchFilters(event.query)
    }
    if (event.typeFilterOptions == TypeFilterOptions.Date) {
      await this.updateInputSearchFilters(event.query)
    }
  }

  async updateInputSearchFilters(filterConditions: { [key: string]: any }) {
    this.filterConditionsInputSearch = {...this.filterConditionsInputSearch, ...filterConditions};
    await this.get()

  }

  async updateNgSelectFilters(filterConditions: { [key: string]: any }) {
    this.request =  <GetLedgerReportQuery>this.request;

    await this.get()
  }


  handleAdvancedFilter($event: boolean) {

  }

  async handleRestorePreviousFilter(event: any) {

    if (event) {

      if (event.typeFilterOptions == TypeFilterOptions.NumberInputSearch || event.typeFilterOptions == TypeFilterOptions.TextInputSearch) {
        this.filterConditionsInputSearch = {...this.filterConditionsInputSearch, ...event.filterConditions};
      }

      this.selectedItemsFilterForPrint = [];
      this.excludedRows = []
    }
    await this.get(undefined, 'back')
  }

  async handleRemoveAllFiltersAndSorts(config: TableScrollingConfigurations) {

    this.tableConfigurations.columns = config.columns;
    this.tableConfigurations.options = config.options;
    this.selectedItemsFilterForPrint = [];
    this.excludedRows = []
    this.requestsList = this.requestsList.slice(0, 1);
    this.filterConditionsInputSearch = {};
    await this.get(undefined, 'back')

  }

  handleTableConfigurationsChange(config: TableScrollingConfigurations) {

    this.tableConfigurations.columns = config.columns;
    this.tableConfigurations.options = config.options;
  }

  async handleExcludeSelectedItemsEvent(ids: number[]) {
    this.excludedRows = [new SearchQuery({
      propertyName: 'id',
      values: ids,
      comparison: 'notIn',
      nextOperand: 'and'
    })];
    await this.get();
  }


  async handleSelectedItemsFilterForPrint(ids: number[]) {
    this.selectedItemsFilterForPrint = [new SearchQuery({
      propertyName: 'id',
      values: ids,
      comparison: 'in',
      nextOperand: 'and'
    })];


  }

  async setDepositIdForCustomerByReferenceCode() {

    let customers = this.selectPerson;

    if (customers == null || customers.length == 0)
      return this.notificationService.showFailureMessage("هیچ شخصی برای ایجاد شناسه انتخاب نشده است", 0);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف',
        message: 'آیا از ایجاد شناسه برای این شخص مطمئن هستید ؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: {title: 'بله', show: true}, cancel: {title: 'خیر', show: true}
        }
      }
    });


    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        let setDeposits = new UpdateDepositPersonCommand();
        let numbers = customers.filter((x: any) => x.selected).map(item => item.accountReferenceCode);
        setDeposits.accountReferences = ((numbers as any))
        this.isLoading = true;
        await this._mediator.send(<UpdateDepositPersonCommand>setDeposits);
        await this.get()
        this.isLoading = false;
      }
    });
  }


  handleRowsSelected(selectPerson: any) {
    if (!this.selectPerson) {
      this.selectPerson = [];
      return
    }
    this.selectPerson = selectPerson;
  }


  async handleCustomClick(action: Action) {
    if (action.uniqueName === 'centralBankReport') this.dialog.open(CentralBankReportDialogComponent)
  }
}

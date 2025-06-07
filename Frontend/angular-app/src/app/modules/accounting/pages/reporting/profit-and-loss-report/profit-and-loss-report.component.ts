import {Component, OnInit} from '@angular/core';
import {BaseComponent} from 'src/app/core/abstraction/base.component';
import {ProfitAndLossReportQuery} from '../../../repositories/reporting/queries/profit-and-loss-report-query';
import {IdentityService} from 'src/app/modules/identity/repositories/identity.service';
import {UserYear} from 'src/app/modules/identity/repositories/models/user-year';
import {FormControl, FormGroup} from '@angular/forms';
import {PagesCommonService} from 'src/app/shared/services/pages/pages-common.service';
import {ToPersianDatePipe} from 'src/app/core/pipes/to-persian-date.pipe';
import {environment} from "../../../../../../environments/environment";
import {GetAccountReviewReportQuery} from "../../../repositories/reporting/queries/get-account-review-report-query";
import {ProfitAndLossRiportModel} from '../../../repositories/profit-and-loss/profit-and-loss-report-model';
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {GetLedgerReportQuery} from "../../../repositories/reporting/queries/get-ledger-report-query";
import {
  TableScrollingConfigurations
} from "../../../../../core/components/custom/table/models/table-scrolling-configurations";
import {ToolBar} from "../../../../../core/components/custom/table/models/tool-bar";
import {FontFamilies} from "../../../../../core/components/custom/table/models/font-families";
import {FontWeights} from "../../../../../core/components/custom/table/models/font-weights";
import {Column, TypeFilterOptions} from "../../../../../core/components/custom/table/models/column";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {DecimalFormat} from "../../../../../core/components/custom/table/models/decimal-format";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TablePaginationOptions} from "../../../../../core/components/custom/table/models/table-pagination-options";
import {PrintOptions} from "../../../../../core/components/custom/table/models/print_options";
import {BaseTable} from "../../../../../core/abstraction/base-table";

@Component({
  selector: 'app-profit-and-loss-report',
  templateUrl: './profit-and-loss-report.component.html',
  styleUrls: ['./profit-and-loss-report.component.scss']
})
export class ProfitAndLossReportComponent extends BaseTable {

  result: ProfitAndLossRiportModel[] = [];
  tableConfigurations!: TableScrollingConfigurations;
  requestsList: string[] = [];
  requestsIndex: number = -1;
  dateNow = new ToPersianDatePipe().transform(new Date());
  SearchForm = new FormGroup({

    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });
  toolBar: ToolBar = {
    showTools: {
      tableSettings: true,
      includeOnlySelectedItemsLocal: false,
      excludeSelectedItemsLocal: false,
      includeOnlySelectedItemsFilter: false,
      excludeSelectedItemsFilter: false,
      undoLocal: false,
      deleteLocal: false,
      restorePreviousFilter: false,
      refresh: true,
      exportExcel: false,
      fullScreen: true,
      printFile: true,
      removeAllFiltersAndSorts: false,
      showAll: true
    },
    isLargeSize: false
  }
  defaultColumnSettings = {
    class: '',
    style: {},
    display: true,
    sortable: false,
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
    fontSize: 16,
    fontFamily: FontFamilies.IranYekanBold,
    fontWeight: FontWeights.medium,
    isCurrencyField: false,
    isDisableDrop: false,
    typeFilterOptions: TypeFilterOptions.None,
    lineStyle: 'onlyShowFirstLine',
    print: true

  };
  columns!: Column[];

  constructor(private Mediator: Mediator, public identityService: IdentityService,
              public Service: PagesCommonService) {
    super();
  }

  async ngOnInit() {
    this.resolve()
    await this.initialize();
    {
      let query = <ProfitAndLossReportQuery>this.request;
      let fromDate = this.getQueryParam('fromDate');
      if (fromDate) {

        fromDate = decodeURIComponent(fromDate);
        fromDate = decodeURIComponent(fromDate);
        query.voucherDateFrom = new Date(fromDate);
      }
      let toDate = this.getQueryParam('toDate');
      if (toDate) {
        toDate = decodeURIComponent(toDate);
        toDate = decodeURIComponent(toDate);
        query.voucherDateTo = new Date(toDate);
      }

    }
  }


  resolve(params?: any) {
    this.columns = [
      // {
      //   ...this.defaultColumnSettings,
      //   index: 0,
      //   field: 'selected',
      //   title: '#',
      //   width: 2,
      //   type: TableColumnDataType.Select,
      //   isDisableDrop: true,
      //   lineStyle: 'onlyShowFirstLine',
      //   digitsInfo: DecimalFormat.Default,
      //
      //
      // },

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
        field: 'code',
        title: 'کد حساب',
        width: 2,
        type: TableColumnDataType.Text,
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 2,
        field: 'name',
        title: 'شرح حساب',
        width: 6.5,
        type: TableColumnDataType.Text,

        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 2,
        field: 'price',
        title: 'مبلغ (ريال)',
        width: 6.5,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.Default,

        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
      },


    ]
  }

  async initialize(params?: any) {
    let voucherDateFrom = this.identityService.getActiveYearStartDate();
    let voucherDateTo = this.identityService.getActiveYearEndDate();
    let companyId = +this.identityService.applicationUser.companyId;
    let yearid: number[] = [this.identityService.applicationUser.yearId] ?? ['4'];
    let query = new ProfitAndLossReportQuery(companyId, yearid, voucherDateFrom, voucherDateTo);

    this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(false, true), new TablePaginationOptions(), this.toolBar, new PrintOptions('گزارش سود و زیان'))

    this.tableConfigurations.options.showSumRow = false;
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.options.itemSize = 32;
    this.tableConfigurations.options.usePagination = false;
    this.tableConfigurations.options.exportOptions.showExportButton = false;

    this.request = query;

    await this.get();


  }

  add(param?: any) {
    throw new Error('Method not implemented.');
  }

  async get(param?: any) {
    // this.result = <ProfitAndLossRiportModel[]>
    this.tableConfigurations.options.isLoadingTable = false;
    await this.Mediator.send(this.request).then((response) => {
      this.result = <ProfitAndLossRiportModel[]>response;

      this.result = this.result.map((item: any, index) => {
        const style = (
          item.name === 'سود ناخالص' ||
          item.name === 'سود و زیان عملیاتی' ||
          item.name === 'سود ناخالص قبل از مالیات' ||
          item.name === 'سود خالص'

        ) ? {'background-color': '#EDD6EFFF'} : (item.name === 'جمع درآمد عملیاتی' ||
          item.name === 'جمع هزینه های عملیاتی') ? {'background-color': '#C6DEFF'} : {};

        return {
          ...item,
          style,
          id: index + 1
        };
      });

      response.objResult.totalCount && (this.tableConfigurations.pagination.totalItems = response.objResult.totalCount);

      this.tableConfigurations.options.isLoadingTable = false;

    }).catch(() => {
      this.tableConfigurations.options.isLoadingTable = false;
    });
    // @ts-ignore
    this.tableConfigurations.printOptions.dateFrom = this.request.voucherDateFrom;
    // @ts-ignore
    this.tableConfigurations.printOptions.dateTo = this.request.voucherDateTo;


  }

  addHours = (date: Date, hours: number): Date => {
    const result = new Date(date);
    result.setHours(result.getHours() + hours, 0, -1);
    return result;
  };


  async print() {

    let query = <ProfitAndLossReportQuery>this.request;

    let token = this.identityService.getToken();
    //@ts-ignore
    window.open(`${environment.apiURL}/accountingreports/ProfitAndLoss/index?access_token=${token}&fromDate=${query.voucherDateFrom.toISOString()}&toDate=${query.voucherDateTo.toISOString()}`, "_blank");

    //
    // let printContents = document.getElementById("printElement")?.innerHTML;
    // this.Service.onPrint(printContents?.toString(), '');
  }


  update(param?: any) {
    throw new Error('Method not implemented.');
  }

  delete(param?: any) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }


}

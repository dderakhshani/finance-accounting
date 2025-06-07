import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {ExtensionsService} from "../../../../../../shared/services/extensions/extensions.service";
import {Router} from "@angular/router";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {TableConfigurations} from "../../../../../../core/components/custom/table/models/table-configurations";
import {MoadianInvoiceHeader} from "../../../../entities/moadian-invoice-header";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {
  GetMoadianInvoiceHeadersQuery
} from "../../../../repositories/moadian/moadian-invoice-header/queries/get-moadian-invoice-headers-query";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {MoadianExcelImportDialogComponent} from "./moadian-excel-import-dialog/moadian-excel-import-dialog.component";
import {
  SubmitMoadianInvoicesByIdsCommand
} from "../../../../repositories/moadian/moadian-invoice-header/commands/submit-moadian-invoices-by-Ids-command";
import {
  InquiryMoadianInvoicesByIdsCommand
} from "../../../../repositories/moadian/moadian-invoice-header/commands/inquiry-moadian-invoices-by-ids-command";
import {ToPersianDatePipe} from "../../../../../../core/pipes/to-persian-date.pipe";
import {FormControl} from "@angular/forms";
import {
  UpdateMoadianInvoicesStatusByIdsDialogComponent
} from "./update-moadian-invoices-status-by-ids-dialog/update-moadian-invoices-status-by-ids-dialog.component";
import {
  MoadianVerificationCodeDialogComponent
} from "./moadian-verification-code-dialog/moadian-verification-code-dialog.component";

@Component({
  selector: 'app-moadian-invoices-list',
  templateUrl: './moadian-invoices-list.component.html',
  styleUrls: ['./moadian-invoices-list.component.scss']
})
export class MoadianInvoicesListComponent extends BaseComponent {
  isProduction = new FormControl(false);

  systemModes = [
    {
      title: 'سامانه اصلی',
      value: true
    },
    {
      title: 'سامانه SANDBOX',
      value: false
    },
  ]

  entities: MoadianInvoiceHeader[] = [];
  tableConfigurations!: TableConfigurations;

  constructor(
    private _mediator: Mediator,
    public extensionsService: ExtensionsService,
    private router: Router,
    public dialog: MatDialog,
    private datePipe: ToPersianDatePipe
  ) {
    super();
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.edit(),
      PreDefinedActions.add(),
      PreDefinedActions.refresh(),
    ]
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    let tableColumns = [
      new TableColumn('selected', '', TableColumnDataType.Select, ''),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, ''),
      new TableColumn(
        'invoiceNumber',
        'شماره',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('invoiceNumber', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'inno',
        'سریال',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('inno', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'indatim',
        'تاریخ',
        TableColumnDataType.Date,
        '2.5%',
        true,
        new TableColumnFilter('invoiceDate', TableColumnFilterTypes.Date),
        (value: MoadianInvoiceHeader) => {
          return this.datePipe.transform(new Date(value.indatim))
        }
      ),
      new TableColumn(
        'taxId',
        'شماره ثبت مالیاتی',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('taxId', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'irtaxid',
        'صورتحساب مرجع',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('irtaxid', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'insTitle',
        'موضوع',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('insTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'listNumber',
        'شماره لیست',
        TableColumnDataType.Number,
        '2.5%',
        true,
        new TableColumnFilter('listNumber', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'statusTitle',
        'وضعیت',
        TableColumnDataType.Text,
        '7.5%',
        true,
        new TableColumnFilter('statusTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'tbill',
        'جمع مبلغ',
        TableColumnDataType.Money,
        '2.5%',
        true,
        new TableColumnFilter('tbill', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'tobTitle',
        'نوع شخص',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('tobTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'personFullName',
        'خریدار',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('personFullName', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'bid',
        'شناسه ملی',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('bid', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'tinb',
        'شماره اقتصادی',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('tinb', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'cdcn',
        'شماره کوتاژ',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('cdcn', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'scc',
        'کد گمرک',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('scc', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'tonw',
        'جمع وزن خالص',
        TableColumnDataType.Number,
        '2.5%',
        true,
        new TableColumnFilter('tonw', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'tocv',
        'مجموع ارزش افزوده',
        TableColumnDataType.Money,
        '2.5%',
        true,
        new TableColumnFilter('tonw', TableColumnFilterTypes.Money)
      ),
      // new TableColumn(
      //   'bpc',
      //   'کد پستی',
      //   TableColumnDataType.Text,
      //   '2.5%',
      //   true,
      //   new TableColumnFilter('bpc', TableColumnFilterTypes.Text)
      // ),

      // new TableColumn(
      //   'submissionDate',
      //   'تاریخ ثبت در سامانه مودیان',
      //   TableColumnDataType.Date,
      //   '2.5%',
      //   true,
      //   new TableColumnFilter('submissionDate', TableColumnFilterTypes.Date)
      // ),

      new TableColumn(
        'intyTitle',
        'نوع',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('intyTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'inpTitle',
        'الگو',
        TableColumnDataType.Text,
        '7.5%',
        true,
        new TableColumnFilter('inpTitle', TableColumnFilterTypes.Text)
      ),


      new TableColumn(
        'creator',
        'ایجاد کننده',
        TableColumnDataType.Text,
        '7.5%',
        true,
      ),
    ];
    var options = new TableOptions(false, true)
    options.defaultSortKey = 'inno'
    options.defaultSortDirection = 'DESC'
    this.tableConfigurations = new TableConfigurations(tableColumns, options);

    this.tableConfigurations.options.showSumRow = true;

    await this.get();
  }

  async get(id?: number) {
    this.isLoading = true;
    this.entities = [];
    let searchQueries: SearchQuery[] = []
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

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetMoadianInvoiceHeadersQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    request.isProduction = this.isProduction.value;
    await this._mediator.send(request).then((response) => {
      this.isLoading = false;
      this.entities = response.data;
      response.totalCount || (!response.totalCount && response.data?.length === 0) ? this.tableConfigurations.pagination.totalItems = response.totalCount : '';
    }).catch(() => {
      this.isLoading = false;
    });
  }

  async add() {
    await this.router.navigateByUrl('/accounting/specialOperations/addMoadianInvoice')
  }

  async update() {
    // @ts-ignore
    let entity = this.voucherHeads.filter(x => x.selected)[0];
    await this.router.navigateByUrl(`/accounting/specialOperations/addMoadianInvoice?id=${entity.id}`);
  }

  async navigateToMoadianInvoiceHeader(moadianInvoiceHeader: MoadianInvoiceHeader) {
    await this.router.navigateByUrl(`/accounting/specialOperations/addMoadianInvoice?id=${moadianInvoiceHeader.id}`)
  }

  openImportByExcelDialog() {
    let dialogConfig = new MatDialogConfig()
    dialogConfig.data = {
      isProduction: this.isProduction.value
    }
    let dialogReference = this.dialog.open(MoadianExcelImportDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(async (isSuccess: boolean) => {
      if (isSuccess) await this.get()
    })
  }

  openUpdateStatusDialog() {
    let dialogConfig = new MatDialogConfig()
    dialogConfig.data = {
      // @ts-ignore
      selectedIds: this.entities.filter(x => x.selected === true).map(x => x.id)
    }
    let dialogReference = this.dialog.open(UpdateMoadianInvoicesStatusByIdsDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(async (isSuccess: boolean) => {
      if (isSuccess) await this.get()
    })
  }

  async submitSelectedInvoicesToMoadian() {
    this.isLoading = true;
    // @ts-ignore
    let invoiceIds = this.entities.filter(x => x.selected === true).map(x => x.id)
    let request = new SubmitMoadianInvoicesByIdsCommand(invoiceIds, this.isProduction.value, this.isProduction.value);
    await this._mediator.send(request).then(async (res) => {
      this.isLoading = false;
      if (this.isProduction.value === true) {
        let dialogConfig = new MatDialogConfig()
        dialogConfig.data = request;
        let dialogReference = this.dialog.open(MoadianVerificationCodeDialogComponent, dialogConfig);

        dialogReference.afterClosed().subscribe(async (isSuccess: boolean) => {
          if (isSuccess) await this.get()
        })
      } else {
        await this.get()
      }
    }).catch(() => {
      this.isLoading = false;
    });
  }

  async inquirySelectedInvoicesToMoadian() {
    this.isLoading = true;
    // @ts-ignore
    let invoiceIds = this.entities.filter(x => x.selected === true).map(x => x.id)

    await this._mediator.send(new InquiryMoadianInvoicesByIdsCommand(invoiceIds, this.isProduction.value)).then(async (res) => {
      await this.get()
    }).catch(() => {
      this.isLoading = false;
    });
  }

  initialize(params?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }
}

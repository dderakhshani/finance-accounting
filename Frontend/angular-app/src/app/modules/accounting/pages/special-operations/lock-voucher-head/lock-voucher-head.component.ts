import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {CodeVoucherGroup} from "../../../entities/code-voucher-group";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";


import {TableConfigurations} from "../../../../../core/components/custom/table/models/table-configurations";
import {VoucherHead} from "../../../entities/voucher-head";

import {Router} from "@angular/router";
import {
  Action,
  ActionTypes,
  PreDefinedActions
} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {
  SearchComparisonTypes,
  SearchOperandTypes,
  SearchQuery
} from "../../../../../shared/services/search/models/search-query";
import {GetVoucherHeadsQuery} from "../../../repositories/voucher-head/queries/get-voucher-heads-query";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {forkJoin} from "rxjs";
import {
  GetCodeVoucherGroupsQuery
} from "../../../repositories/code-voucher-group/queries/get-code-voucher-groups-query";
import {FormControl, FormGroup} from "@angular/forms";
import {IdentityService} from "../../../../identity/repositories/identity.service";
import {
  BulkVoucherStatusUpdateCommand
} from "../../../repositories/voucher-head/commands/bulk-voucher-status-update-command";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {
  CombineVoucherHeadsDialogComponent
} from "../combine-voucher-heads-dialog/combine-voucher-heads-dialog.component";
import {
  ConfirmDialogComponent,
  ConfirmDialogConfig
} from "../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";


@Component({
  selector: 'app-lock-voucher-head',
  templateUrl: './lock-voucher-head.component.html',
  styleUrls: ['./lock-voucher-head.component.scss']
})
export class LockVoucherHeadComponent extends BaseComponent {
  codeVoucherGroups: CodeVoucherGroup[] = [];
  voucherStates: any[] = [];

  voucherHeads: VoucherHead[] = [];

  tableConfigurations!: TableConfigurations;

  filtersForm!: FormGroup;

  filterFormLastValue!: any;
  filteredVoucherHeadsTotalCount = 0;


  constructor(
    private _mediator: Mediator,
    private router: Router,
    private identityService: IdentityService,
    private notificationService: NotificationService,
    private matDialog: MatDialog
  ) {
    super();
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      new Action(
        'قفل کردن اسناد',
        'lock',
        ActionTypes.custom,
        'lock',
        'none',
        undefined,
        undefined,
        'bg-red-700',
      ),
      new Action(
        'مرور  اسناد',
        'preview',
        ActionTypes.custom,
        'review',
        'none',
        undefined,
        undefined,
        'bg-amber-700',
      )
      ,
      new Action(
        'باز کردن اسناد',
        'lock_open',
        ActionTypes.custom,
        'unlock',
        'none',
        undefined,
        undefined,
        'bg-green-700',
      ),
      new Action(
        'ترکیب اسناد',
        '',
        ActionTypes.custom,
        'combine',
        'primary',
        undefined,
        undefined,
        '',
      ),

    ]
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {

    this.voucherStates = [
      {
        title: 'همه',
        id: 0
      },
      {
        title: 'موقت',
        id: 1
      },
      {
        title: 'مرور',
        id: 2
      },
      {
        title: 'دائم',
        id: 3
      },
    ];
    let tableColumns = [
      new TableColumn('selected', '', TableColumnDataType.Select, ''),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, ''),
      new TableColumn(
        'voucherNo',
        'شماره سند',
        TableColumnDataType.Number,
        '',
        true,
        undefined, //new TableColumnFilter('voucherNo', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'voucherDate',
        'تاریخ',
        TableColumnDataType.Date,
        '',
        true,
        undefined, //new TableColumnFilter('voucherDate', TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'totalDebit',
        'بدهکار',
        TableColumnDataType.Money,
        '',
        true,
        undefined, //new TableColumnFilter('totalDebit', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'totalCredit',
        'بستانکار',
        TableColumnDataType.Money,
        '',
        true,
        undefined, //new TableColumnFilter('totalCredit', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'codeVoucherGroupTitle',
        'نوع سند',
        TableColumnDataType.Text,
        '',
        true,
        undefined, //new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'voucherStateName',
        'وضعیت سند',
        TableColumnDataType.Text,
        '',
        true,
        undefined, //new TableColumnFilter('voucherStateName', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'voucherDescription',
        'شرح سند',
        TableColumnDataType.Text,
        '',
        true,
        undefined, //new TableColumnFilter('voucherDescription', TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        'traceNo',
        'شماره پیگیری',
        TableColumnDataType.Text,
        '',
        true,
        undefined, //new TableColumnFilter('traceNo', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'creator',
        'صادر کننده',
        TableColumnDataType.Text,
        '',
        true,
        undefined, //new TableColumnFilter('creator', TableColumnFilterTypes.Text),

      ),
    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));

    this.tableConfigurations.options.showSumRow = true;

    let currentYear = this.identityService.applicationUser.years.find(x => x.id === +this.identityService.applicationUser.yearId)

    this.filtersForm = new FormGroup({
      codeVoucherGroupId: new FormControl(),
      voucherStateId: new FormControl(0),
      fromDate: new FormControl(currentYear?.firstDate),
      toDate: new FormControl(currentYear?.lastDate),
      voucherNo: new FormControl(),
      fromVoucherNo: new FormControl(),
      toVoucherNo: new FormControl(),
    })

    forkJoin([
      this.get(),
      this._mediator.send(new GetCodeVoucherGroupsQuery(0, 0, [], 'id'))])
      .subscribe(([
                    getResponse,
                    codeVoucherGroups]) => {
        this.codeVoucherGroups = codeVoucherGroups.data;

      });
  }


  initialize(): any {

  }

  async get(id?: number) {

    this.voucherHeads = [];
    this.filterFormLastValue = this.filtersForm.getRawValue();
    this.filterFormLastValue.toDate = new Date(this.filterFormLastValue.toDate + 86399999);
    let searchQueries = this.getFilters();


    let request = new GetVoucherHeadsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, 'voucherNo DESC')
    let response = await this._mediator.send(request);
    this.voucherHeads = response.data;
    response.totalCount ? this.tableConfigurations.pagination.totalItems = response.totalCount : '';
    this.filteredVoucherHeadsTotalCount = response.totalCount;
  }


  getFilters(): SearchQuery[] {
    let searchQueries: SearchQuery[] = []

    if (this.filterFormLastValue.fromDate && this.filterFormLastValue.toDate) {
      searchQueries.push(new SearchQuery({
        propertyName: 'voucherDate',
        values: [this.filterFormLastValue.fromDate, this.filterFormLastValue.toDate],
        comparison: SearchComparisonTypes.between,
        nextOperand: SearchOperandTypes.and
      }))
    }
    if (this.filterFormLastValue.fromDate && !this.filterFormLastValue.toDate) {
      searchQueries.push(new SearchQuery({
        propertyName: 'voucherDate',
        values: [this.filterFormLastValue.fromDate],
        comparison: SearchComparisonTypes.greaterThan,
        nextOperand: SearchOperandTypes.and
      }))
    }
    if (!this.filterFormLastValue.fromDate && this.filterFormLastValue.toDate) {
      searchQueries.push(new SearchQuery({
        propertyName: 'voucherDate',
        values: [this.filterFormLastValue.toDate],
        comparison: SearchComparisonTypes.lessThan,
        nextOperand: SearchOperandTypes.and
      }))
    }

    if (!this.filterFormLastValue.voucherNo && this.filterFormLastValue.fromVoucherNo && this.filterFormLastValue.toVoucherNo) {
      searchQueries.push(new SearchQuery({
        propertyName: 'voucherNo',
        values: [this.filterFormLastValue.fromVoucherNo, this.filterFormLastValue.toVoucherNo],
        comparison: SearchComparisonTypes.between,
        nextOperand: SearchOperandTypes.and
      }))
    }
    if (!this.filterFormLastValue.voucherNo && this.filterFormLastValue.fromVoucherNo && !this.filterFormLastValue.toVoucherNo) {
      searchQueries.push(new SearchQuery({
        propertyName: 'voucherNo',
        values: [this.filterFormLastValue.fromVoucherNo],
        comparison: SearchComparisonTypes.greaterThan,
        nextOperand: SearchOperandTypes.and
      }))
    }
    if (!this.filterFormLastValue.voucherNo && !this.filterFormLastValue.fromVoucherNo && this.filterFormLastValue.toVoucherNo) {
      searchQueries.push(new SearchQuery({
        propertyName: 'voucherNo',
        values: [this.filterFormLastValue.toVoucherNo],
        comparison: SearchComparisonTypes.lessThan,
        nextOperand: SearchOperandTypes.and
      }))
    }
    if (this.filterFormLastValue.voucherNo) {
      searchQueries.push(new SearchQuery({
        propertyName: 'voucherNo',
        values: [this.filterFormLastValue.voucherNo],
        comparison: SearchComparisonTypes.equals,
        nextOperand: SearchOperandTypes.and
      }))
    }

    if (this.filterFormLastValue.voucherStateId) {
      searchQueries.push(new SearchQuery({
        propertyName: 'voucherStateId',
        values: [this.filterFormLastValue.voucherStateId],
        comparison: SearchComparisonTypes.equals,
        nextOperand: SearchOperandTypes.and
      }))
    }
    if (this.filterFormLastValue.codeVoucherGroupId) {
      searchQueries.push(new SearchQuery({
        propertyName: 'codeVoucherGroupId',
        values: [this.filterFormLastValue.codeVoucherGroupId],
        comparison: SearchComparisonTypes.equals,
        nextOperand: SearchOperandTypes.and
      }))
    }
    return searchQueries
  }

  async navigateToVoucherHead(voucherHead: VoucherHead) {
    await this.router.navigateByUrl(`/accounting/voucherHead/add?id=${voucherHead.id}`)
  }


  async handleCustomClick(customClickName: string) {
    if (customClickName === 'combine') await this.combineSelectedVoucherHeads()
    else await this.updateStatusBulk(customClickName)
  }

  async combineSelectedVoucherHeads() {

    // @ts-ignore
    let selectedVoucherHeads = this.voucherHeads.filter(x => x.selected === true);
    // @ts-ignore
    let vouchersThatHaveCorrectionRequest = this.voucherHeads.filter(x => x.selected && x.hasCorrectionRequest)
    if (vouchersThatHaveCorrectionRequest.length > 0) {
      let dialogConfig = new MatDialogConfig();
      let confirmDialogData = new ConfirmDialogConfig()
      confirmDialogData.color = 'red'
      confirmDialogData.title = 'اخطار'
      confirmDialogData.message = `اسناد شماره های ${vouchersThatHaveCorrectionRequest.map(x => x.voucherNo).join(",")} درخواست تغییرات فعال دارد و قابل ویرایش نمیباشد.`;
      confirmDialogData.actions = {
        confirm: {title: 'تایید', show: true},
        cancel: {title: '', show: false},
      }
      dialogConfig.data = confirmDialogData
      this.matDialog.open(ConfirmDialogComponent, dialogConfig)
      return;
    }

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      voucherHeads: selectedVoucherHeads
    }
    if (selectedVoucherHeads?.length > 1) {
      let dialogRef = this.matDialog.open(CombineVoucherHeadsDialogComponent, dialogConfig)
      dialogRef.afterClosed().subscribe(() => {
        this.get()
      })
    }
  }

  async updateStatusBulk(statusName: string) {
    let status = 0;
    if (statusName === 'lock') status = 3;
    if (statusName === 'review') status = 2;
    if (statusName === 'unlock') status = 1;

    if (this.filtersForm.value.voucherStateId === status || !status) return;

    let request = new BulkVoucherStatusUpdateCommand();
    request.status = status;
    // @ts-ignore
    let vouchersThatHaveCorrectionRequest = this.voucherHeads.filter(x => x.selected && x.hasCorrectionRequest)
    if (vouchersThatHaveCorrectionRequest.length > 0) {
      let dialogConfig = new MatDialogConfig();
      let confirmDialogData = new ConfirmDialogConfig()
      confirmDialogData.color = 'red'
      confirmDialogData.title = 'اخطار'
      confirmDialogData.message = `اسناد شماره های ${vouchersThatHaveCorrectionRequest.map(x => x.voucherNo).join(",")} درخواست تغییرات فعال دارد و قابل ویرایش نمیباشد.`;
      confirmDialogData.actions = {
        confirm: {title: 'تایید', show: true},
        cancel: {title: '', show: false},
      }
      dialogConfig.data = confirmDialogData
      this.matDialog.open(ConfirmDialogComponent, dialogConfig)
      return;
    } else {
      // @ts-ignore
      request.voucherIds = this.voucherHeads.filter(x => x.selected).map(x => x.id)
      if (request.voucherIds?.length > 0) this.filteredVoucherHeadsTotalCount = request.voucherIds.length
      request.conditions = this.getFilters();

      await this._mediator.send(request).then(async () => {

        this.notificationService.showSuccessMessage("تعداد" + ` ${this.filteredVoucherHeadsTotalCount} ` + "سند به وضعیت" + ` ${this.voucherStates.find(x => x.id == status)?.title} ` + "تغییر یافت.")
        return await this.get()
      })
    }


  }

  codeVoucherGroupDisplayFn(groupId: number) {
    let city = this.codeVoucherGroups.find(x => x.id === groupId)
    return city ? [city.code, city.title].join(' ') : '';
  }

  async update() {
  }


  add(): any {
  }

  delete(): any {
  }

  close(): any {
  }
}

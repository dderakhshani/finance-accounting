import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BaseComponent } from "../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../core/components/custom/table/models/table-configurations";
import { Mediator } from "../../../../core/services/mediator/mediator.service";
import { forkJoin } from "rxjs";
import { GetCodeVoucherGroupsQuery } from "../../repositories/code-voucher-group/queries/get-code-voucher-groups-query";

import { GetAccountReferencesQuery } from "../../repositories/account-reference/queries/get-account-references-query";
import { GetAccountHeadsQuery } from "../../repositories/account-head/queries/get-account-heads-query";
import { IdentityService } from "../../../identity/repositories/identity.service";
import { UserYear } from "../../../identity/repositories/models/user-year";
import { AccountHead } from "../../entities/account-head";
import { AccountReference } from "../../entities/account-reference";
import { AccountReferencesGroup } from "../../entities/account-references-group";
import { CodeVoucherGroup } from "../../entities/code-voucher-group";
import { ReportFormatTypes } from "../../repositories/reporting/ReportFormatTypes";
import { FormControl } from "@angular/forms";
import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {
  GetAccountReferencesGroupsQuery
} from "../../repositories/account-reference-group/queries/get-account-references-groups-query";
import { TableOptions } from "../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../core/components/custom/table/models/table-column";
import { GetVoucherHeadsQuery } from "../../repositories/voucher-head/queries/get-voucher-heads-query";
import { IRequest } from "../../../../core/services/mediator/interfaces";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import { BaseValue } from "../../../admin/entities/base-value";
import { AccountReviewReportResultModel } from '../../repositories/account-review/account-review-report-result-model';
import { GetAccountReviewReportQuery } from '../../repositories/reporting/queries/get-account-review-report-query';

@Component({
  selector: 'app-accounting-report-template',
  templateUrl: './accounting-report-template.component.html',
  styleUrls: ['./accounting-report-template.component.scss']
})
export class AccountingReportTemplateComponent extends BaseComponent {
  @Input() fetchOnInit = false;
  @Input() reportName: string = '';
  selectedTabIndex: number = 0;

  reportResult: AccountReviewReportResultModel[] = []
  originalReportResult: AccountReviewReportResultModel[] = []
  accountLevels: any[] = []
  accountLevelSelected: number = 0;
  voucherStates: any[] = []
  accountTypes: any[] = []
  reportTypes: any[] = []

  //userAllowedYears: UserYear[] = [];

  accountHeads: AccountHead[] = [];
  selectedAccountHeads: AccountHead[] = [];
  accountHeadControl = new FormControl();

  accountReferences: AccountReference[] = [];
  selectedAccountReferences: AccountReference[] = [];
  accountReferenceControl = new FormControl();

  accountReferencesGroups: AccountReferencesGroup[] = [];
  selectedAccountReferencesGroups: AccountReferencesGroup[] = [];
  accountReferencesGroupControl = new FormControl();

  codeVoucherGroups: CodeVoucherGroup[] = [];
  selectedCodeVoucherGroups: CodeVoucherGroup[] = [];
  codeVoucherGroupControl = new FormControl();

  tableConfigurations!: TableConfigurations;
  @Input() columns: TableColumn[] = []

  @Output() rowDoubleClicked = new EventEmitter<any>();
  @Output() onShowCurrencyRelatedFields = new EventEmitter<any>();

  @Output() printRialEmitter = new EventEmitter<any>();
  @Output() downloadRialExcelEmitter = new EventEmitter<any>();
  showCurrencyRelatedFieldsStatus: boolean = false;
  totalDebit = new FormControl();
  totalCredit = new FormControl();
  currencyTypes: BaseValue[] = [];

  constructor(
    private Mediator: Mediator,
    private identityService: IdentityService
  ) {
    super()
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
    this.isLoading = true;
    this.accountLevels = [
      {
        title: 'گروه',
        id: 1
      },
      {
        title: 'کل',
        id: 2
      },
      {
        title: 'معین',
        id: 3
      },
      {
        title: 'تفصیل',
        id: 4
      },
    ]
    this.voucherStates = [
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
    this.accountTypes = [
      {
        title: 'موقت',
        id: 1
      },
      {
        title: 'دائم',
        id: 2
      },
    ]
    this.reportTypes = [
      {
        title: 'چهار ستونی',
        id: 4
      },
      {
        title: 'شش ستونی',
        id: 6
      },
      {
        title: 'هشت ستونی',
        id: 8
      }
    ]

    this.tableConfigurations = new TableConfigurations(this.columns, new TableOptions())
    this.tableConfigurations.options.usePagination = true;

    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.useBuiltInFilters = true;
    this.tableConfigurations.options.useBuiltInSorting = true;
    this.tableConfigurations.options.hasDefaultSortKey = true;
    this.tableConfigurations.options.defaultSortKey = 'id';
    this.tableConfigurations.options.defaultSortDirection = 'ASC';


    await forkJoin([
      //this.identityService.getUserAllowedYears(),
      this.getAccountHeads(),
      this.getAccountReferences(),
      this.getAccountReferencesGroups(),
      this.getCodeVoucherGroups(),
      this.Mediator.send(new GetBaseValuesByUniqueNameQuery('currencyType'))
    ]).subscribe(async ([
      //userAllowedYears,
      accountHeads,
      accountReferences,
      accountReferencesGroups,
      codeVoucherGroups,
      currencyTypes]) => {
      //this.userAllowedYears = userAllowedYears.objResult;
      this.accountHeads = accountHeads;
      this.accountReferences = accountReferences;
      this.accountReferencesGroups = accountReferencesGroups;
      this.codeVoucherGroups = codeVoucherGroups;
      this.currencyTypes = currencyTypes;
      this.form.patchValue({
        currencyTypeBaseId: this.currencyTypes.find(x => x.uniqueName === 'IRR')?.id,
      })
      await this.initialize()
      this.isLoading = false;
      if (this.fetchOnInit) await this.get()
    })
  }

  async initialize(params?: any) {
    // let currentUserYear = this.userAllowedYears.find(x => x.id == this.identityService.applicationUser.yearId);
    // if (currentUserYear) currentUserYear._selected = true;

    this.form.patchValue({
      voucherDateFrom: this.identityService.getActiveYearStartDate(),
      voucherDateTo: new Date(new Date().setHours(0, 0, 0, 0)),
      companyId: +this.identityService.applicationUser.companyId,
      reportFormat: ReportFormatTypes.None
    })
    this.form.controls['yearIds'].controls.push(new FormControl(+this.identityService.applicationUser.yearId))
    this.form.patchValue(this.form.getRawValue())


    this.accountHeadControl.valueChanges.subscribe(async (newValue) => {
      if (typeof newValue !== "number") this.accountHeads = await this.getAccountHeads(newValue);
    })
    this.accountReferenceControl.valueChanges.subscribe(async (newValue) => {
      if (typeof newValue !== "number") this.accountReferences = await this.getAccountReferences(newValue);
    })
    this.accountReferencesGroupControl.valueChanges.subscribe(async (newValue) => {
      if (typeof newValue !== "number") this.accountReferencesGroups = await this.getAccountReferencesGroups(newValue);
    })
    this.codeVoucherGroupControl.valueChanges.subscribe(async (newValue) => {
      if (typeof newValue !== "number") this.codeVoucherGroups = await this.getCodeVoucherGroups(newValue);
      if (!newValue) this.form.controls['codeVoucherGroupId'].setValue(null)
    })
  }

  async onRowDoubleClicked(row: any) {
    this.rowDoubleClicked.emit(row);
  }
  async printRial() {
    this.printRialEmitter.emit(this.showCurrencyRelatedFieldsStatus);
  }
  downloadRialExcel() {
    this.downloadRialExcelEmitter.emit();
  }
  async get(param?: any) {
    this.isLoading = true;
    this.selectedTabIndex = 0;
    this.totalDebit.setValue(0)
    this.totalCredit.setValue(0)
    this.reportResult = [];
    let request = <GetAccountReviewReportQuery>this.request;
    if (param) {
      request = <GetAccountReviewReportQuery>param;
    }
    return await this.Mediator.send(request).then(res => {
      this.reportResult = res.filter((x: any) => ((
        x.debitBeforeDate ||
        x.creditBeforeDate ||
        x.debit ||
        x.credit ||
        x.remainDebit ||
        x.remainCredit ||
        x.debitAfterDate ||
        x.creditAfterDate
      ) && !this.showCurrencyRelatedFieldsStatus) ||
        ((
          x.debitCurrencyAmountBefore ||
          x.creditCurrencyAmountBefore ||
          x.debitCurrencyAmount ||
          x.creditCurrencyAmount ||
          x.debitCurrencyRemain ||
          x.creditCurrencyRemain ||
          x.debitCurrencyAmountAfter ||
          x.creditCurrencyAmountAfter
        ) && this.showCurrencyRelatedFieldsStatus));


      this.originalReportResult = res;

      this.updateTotalCreditAndTotalDebit()
      this.isLoading = false;
    }).catch((ex) => {
      console.warn(ex);
      this.isLoading = false;
    })

  }

  updateTotalCreditAndTotalDebit() {
    //let selectedItems = this.reportResult.filter(x => x.selected);
    //let items = selectedItems.length > 0 ? selectedItems : this.reportResult;
    let items = this.reportResult;

    if (this.showCurrencyRelatedFieldsStatus) {
      items.forEach(x => {
        x.debitBeforeDate = x.debitCurrencyAmountBefore;
        x.creditBeforeDate = x.creditCurrencyAmountBefore;
        x.debit = x.debitCurrencyAmount;
        x.credit = x.creditCurrencyAmount;
        x.remainDebit = x.debitCurrencyRemain;
        x.remainCredit = x.creditCurrencyRemain;
        x.debitAfterDate = x.debitCurrencyAmountAfter;
        x.creditAfterDate = x.creditCurrencyAmountAfter;
      });

    }
  }

  async getExcel() {
    let a = this.request;
    // @ts-ignore
    a.reportFormat = ReportFormatTypes.Excel;

    return await this.Mediator.send(a).then(res => {
      let binaryData = [];
      binaryData.push(res);
      let blob = new Blob(binaryData, { type: res.type });

      let a = document.createElement("a");
      document.body.appendChild(a);

      let url = window.URL.createObjectURL(blob);
      a.href = url;
      a.download = 'filename.xlsx';
      a.click();

      setTimeout(function () {
        window.URL.revokeObjectURL(url);
      }, 100);
    })
  }

  async exportFile(format: string) {
    let request = <GetAccountReviewReportQuery>this.request;
    // @ts-ignore
    if (format === 'pdf') request.reportFormat = ReportFormatTypes.Pdf;
    // @ts-ignore
    if (format === 'excel') request.reportFormat = ReportFormatTypes.Excel;
    // @ts-ignore
    if (format === 'word') request.reportFormat = ReportFormatTypes.Word;

    return await this.Mediator.send(request).then((res: any) => {
      let binaryData = [];
      binaryData.push(res);
      let blob = new Blob(binaryData, { type: res.type });

      let a = document.createElement("a");
      document.body.appendChild(a);

      let url = window.URL.createObjectURL(blob);
      a.href = url;

      if (format === 'pdf') a.download = 'filename.pdf';
      if (format === 'excel') a.download = 'filename.xlsx';
      if (format === 'word') a.download = 'filename.docx';
      a.click();

      setTimeout(function () {
        window.URL.revokeObjectURL(url);
      }, 100);
    })
  }

  writeReportToIframe() {

    let iframe = <any>document.getElementById('iframe');
    let content = this.reportResult;
    let doc = iframe.contentDocument || iframe.contentWindow;
    doc.open();
    doc.write(content);
    doc.close();
  }

  async getAccountHeads(query?: string) {
    let searchQueries: SearchQuery[] = [];

    if (query) {
      searchQueries.push(new SearchQuery({
        propertyName: 'fullCode',
        comparison: 'contains',
        values: [query],
        nextOperand: 'or'
      }))
      searchQueries.push(new SearchQuery({
        propertyName: 'title',
        comparison: 'contains',
        values: [query],
        nextOperand: 'or'
      }))
    }
    // searchQueries.push(new SearchQuery({
    //   propertyName: 'lastLevel',
    //   comparison: 'equal',
    //   values: [true],
    //   nextOperand: 'and'
    // }))
    return await this.Mediator.send(new GetAccountHeadsQuery(0, 0, searchQueries, "fullCode"))
  }

  resetAccountHeadIdsFilter() {
    // @ts-ignore
    this.request.accountHeadIds = []
    this.form.controls['accountHeadIds'].value = [];
    this.form.controls['accountHeadIds'].controls = [];

  }

  accountHeadDisplayFn(accountHeadId: number) {
    let accountHead = this.accountHeads.find(x => x.id === accountHeadId) ?? this.selectedAccountHeads.find(x => x.id === accountHeadId)
    return accountHead ? [accountHead.fullCode, accountHead.title].join(' ') : '';
  }

  handleAccountHeadSelection(accountHeadId: number) {
    this.selectedAccountHeads.push(<AccountHead>this.accountHeads.find(x => x.id === accountHeadId))
    this.form.controls['accountHeadIds'].controls.push(new FormControl(accountHeadId))
    this.form.patchValue(this.form.getRawValue())
    this.accountHeadControl.setValue(null)
  }

  removeAccountHead(accountHeadId: number) {
    this.selectedAccountHeads.slice(this.selectedAccountHeads.findIndex(x => x.id === accountHeadId))
    this.form.controls['accountHeadIds'].removeAt(this.form.controls['accountHeadIds'].value.findIndex((x: any) => x === accountHeadId))
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
    return await this.Mediator.send(new GetAccountReferencesQuery(1, 10, searchQueries, "code")).then(res => res.data)
  }

  accountReferenceDisplayFn(accountReferenceId: number) {
    let accountReference = this.accountReferences.find(x => x.id === accountReferenceId) ?? this.selectedAccountReferences.find(x => x.id === accountReferenceId)
    return accountReference ? [accountReference.code, accountReference.title].join(' ') : '';
  }

  handleAccountReferenceSelection(accountReferenceId: number) {
    this.selectedAccountReferences.push(<AccountReference>this.accountReferences.find(x => x.id === accountReferenceId))
    this.form.controls['referenceIds'].controls.push(new FormControl(accountReferenceId))
    this.form.patchValue(this.form.getRawValue())
    this.accountReferenceControl.setValue(null)
  }

  removeAccountReference(accountReferenceId: number) {
    this.selectedAccountReferences.slice(this.selectedAccountReferences.findIndex(x => x.id === accountReferenceId))
    this.form.controls['referenceIds'].removeAt(this.form.controls['referenceIds'].value.findIndex((x: any) => x === accountReferenceId))
  }


  async getAccountReferencesGroups(query?: string) {
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
    return await this.Mediator.send(new GetAccountReferencesGroupsQuery(1, 10, searchQueries, "code")).then(res => res.data)
  }

  accountReferencesGroupDisplayFn(accountReferencesGroupId: number) {
    let accountReferencesGroup = this.accountReferencesGroups.find(x => x.id === accountReferencesGroupId) ?? this.selectedAccountReferencesGroups.find(x => x.id === accountReferencesGroupId)

    return accountReferencesGroup ? [accountReferencesGroup.code, accountReferencesGroup.title].join(' ') : '';
  }

  handleAccountReferencesGroupSelection(accountReferencesGroupId: number) {
    this.selectedAccountReferencesGroups.push(<AccountReferencesGroup>this.accountReferencesGroups.find(x => x.id === accountReferencesGroupId))
    this.form.controls['referenceGroupIds'].controls.push(new FormControl(accountReferencesGroupId))
    this.form.patchValue(this.form.getRawValue())
    this.accountReferencesGroupControl.setValue(null)
  }

  removeAccountReferencesGroup(accountReferencesGroupId: number) {
    this.selectedAccountReferencesGroups.slice(this.selectedAccountReferencesGroups.findIndex(x => x.id === accountReferencesGroupId))
    this.form.controls['referenceGroupIds'].removeAt(this.form.controls['referenceGroupIds'].value.findIndex((x: any) => x === accountReferencesGroupId))
  }

  async getCodeVoucherGroups(query?: string) {
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
    return await this.Mediator.send(new GetCodeVoucherGroupsQuery(1, 10, searchQueries)).then(res => res.data)
  }

  handleCodeVoucherGroupSelection(codeVoucherGroupId: number) {
    this.selectedCodeVoucherGroups.push(<CodeVoucherGroup>this.codeVoucherGroups.find(x => x.id === codeVoucherGroupId));
    this.form.controls['codeVoucherGroupIds'].controls.push(new FormControl(codeVoucherGroupId))
    this.form.patchValue(this.form.getRawValue())
    this.codeVoucherGroupControl.setValue(null)
  }

  codeVoucherGroupDisplayFn(codeVoucherGroupId: number) {
    let codeVoucherGroup = this.codeVoucherGroups.find(x => x.id === codeVoucherGroupId) ?? this.selectedCodeVoucherGroups.find(x => x.id === codeVoucherGroupId);
    return codeVoucherGroup ? [codeVoucherGroup.code, codeVoucherGroup.title].join(' ') : '';
  }

  showCurrencyRelatedFields() {

    this.showCurrencyRelatedFieldsStatus = this.currencyTypes.find(x => x.id == this.form.getRawValue().currencyTypeBaseId)?.uniqueName !== 'IRR';
    this.reportResult = this.originalReportResult.filter((x: any) => ((
      x.debitBeforeDate ||
      x.creditBeforeDate ||
      x.debit ||
      x.credit ||
      x.remainDebit ||
      x.remainCredit ||
      x.debitAfterDate ||
      x.creditAfterDate
    ) && !this.showCurrencyRelatedFieldsStatus) ||
      ((
        x.debitCurrencyAmountBefore ||
        x.creditCurrencyAmountBefore ||
        x.debitCurrencyAmount ||
        x.creditCurrencyAmount ||
        x.debitCurrencyRemain ||
        x.creditCurrencyRemain ||
        x.debitCurrencyAmountAfter ||
        x.creditCurrencyAmountAfter
      ) && this.showCurrencyRelatedFieldsStatus));
    this.onShowCurrencyRelatedFields.emit(this.showCurrencyRelatedFieldsStatus)

    this.get()
  }

  removeCodeVoucherGroup(codeVoucherGroupId: number) {
    this.selectedCodeVoucherGroups.slice(this.selectedCodeVoucherGroups.findIndex(x => x.id === codeVoucherGroupId))
    this.form.controls["codeVoucherGroupIds"].removeAt(this.form.controls['codeVoucherGroupIds'].value.findIndex((x: any) => x === codeVoucherGroupId))
  }

  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }


  update(param?: any): any {
  }

  removeAllFilters() {
    this.selectedCodeVoucherGroups = [];
    this.form.controls['codeVoucherGroupIds'].value = [];
    this.form.controls['codeVoucherGroupIds'].controls = [];
    // @ts-ignore
    this.request.codeVoucherGroupIds = [];
    this.codeVoucherGroupControl.setValue(null);

    this.selectedAccountReferences = [];
    this.form.controls['referenceIds'].value = [];
    this.form.controls['referenceIds'].controls = [];
    // @ts-ignore
    this.request.referenceIds = [];
    this.accountReferenceControl.setValue(null);


    this.selectedAccountReferencesGroups = [];
    this.form.controls['referenceGroupIds'].value = [];
    this.form.controls['referenceGroupIds'].controls = [];
    // @ts-ignore
    this.request.referenceGroupIds = [];
    this.accountReferencesGroupControl.setValue(null);

    // @ts-ignore
    this.request.accountHeadIds = []
    this.selectedAccountHeads = [];
    this.form.controls['accountHeadIds'].value = [];
    this.form.controls['accountHeadIds'].controls = [];
    this.accountHeadControl.setValue(null);

    this.form.controls['creditFrom'].value = '';
    this.form.controls['creditTo'].value = '';
    this.form.controls['debitFrom'].value = '';
    this.form.controls['debitTo'].value = '';
    this.form.controls['voucherRowDescription'].value = '';
  }
}

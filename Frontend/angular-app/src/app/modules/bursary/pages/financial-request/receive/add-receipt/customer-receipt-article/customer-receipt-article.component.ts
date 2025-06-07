import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { PageModes } from 'src/app/core/enums/page-modes';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { BaseValue } from 'src/app/modules/admin/entities/base-value';
import { AccountHead } from 'src/app/modules/bursary/entities/account-head';
import { AccountReference } from 'src/app/modules/bursary/entities/account-reference';
import { AccountReferencesGroup } from 'src/app/modules/bursary/entities/account-reference-group';
import { ChequeSheet } from 'src/app/modules/bursary/entities/cheque-sheet';
import { Documents, CurrencyType, CodeVouchers, AccountHeads, AccountTypes, FinantialReferences, AccountReferencesGroupEnums } from 'src/app/modules/bursary/entities/enums';
import { GetChqueSheetsQuery } from 'src/app/modules/bursary/repositories/cheque/queries/get-cheque-sheets-query';
import {  GetBursaryAccountHeadsQuery } from 'src/app/modules/bursary/repositories/financial-request/account-head/queries/get-bursary-account-heads-query';
import {  GetBursaryAccountReferenceGroupsQuery } from 'src/app/modules/bursary/repositories/financial-request/account-reference-group/queries/get-bursary-reference-groups-query';
import {GetBursaryAccountReferencesQuery } from 'src/app/modules/bursary/repositories/financial-request/referenceAccount/queries/get-account-references-query';
import { Bank } from 'src/app/modules/logistics/entities/bank';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';
import { AddReceiveChequeComponent } from '../../cheque/add-receive-cheque.component';

@Component({
  selector: 'app-customer-receipt-article',
  templateUrl: './customer-receipt-article.component.html',
  styleUrls: ['./customer-receipt-article.component.scss']
})

export class CustomerReceiptArticleComponent extends BaseComponent {


//TODO : در صورتی که یک اکانت هد گروهی نداشته باشد برنامه هنگام جستجو باگ دارد


  public documents = Documents;
  public currencyType = CurrencyType;
  public codeVoucherGroupEnums = CodeVouchers;
  public accountHeadEnums = AccountHeads;
   public finantialReferencesEnums = FinantialReferences;
  public moneyTypes: any[] = [{ "value": true, "title": "ریالی" }, { "value": false, "title": "ارزی" }];
  public useReferenceTypes: any[] = [{ "value": true, "title": "با کد تفصیل" }, { "value": false, "title": "بدون کد تفصیل" }];
  public isUploading !: boolean;
  public nonRialTypes: any = [{ "value": 1, "title": "واریز مشتری به صرافی" },
  { "value": 2, "title": "حواله صرافی به حساب ارزی در بانک" },
  { "value": 3, "title": "حواله ریالی صرافی به حساب ریالی در بانک" },
  { "value": 4, "title": "حواله مشتری به تامین کننده" },
];

  @Input() index!: number;
  @Input() documentTypes: BaseValue[] = [];
  @Input() currencyTypes: BaseValue[] = [];
  @Input() referenceTypes: BaseValue[] = [];
  @Input() chequeTypes: BaseValue[] = [];
  @Input() finantialReferenceTypes: BaseValue[] = [];
  @Input() chequeSheets: ChequeSheet[] = [];
  @Input() banks: Bank[] = [];
  @Output() removeItem: EventEmitter<any> = new EventEmitter<any>();

  set files(values: string[]) {
    values.forEach((value: string) => {
      this.form.controls.files.controls.push(new FormControl(value));
    })
  }

  accountReferenceGroupEnums = AccountReferencesGroupEnums;
  creditAccountHeads: AccountHead[] = [];
  creditAccountReferenceGroups: AccountReferencesGroup[] = [];
  creditAccountReferences: AccountReference[] = [];
  debitAccountHeads: AccountHead[] = [];
  debitAccountReferenceGroups: AccountReferencesGroup[] = [];
  debitAccountReferences: AccountReference[] = [];

  constructor(private _mediator: Mediator, public dialog: MatDialog) {
    super();

  }

  async ngOnInit() {
    await this.resolve()
  }
  async resolve(params?: any) {

    await this.getAccountHeads(AccountTypes.Credit)
    await this.getAccountReferences(AccountTypes.Credit)
    await this.getAccountReferencesGroups(AccountTypes.Credit)
    await this.getAccountHeads(AccountTypes.Debit)
    await this.getAccountReferences(AccountTypes.Debit)
    await this.getAccountReferencesGroups(AccountTypes.Debit)
    this.form.patchValue(this.form.getRawValue())
    this.initialize()
  }


  disableCredit(){
  if (this.form.get('isCreditAccountHead')?.value != true){
  this.form.controls['creditAccountReferenceGroupId'].disable({ emitEvent: false });
  this.form.controls['creditAccountReferenceId'].disable({ emitEvent: false });
}
else
{
  this.form.controls['creditAccountReferenceGroupId'].enable();
  this.form.controls['creditAccountReferenceId'].enable();
}


}

disableDebits(){
    if (this.form.get('isDebitAccountHead')?.value != true){
    this.form.controls['debitAccountReferenceGroupId'].disable({ emitEvent: false });
    this.form.controls['debitAccountReferenceId'].disable({ emitEvent: false });
  }
  else
  {
    this.form.controls['debitAccountReferenceGroupId'].enable();
    this.form.controls['debitAccountReferenceId'].enable();
  }


}
  initialize(params?: any) {


    this.form.controls['creditAccountHeadId'].valueChanges.subscribe(async (newVal: any) => {
      await this.getAccountHeads(AccountTypes.Credit);
      if (typeof newVal === 'number' || !newVal) await this.getAccountReferencesGroups(AccountTypes.Credit);
    })

    this.form.controls['creditAccountReferenceGroupId'].valueChanges.subscribe(async (newVal: any) => {
      await this.getAccountReferencesGroups(AccountTypes.Credit);
      if (typeof newVal === 'number' || !newVal) {
        await this.getAccountHeads(AccountTypes.Credit);
        await this.getAccountReferences(AccountTypes.Credit);
      }
    })

    this.form.controls['creditAccountReferenceId'].valueChanges.subscribe(async (newVal: any) => {
      await this.getAccountReferences(AccountTypes.Credit);
      if (typeof newVal === 'number' || !newVal) await this.getAccountReferencesGroups(AccountTypes.Credit);
    })

    this.form.controls['debitAccountHeadId'].valueChanges.subscribe(async (newVal: any) => {
      await this.getAccountHeads(AccountTypes.Debit);
      if (typeof newVal === 'number' || !newVal) await this.getAccountReferencesGroups(AccountTypes.Debit);
    })

    this.form.controls['debitAccountReferenceGroupId'].valueChanges.subscribe(async (newVal: any) => {
      await this.getAccountReferencesGroups(AccountTypes.Debit);
      if (typeof newVal === 'number' || !newVal) {
        await this.getAccountHeads(AccountTypes.Debit);
        await this.getAccountReferences(AccountTypes.Debit);
      }
    })

    this.form.controls['debitAccountReferenceId'].valueChanges.subscribe(async (newVal: any) => {
      await this.getAccountReferences(AccountTypes.Debit);
      if (typeof newVal === 'number' || !newVal) await this.getAccountReferencesGroups(AccountTypes.Debit);
    });
    if (this.pageMode == PageModes.Add) {
      this.form.patchValue({
        besCurrencyStatus: false,
        bedCurrencyStatus: false
      });
      this.form.controls['isRial'].valueChanges.subscribe(
        (value:any)=>{
          if(value){
            // "Fix Rial currency mode - sets foreign currency debit to false"
            this.form.controls['bedCurrencyStatus'].setValue(false)
          }else {
            // "Fix Rial currency mode - sets foreign currency debit to true"
            this.form.controls['bedCurrencyStatus'].setValue(true)
          }
        }
      );
    }


  }

  add(param?: any) {

  }
  get(param?: any) {

  }

  update(param?: any) {

  }

  delete(param?: any) {

  }

  close() {

  }

  async getAccountHeads(accountType: AccountTypes) {

    let accountHeadId = accountType === AccountTypes.Credit ? this.form.controls['creditAccountHeadId']?.value : this.form.controls['debitAccountHeadId']?.value
    let accountReferenceGroupId = accountType === AccountTypes.Credit ? this.form.controls['creditAccountReferenceGroupId']?.value : this.form.controls['debitAccountReferenceGroupId']?.value
    let isAccountHeadIdNumeric = typeof accountHeadId === 'number'
    let isAccountReferenceGroupIdNumeric = typeof accountReferenceGroupId === 'number'
    let searchTerm = !isAccountHeadIdNumeric && typeof accountHeadId === 'string' ? accountHeadId : null;

    let filter: SearchQuery[] = []

    if (!isAccountHeadIdNumeric && isAccountReferenceGroupIdNumeric) {
      filter.push({
        propertyName: 'accountReferenceGroupsIds',
        comparison: 'inList',
        values: [accountReferenceGroupId],
        nextOperand: 'and'
      })
    }

    if (isAccountHeadIdNumeric) {
      filter.push({
        propertyName: 'id',
        comparison: 'equal',
        values: [accountHeadId],
        nextOperand: 'and'
      })
    }

    if (searchTerm) {
      filter.push({
        propertyName: 'title',
        comparison: 'contains',
        values: [accountHeadId],
        nextOperand: 'and'
      })
    }

    await this._mediator.send(new GetBursaryAccountHeadsQuery(0, 25, filter)).then((res) => {
      accountType === AccountTypes.Credit ? this.creditAccountHeads = res.data : this.debitAccountHeads = res.data
    });
  }

  async getAccountReferencesGroups(accountType: AccountTypes) {

    let accountReferenceGroupId = accountType === AccountTypes.Credit ? this.form.controls['creditAccountReferenceGroupId']?.value : this.form.controls['debitAccountReferenceGroupId']?.value
    let isAccountReferenceGroupIdNumeric = typeof accountReferenceGroupId === 'number'
    let accountHeadId = accountType === AccountTypes.Credit ? this.form.controls['creditAccountHeadId']?.value : this.form.controls['debitAccountHeadId']?.value;
    let isAccountHeadIdNumeric = typeof accountHeadId === 'number'
    let accountReferenceId = accountType === AccountTypes.Credit ? this.form.controls['creditAccountReferenceId']?.value : this.form.controls['debitAccountReferenceId']?.value;
    let isAccountReferenceIdNumeric = typeof accountReferenceId === 'number'
    let searchTerm = !isAccountReferenceGroupIdNumeric && typeof accountReferenceGroupId === 'string' ? accountReferenceGroupId : null;

    let filter: SearchQuery[] = []

    if (!isAccountReferenceGroupIdNumeric && isAccountHeadIdNumeric && !isAccountReferenceIdNumeric) {
      filter.push({
        propertyName: 'id',
        comparison: 'in',
        values: accountType === AccountTypes.Credit ? [...(this.creditAccountHeads.find(x => x.id === accountHeadId)?.accountReferenceGroupsIds as Number[])] : [...(this.debitAccountHeads.find(x => x.id === accountHeadId)?.accountReferenceGroupsIds as Number[])],
        nextOperand: 'and'
      })

      if (accountType === AccountTypes.Credit) {
        this.form.controls['creditAccountReferenceId'].disable({ emitEvent: false })
        this.form.controls['creditAccountReferenceId'].setValue(null, { emitEvent: false })
      } else {
        this.form.controls['debitAccountReferenceId'].disable({ emitEvent: false })
        this.form.controls['debitAccountReferenceId'].setValue(null, { emitEvent: false })
      }
    } else {
      accountType === AccountTypes.Credit ? this.form.controls['creditAccountReferenceId'].enable({ emitEvent: false }) : this.form.controls['debitAccountReferenceId'].enable({ emitEvent: false })
    }

    if (!isAccountReferenceGroupIdNumeric && isAccountReferenceIdNumeric && !isAccountHeadIdNumeric) {
      filter.push({
        propertyName: 'id',
        comparison: 'in',
        values: [...(this.creditAccountReferences.find(x => x.id === accountReferenceId)?.accountReferencesGroupsIdList as Number[])],
        nextOperand: 'and'
      })

      if (accountType === AccountTypes.Credit) {
        this.form.controls['creditAccountHeadId'].disable({ emitEvent: false })
        this.form.controls['creditAccountHeadId'].setValue(null, { emitEvent: false })
      } else {
        this.form.controls['debitAccountHeadId'].disable({ emitEvent: false })
        this.form.controls['debitAccountHeadId'].setValue(null, { emitEvent: false })
      }
    } else {
      accountType === AccountTypes.Credit ? this.form.controls['creditAccountHeadId'].enable({ emitEvent: false }) : this.form.controls['debitAccountHeadId'].enable({ emitEvent: false })
    }

    if (isAccountReferenceGroupIdNumeric && (this.pageMode === PageModes.Update || ((isAccountHeadIdNumeric && !isAccountReferenceIdNumeric) || (!isAccountHeadIdNumeric && isAccountReferenceIdNumeric)))) {
      filter.push({
        propertyName: 'id',
        comparison: 'equal',
        values: [accountReferenceGroupId],
        nextOperand: 'and'
      })
    }

    if (searchTerm) {
      filter.push({
        propertyName: 'title',
        comparison: 'contains',
        values: [accountReferenceGroupId],
        nextOperand: 'and'
      })
    }
this.isLoading = true;
    await this._mediator.send(new GetBursaryAccountReferenceGroupsQuery(0, 25, filter)).then((res) => {
      accountType === AccountTypes.Credit ? this.creditAccountReferenceGroups = res.data : this.debitAccountReferenceGroups = res.data
    });
    this.isLoading = false;

  }

  async getAccountReferences(accountType: AccountTypes) {

    let accountReferenceGroupId = accountType === AccountTypes.Credit ? this.form.controls['creditAccountReferenceGroupId']?.value : this.form.controls['debitAccountReferenceGroupId']?.value;
    let isAccountReferenceGroupIdNumeric = typeof accountReferenceGroupId === 'number'
    let accountReferenceId = accountType === AccountTypes.Credit ? this.form.controls['creditAccountReferenceId']?.value : this.form.controls['debitAccountReferenceId']?.value;
    let isAccountReferenceIdNumeric = typeof accountReferenceId === 'number'
    let searchTerm = !isAccountReferenceIdNumeric && typeof accountReferenceId === 'string' ? accountReferenceId : null;

    let filter: SearchQuery[] = []

    if (!isAccountReferenceIdNumeric && isAccountReferenceGroupIdNumeric) {
      filter.push({
        propertyName: 'accountReferencesGroupsIdList',
        comparison: 'inList',
        values: [accountReferenceGroupId],
        nextOperand: 'and'
      })
    }

    if (isAccountReferenceIdNumeric) {
      filter.push({
        propertyName: 'id',
        comparison: 'equal',
        values: [accountReferenceId],
        nextOperand: 'and'
      })
    }

    if (searchTerm) {
      filter.push({
        propertyName: 'title',
        comparison: 'contains',
        values: [accountReferenceId],
        nextOperand: 'and'
      })
    }

    await this._mediator.send(new GetBursaryAccountReferencesQuery(0, 25, filter)).then((res) => {
      accountType === AccountTypes.Credit ? this.creditAccountReferences = res.data : this.debitAccountReferences = res.data
    });

  }

  accountHeadDisplayFn(id: number) {
    return this.creditAccountHeads.find(x => x.id === id)?.title ?? this.debitAccountHeads.find(x => x.id === id)?.title
  }

  accountReferenceGroupDisplayFn(id: number) {
    return this.creditAccountReferenceGroups.find(x => x.id === id)?.title ?? this.debitAccountReferenceGroups.find(x => x.id === id)?.title
  }

  accountReferenceDisplayFn(id: number) {
    return this.creditAccountReferences.find(x => x.id === id)?.title ?? this.debitAccountReferences.find(x => x.id === id)?.title
  }

  changeDocumentsType(form: FormGroup) {

    if (form.get('documentTypeBaseId')?.value == this.documents.Cash && form.get('isRial')?.value == this.currencyType.NonRial){

      form.patchValue({

         creditAccountHeadId: this.accountHeadEnums.AccountHeadCode_2304,
         creditAccountReferenceGroupId : this.accountReferenceGroupEnums.AccountReferencesGroupCode_32,
         debitAccountHeadId:  this.accountHeadEnums.AccountHeadCode_2605,
         debitAccountReferenceGroupId : this.accountReferenceGroupEnums.AccountReferencesGroupCode_01002002,
      });
    }
    else{
      form.patchValue({
        debitAccountHeadId: form.value.documentTypeBaseId == this.documents.ChequeSheet ? this.accountHeadEnums.AccountHeadCode_2301 : this.accountHeadEnums.AccountHeadCode_2601,
        creditAccountHeadId: this.accountHeadEnums.AccountHeadCode_2304,

      });
    }
  }


  disableCredits(form:FormGroup){
    if (form.get('nonReferenceCode')?.value == false){

    }
  }

  fillAccountHeadByNonRialType(id: number, receipt: FormGroup) {

    if (this.form.get('documentTypeBaseId')?.value != this.documents.Cash)
{
  let creditAccountHeadId = 0;
  let debitAccountHeadId = 0;
  let debitAccountReferenceGroupId = 0;

  if (id == 1) {

    creditAccountHeadId = this.accountHeadEnums.AccountHeadCode_2304;
    debitAccountHeadId = this.accountHeadEnums.AccountHeadCode_2410;
    debitAccountReferenceGroupId = this.accountReferenceGroupEnums.AccountReferencesGroupCode_06004;
    receipt.patchValue({
      creditAccountHeadId: creditAccountHeadId,
      debitAccountHeadId: debitAccountHeadId,
      debitAccountReferenceGroupId : debitAccountReferenceGroupId
    });
    this.getAccountReferencesGroups(AccountTypes.Debit);

  }

  else if (id == 2){
      creditAccountHeadId = this.accountHeadEnums.AccountHeadCode_2602;
      debitAccountHeadId = this.accountHeadEnums.AccountHeadCode_2401;
      debitAccountReferenceGroupId = this.accountReferenceGroupEnums.AccountReferencesGroupCode_06004;

      receipt.patchValue({
        creditAccountHeadId: creditAccountHeadId,
        debitAccountHeadId: debitAccountHeadId,
        debitAccountReferenceGroupId : debitAccountReferenceGroupId
      });
      this.getAccountReferencesGroups(AccountTypes.Debit);

  }
  else if (id == 3){
    creditAccountHeadId = this.accountHeadEnums.AccountHeadCode_2601; // 2601
debitAccountHeadId = this.accountHeadEnums.AccountHeadCode_2410;
    debitAccountReferenceGroupId = this.accountReferenceGroupEnums.AccountReferencesGroupCode_06004;

    receipt.patchValue({
      creditAccountHeadId: creditAccountHeadId,
      debitAccountHeadId: debitAccountHeadId,
      debitAccountReferenceGroupId : debitAccountReferenceGroupId
    });
    this.getAccountReferencesGroups(AccountTypes.Debit);

  }
    else if (id == 4){

    receipt.patchValue({
      creditAccountHeadId: this.accountHeadEnums.AccountHeadCode_2304,
      debitAccountHeadId: this.accountHeadEnums.AccountHeadCode_5102,

    })
  }

}

  }

  removeArticle(index: any) {
    this.removeItem.emit(this.index);
  }


  ttt(event:any,receip:FormGroup){
if (event.key === '-'){

  receip.patchValue({
    amount:receip.value.amount+"000"
  });

  //receip.value.amount = receip.value.amount + "000";
}
  }

  // Fill amount : amount = currncyFee * currencyAmount
  // or
  // Fill CurrencyAmount : currencyFee / amount
  calculateAmount(receipt: FormGroup) {



     if (receipt.value.currencyFee != null && receipt.value.currencyFee != 0)
      receipt.patchValue({
        amount: receipt.value.currencyFee * receipt.value.currencyAmount
      })
    // else
    //   receipt.patchValue({
    //     currencyAmount: receipt.value.currencyFee / receipt.value.amount
    //   })
  }

  autoFillChequeDetails(chequeSheet: FormGroup, sheetId: any) {
    let cs !: ChequeSheet;
    this.chequeSheets.forEach((item) => {
      if (item.id == sheetId)
        cs = item;
    })
    chequeSheet.patchValue(cs);
  }

  chequeSheetsSelect(id: number, form: FormGroup) {

    form.controls.id.setValue(id);
    this.autoFillChequeDetails(form, id);
  }

  async chequeSheetsFilter(searchTerm: string, defaultId: any) {
    var filters: any = undefined;

    if (searchTerm != '') {
      filters = [
        new SearchQuery({
          propertyName: 'sheetUniqueNumber',
          comparison: 'contains',
          values: [searchTerm],
          nextOperand: 'and'
        }),
      ];
    }

    if (defaultId != undefined) {
      filters = [
        new SearchQuery({
          propertyName: 'id',
          comparison: 'equal',
          values: [defaultId],
          nextOperand: 'and'
        }),
      ]
    }
    else {
      filters.push(
        new SearchQuery({
          propertyName: 'isActive',
          comparison: 'equal',
          values: [true],

        }),
      )}

    await this._mediator.send(new GetChqueSheetsQuery(0, 25, filters)).then(res => {
      this.chequeSheets = res.data
    })
  }

  addChequeSheet(form: FormGroup) {

    const dialogRef = this.dialog.open(AddReceiveChequeComponent, {
      width: '80%',
      height: '80%',
    });

    dialogRef.afterClosed().subscribe(result => {
      this._mediator.send(new GetChqueSheetsQuery()).then(x => {
        this.chequeSheets = x.data
        this.chequeSheetsFilter("", undefined);
      })
    });
  }
}





import {AfterViewInit, Component, OnInit} from '@angular/core';

import {AccountManagerService} from "../../../../../accounting/services/account-manager.service";
import {BehaviorSubject, forkJoin} from "rxjs";
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {CurrencyType, Documents} from "../../../../entities/enums";
import {BaseValue} from "../../../../../admin/entities/base-value";
import {GetPayTypesQuery} from "../../../../repositories/payTypes/queries/get-pay-types";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {GetChequeTypesQuery} from "../../../../repositories/chequetypes/queries/get-cheque-types";
import {AccountHead} from "../../../../../accounting/entities/account-head";
import {AccountReferencesGroup} from "../../../../../accounting/entities/account-references-group";
import {AccountReference} from "../../../../../accounting/entities/account-reference";
import {FormControl, FormGroup} from "@angular/forms";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import {GetPaySubjectsListQuery} from "../../../../repositories/payables-documents/queries/get-pay-subjects-list";
import {ChequeBooksSheet} from "../../../../entities/cheque-books-sheet";
import {
  PayablesChequeBooksSheetsQuery
} from "../../../../repositories/payables_cheque_book-sheets/queries/payables-cheque-books-sheets-query";
import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {BankAccounts} from "../../../../entities/bank-accounts";
import {GetBankAccounts} from "../../../../repositories/bank-accounts/queries/get-bank-accounts";
import {Toastr_Service} from "../../../../../../shared/services/toastrService/toastr_.service";

import {MatSlideToggleChange} from "@angular/material/slide-toggle";
import {CustomDecimalPipe} from "../../../../../../core/components/custom/table/table-details/pipe/custom-decimal.pipe";
import {PayTypesEnum} from "../../../../entities/pay_types";

export enum MonetarySystem {
  Rial = 29340,
  Currency = 29341
}

@Component({
  selector: 'app-payable-item-detail',
  templateUrl: './payable-item-detail.component.html',
  styleUrls: ['./payable-item-detail.component.scss']
})
export class PayableItemDetailComponent extends BaseComponent implements OnInit, AfterViewInit {


  accountReferenceControl = new FormControl();

  accountHeadsCredit$: BehaviorSubject<AccountHead[]> = new BehaviorSubject<AccountHead[]>([]);
  accountReferenceGroupsCredit$: BehaviorSubject<AccountReferencesGroup[]> = new BehaviorSubject<AccountReferencesGroup[]>([]);
  accountReferencesCredit$: BehaviorSubject<AccountReference[]> = new BehaviorSubject<AccountReference[]>([]);

  currencyTypes: BaseValue[] = [];
  codeVoucherGroupId!: number;
  protected readonly currencyType = CurrencyType;
  protected readonly documents = Documents;
  controlsCredit = ['creditAccountHeadId', 'creditReferenceGroupId', 'creditReferenceId']
  public moneyTypes: any[] = [
    {value: 29340, title: "ریالی"},
    {value: 29341, title: "ارزی"}
  ];

  payTypes!: BaseValue[];
  paySubjectsList!: BaseValue[];
  chequeTypes !: BaseValue[];
  filteredChequeTypes !: BaseValue[];
  private isUpdatingCurrency: boolean = false;
  chequeBookSheet: ChequeBooksSheet[] = [];

  bankAccounts: BankAccounts[] = [];
  private isUpdating!: boolean;
  isCalculateByCurrency: boolean = false;
  payTypesEnum = PayTypesEnum;
  monetarySystemEnum = MonetarySystem;

  constructor(public accountManagerService: AccountManagerService,
              private toastr: Toastr_Service,
              private customDecimal: CustomDecimalPipe,
              private _mediator: Mediator,) {
    super();
  }

  async ngOnInit(): Promise<void> {
    await this.resolve();

  }

  async resolve(params?: any) {
    this.isLoading = true;
    forkJoin([
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('currencyType')),
      this._mediator.send(new GetPayTypesQuery()),
      this._mediator.send(new GetChequeTypesQuery()),
      this._mediator.send(new GetPaySubjectsListQuery()),
      this.getChequeBooksSheet(),
      this.getBankAccounts(),

    ])
      .subscribe(async ([
                          currencyTypes,
                          payTypes,
                          chequeTypes,
                          paySubjectsList,
                          chequeBookSheet,
                          bankAccounts,
                        ]) => {
        this.currencyTypes = currencyTypes;
        this.currencyTypes = this.currencyTypes.filter((ct: any) => ct.title !== 'ریال');
        this.payTypes = payTypes;
        this.chequeTypes = chequeTypes;
        this.paySubjectsList = paySubjectsList;
        this.chequeBookSheet = chequeBookSheet;
        this.bankAccounts = bankAccounts;
        setTimeout(() => {
          this.form.patchValue(this.form.getRawValue());
        }, 3000)

        this.isLoading = false;
      },)
    this.initialize()
  }

  initialize(params?: any) {
    this.accountManagerService.init().catch(error => {
      console.error('Error initializing account data:', error);
    });
    this.updateDataAutoComplete()
    this.form.controls['creditAccountHeadId'].disable({emitEvent: false});
    this.form.controls['creditReferenceGroupId'].disable({emitEvent: false});
    this.form.controls['creditReferenceId'].disable({emitEvent: false});

    this.form?.controls['payTypeId']?.valueChanges.subscribe((pt: any) => {
      this.resetAllFiltersCredit(true);
      this.filterChequeTypes();
      if (pt !== this.payTypesEnum.Id.Cash) {

        this.form.controls['creditAccountHeadId'].disable({emitEvent: false});
        this.form.controls['creditReferenceGroupId'].disable({emitEvent: false});
        this.form.controls['creditReferenceId'].disable({emitEvent: false});
      }


    });

    this.form?.controls['monetarySystemId']?.valueChanges.subscribe(() => {
      this.filterChequeTypes();
    });


  }

  handleRowChange(e: MatSlideToggleChange) {

    this.form.controls['amount'].setValue(null, {emitEvent: false});
    this.form.controls['currencyRate'].setValue(null, {emitEvent: false});
    if (this.isCalculateByCurrency) {
      this.form?.controls['currencyRate'].enable({emitEvent: false});
      this.form?.controls['amount'].disable({emitEvent: false});
    } else {
      this.form?.controls['currencyRate'].disable({emitEvent: false});
      this.form?.controls['amount'].enable({emitEvent: false});
    }

    this.setupAutoCalculation();
  }

  setupAutoCalculation() {
    if (this.isCalculateByCurrency) {
      this.form?.controls['currencyRate'].enable({emitEvent: false});
      this.form?.controls['amount'].disable({emitEvent: false});
    } else {
      this.form?.controls['currencyRate'].disable({emitEvent: false});
      this.form?.controls['amount'].enable({emitEvent: false});
    }

    const controlsToWatch = this.isCalculateByCurrency
      ? ['currencyAmount', 'currencyRate']
      : ['currencyAmount', 'amount'];

    controlsToWatch.forEach(controlName => {
      this.form?.controls[controlName].valueChanges.subscribe(() => {
        if (this.isUpdatingCurrency) return;
        this.calculateThirdField();
      });
    });
  }

  resetField(fieldName: string, cal: boolean = false) {
    this.form?.controls[fieldName].reset(null);
  }

  calculateThirdField() {
    if (this.isUpdatingCurrency) return;
    this.isUpdatingCurrency = true;
    const cAmount = this.parseNumber(this.form.controls['currencyAmount'].value);
    const cRate = this.parseNumber(this.form.controls['currencyRate'].value);
    const amount = this.parseNumber(this.form.controls['amount'].value);

    if (this.isCalculateByCurrency && cRate && cAmount) {
      const amount = this.customDecimal.transform(cRate * cAmount);
      this.form.controls['amount'].setValue(amount, {emitEvent: false});

    } else if (!this.isCalculateByCurrency && amount && cAmount) {
      const rate = this.customDecimal.transform(Number((amount / cAmount).toFixed(3)));
      this.form.controls['currencyRate'].setValue(rate, {emitEvent: false});
    }
    this.isUpdatingCurrency = false;
  }

  private parseNumber(value: any): number {
    if (value === null || value === undefined || value === '') return NaN;
    return Number(value.toString().replace(/,/g, ''));
  }

  chequeBookSheetDisplayFn(chequeBookSheetId: number): string {
    const chequeBookSheet = this.chequeBookSheet.find(x => x.id === chequeBookSheetId);
    if (!chequeBookSheet) {
      return '';
    }
    return `ش چک : ${chequeBookSheet.chequeSheetNo} `
    // + `  به ش صیادی : ${chequeBookSheet.sayyadNo}  به مبلغ : ${this.customDecimalPipe.transform(
    //    + chequeBookSheet.amount, 'default',)}`;
  }

  async getChequeBooksSheet(id?: number): Promise<ChequeBooksSheet[]> {
    const searchQueries: SearchQuery[] = [
      new SearchQuery({
        propertyName: 'chequeBookId',
        values: [id],
        comparison: 'equal',
        nextOperand: 'and'
      })
    ];

    const request = new PayablesChequeBooksSheetsQuery(0, 0);

    try {
      return await this._mediator.send(request);
    } catch (error) {
      console.error('خطا در دریافت برگ چک‌ها:', error);
      return [];
    }
  }

  bankAccountsDisplayFn(id: number): string {
    const bankAccount: BankAccounts = this.bankAccounts.find(x => x.id === id) as BankAccounts;
    if (!bankAccount) {
      return '';
    }
    this.form.controls['creditAccountHeadId'].setValue(bankAccount.accountHeadId, {emitEvent: false});
    this.form.controls['creditReferenceGroupId'].setValue(bankAccount.accountReferencesGroupId, {emitEvent: false});
    this.form.controls['creditReferenceId'].setValue(bankAccount.referenceId, {emitEvent: false});

    return `بانک : ${bankAccount.title ?? ('[' + bankAccount.bankTitle + ',' + bankAccount.bankBranchTitle + ',' + bankAccount.bankBranchTitle + ',' + bankAccount.accountNumber + ']')} `
    // + `  به ش صیادی : ${chequeBookSheet.sayyadNo}  به مبلغ : ${this.customDecimalPipe.transform(
    //    + chequeBookSheet.amount, 'default',)}`;
  }

  async getBankAccounts(id?: number): Promise<BankAccounts[]> {
    const searchQueries: SearchQuery[] = [
      new SearchQuery({
        propertyName: 'id',
        values: [id],
        comparison: 'equal',
        nextOperand: 'and'
      })
    ];

    const request = new GetBankAccounts(0, 0);

    try {
      return await this._mediator.send(request).then((res: any): BankAccounts[] => {
        return res.objResult.data;
      });
    } catch (error) {
      console.error('خطا در دریافت حساب های بانکی:', error);
      return [];
    }
  }

  filterChequeTypes(): void {
    const payTypeId = this.form?.controls['payTypeId'].value;
    const monetarySystemId = this.form?.controls['monetarySystemId']?.value;
    if (monetarySystemId == this.monetarySystemEnum.Rial) {
      this.form.controls['currencyTypeBaseId'].setValue(28306, {emitEvent: false});
      this.setupAutoCalculation()
    }
    if (payTypeId == this.payTypesEnum.Id.Cheque) {
      if (monetarySystemId == this.monetarySystemEnum.Currency) {
        this.filteredChequeTypes = this.chequeTypes.filter((ct: any) =>
          ct.title.includes('ارزی')
        );
        this.form.controls['showDebit'].setValue(true, {emitEvent: false});
        this.form.controls['showCredit'].setValue(true, {emitEvent: false});
      } else {
        this.filteredChequeTypes = this.chequeTypes.filter((ct: any) =>
          !ct.title.includes('ارزی')
        );
        this.form.controls['showDebit'].setValue(null, {emitEvent: false});
        this.form.controls['showCredit'].setValue(null, {emitEvent: false});
      }
    }
  }

  private updateDataAutoComplete() {
    this.accountHeadsCredit$ = new BehaviorSubject([...this.accountManagerService.accountHeads.value]);
    this.accountReferenceGroupsCredit$ = new BehaviorSubject([...this.accountManagerService.accountReferenceGroups.value]);
    this.accountReferencesCredit$ = new BehaviorSubject([...this.accountManagerService.accountReferences.value]);
  }

  trackById(index: number, item: any) {
    return item.id;
  }

  showCurrencyRelatedFields() {

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

  fillDescription() {


  }

  changeMonetarySystem() {
    if (this.form.controls['monetarySystemId'].value == this.monetarySystemEnum.Rial) {
      this.form.controls['currencyTypeBaseId'].setValue(28306, {emitEvent: false});
    }

  }

  changePayType() {


  }


  ChangeChequeType() {

  }


  handleAccountFieldChange(changedField?: string, value?: any) {
    const row: { accountHeadId: number, accountReferencesGroupId: number, referenceId: number } =
      {
        accountHeadId: this.form?.controls['creditAccountHeadId'].value,
        accountReferencesGroupId: this.form?.controls['creditReferenceGroupId'].value,
        referenceId: this.form?.controls['creditReferenceId'].value,
      }
    const accountHead = this.accountManagerService.accountHeads.value.find(x => x.id === row.accountHeadId) ?? null;
    const accountReference = this.accountManagerService.accountReferences.value.find(x => x.id === row.referenceId) ?? null;
    const accountReferencesGroup = this.accountManagerService.accountReferenceGroups.value.find(x => x.id === row.accountReferencesGroupId) ?? null;
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

  }

  private handleAllSelectedState(row: any) {
    const accountHead = this.accountManagerService.accountHeads.value.filter(x => x.id === row.accountHeadId) ?? [];
    const accountReference = this.accountManagerService.accountReferences.value.filter(x => x.id === row.referenceId) ?? [];
    const accountReferencesGroup = this.accountManagerService.accountReferenceGroups.value.filter(x => x.id === row.accountReferencesGroupId) ?? [];
    this.accountReferenceGroupsCredit$.next(accountReferencesGroup);
    this.accountReferencesCredit$.next(accountReference);
    this.accountHeadsCredit$.next(accountHead);
    this.disableCreditControlsBasedOnData()
  }

  private handleNoneSelectedState(row: any) {
    this.resetAllFiltersCredit(true)
  }

  disableCreditControlsBasedOnData(): void {
    this.controlsCredit.forEach(controlName => {
      const control = this.form?.get(controlName);

      if (!control) return;

      const hasAccountHead = this.accountHeadsCredit$.value.length > 0;
      const hasReferenceGroup = this.accountReferenceGroupsCredit$.value.length > 0;
      const hasReference = this.accountReferencesCredit$.value.length > 0;

      switch (controlName) {
        case 'creditAccountHeadId':
          if (!hasAccountHead) {
            control.disable({emitEvent: false});
          } else {
            control.enable({emitEvent: false});
          }
          break;
        case 'creditReferenceGroupId':
          if (!hasReferenceGroup) {
            control.disable({emitEvent: false});
          } else {
            control.enable({emitEvent: false});
          }
          break;
        case 'creditReferenceId':
          if (!hasReference) {
            control.disable({emitEvent: false});
          } else {
            control.enable({emitEvent: false});
          }
          break;
        default:
          break;
      }
    });
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
      this.accountReferenceGroupsCredit$.next(relatedGroups);
      if (relatedGroups.length === 1) {
        if (row.accountReferencesGroupId !== relatedGroups[0].id) {
          this.form.controls['creditReferenceGroupId'].setValue(relatedGroups[0].id, {emitEvent: false});
          this.handleAccountFieldChange();
        }
      }
      this.form.controls['creditReferenceGroupId'].enable({emitEvent: false})
      return;
    }
    if (accountHead && !accountReference && !accountReferencesGroup) {
      const groups = this.accountManagerService.getGroupsRelatedToAccountHead(+accountHead.id);
      this.accountReferenceGroupsCredit$.next(groups);
      if (groups.length > 0) {
        this.form.controls['creditReferenceGroupId'].enable({emitEvent: false});
        this.form.controls['creditReferenceId'].enable({emitEvent: false});
      } else {
        this.form.controls['creditReferenceGroupId'].disable({emitEvent: false});
        this.form.controls['creditReferenceId'].disable({emitEvent: false});
      }
      return;
    }

    if (accountHead && accountReferencesGroup && !accountReference) {
      const refs = this.accountManagerService.getAccountReferencesRelatedToGroup(accountReferencesGroup.id)
        .filter(x => x.isActive);
      this.accountReferencesCredit$.next(refs);
      if (refs.length > 0) {
        this.form.controls['creditReferenceId'].enable({emitEvent: false});
      } else {
        this.form.controls['creditReferenceId'].disable({emitEvent: false});
      }

      return;
    }

    if (accountReference && !accountHead && !accountReferencesGroup) {
      const groups = this.accountManagerService.getGroupsRelatedToAccountReference(accountReference.id);
      this.accountReferenceGroupsCredit$.next(groups);
      if (groups.length > 0) {
        this.form.controls['creditReferenceGroupId'].enable({emitEvent: false});
        this.form.controls['creditAccountHeadId'].disable({emitEvent: false});
      } else {
        this.form.controls['creditReferenceGroupId'].disable({emitEvent: false});
        this.form.controls['creditAccountHeadId'].disable({emitEvent: false});

      }
      return;
    }

    if (accountReference && accountReferencesGroup && !accountHead) {
      const heads = this.accountManagerService.getAccountHeadsRelatedToGroup(accountReferencesGroup.id).filter(x => x.lastLevel);
      if (heads.length === 0) {
        this.toastr.showToast({title: "خطا", message: "هیچ سرفصل حسابی مرتبط با این گروه یافت نشد.", type: "error"});
        return;
      }
      this.accountHeadsCredit$.next(heads);
      this.form.controls['creditAccountHeadId'].enable({emitEvent: false});
      return;
    }
    console.warn('Unexpected state combination:', {accountHead, accountReference, accountReferencesGroup});
  }


  private resetAllFiltersCredit(restValue: boolean = false) {
    this.accountHeadsCredit$.next(this.accountManagerService.accountHeads.value.filter(x => x.lastLevel));
    this.accountReferenceGroupsCredit$.next(this.accountManagerService.accountReferenceGroups.value);
    this.accountReferencesCredit$.next(this.accountManagerService.accountReferences.value.filter(x => x.isActive));
    this.form.controls['creditAccountHeadId'].enable({emitEvent: false});
    this.form.controls['creditReferenceGroupId'].disable({emitEvent: false});
    this.form.controls['creditReferenceId'].enable({emitEvent: false});
    if (restValue) {
      this.form.controls['creditAccountHeadId'].setValue(null, {emitEvent: false});
      this.form.controls['creditReferenceGroupId'].setValue(null, {emitEvent: false});
      this.form.controls['creditReferenceId'].setValue(null, {emitEvent: false});
    }
  }

  ngOnDestroy() {
    this.accountHeadsCredit$.complete();
    this.accountReferenceGroupsCredit$.complete();
    this.accountReferencesCredit$.complete();
  }


}

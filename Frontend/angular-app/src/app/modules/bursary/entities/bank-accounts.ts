export class BankAccounts {

  id!: number;
  parentId!: number;
  bankBranchId!: number;
  title!: string;
  sheba!: string;
  accountNumber!: string;
  bankTitle!: string;
  accountTypeBaseTitle!: string;
  bankBranchTitle!: string;
  subsidiaryCodeId!: number;
  relatedBankAccountId!: number;
  accountTypeBaseId!: number;
  accountReferenceId!: number;
  referenceId!: number;
  accountStatus!: number;
  withdrawalLimit!: number;
  haveChekBook!: boolean;
  currenceTypeBaseId!: number;
  signersJson!: string;
  accountHeadId!: number;
  accountReferencesGroupId!: number | null;
  accountTypeBase!: any | null;
  bankBranch!: any | null;
  createdBy!: any | null;
  modifiedBy!: any | null;
  ownerRole!: any | null;
  parent!: any | null;
  bankAccountCardexes!: any[];
  inverseParent!: any[];
  payCheques!: any[];
  payables_ChequeBooks!: any[];
  payables_Documents!: any[];
  isDeleted!: boolean;
}

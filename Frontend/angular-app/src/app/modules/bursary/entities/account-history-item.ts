export interface AccountHistoryItem {
    transactionType: string;
    operationCode: string;
    origKey: string;
    transactionUniqueId: string;
    transactionAmount: number | null;
    transactionAmountDebit: number | null;
    transactionAmountCredit: number | null;
    transactionDate: number | null;
    transactionTime: string;
    effectiveDate: number | null;
    docNumber: number | null;
    description: string;
    transactionRow: string;
    balance: string;
    payId1: string;
    payId2: string;
    codeDigit: string;
    branchCode: number | null;
    BankName: string;
    dateAndTime: string;
    transactionTypeTitle:string;
    accountReferenceTitle:string;
    persianDateAndTime:string;
}
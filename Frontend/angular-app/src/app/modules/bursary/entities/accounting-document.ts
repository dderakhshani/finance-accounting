export class AccountingDocument {

  DocumentNo : string | undefined = undefined
  DocumentId : number | undefined = undefined
  DocumentDate : Date | undefined = undefined
  CodeVoucherGroupId : number | undefined = undefined
  DebitAccountHeadId : number | undefined = undefined
  DebitAccountReferenceGroupId : number | undefined = undefined
  DebitAccountReferenceId : number | undefined = undefined
  CreditAccountHeadId : number | undefined = undefined
  CreditAccountReferenceGroupId : number | undefined = undefined
  CreditAccountReferenceId : number | undefined = undefined
  Amount  : number | undefined = undefined
  DocumentTypeBaseId : number | undefined = undefined
  SheetUniqueNumber : string | undefined = undefined
  CurrencyFee:number|undefined = undefined;
  CurrencyAmount : number| undefined = undefined;
  CurrencyTypeBaseId : number | undefined = undefined;
  NonRialStatus : number | undefined = undefined;
  ChequeSheetId : number | undefined = undefined;
  IsRial : boolean | undefined = undefined;
  ReferenceName : string | undefined = undefined;
  ReferenceCode : string | undefined = undefined;
  Description : string | undefined = undefined;
  BesCurrencyStatus : boolean | undefined = undefined;
  BedCurrencyStatus : boolean | undefined = undefined;
}

import { ChequeSheet } from "./cheque-sheet";

export class FinancialRequestDetail {

    documentTypeBaseId  : number | undefined = undefined ;
    financialReferenceTypeBaseId   : number | undefined = undefined;
    financialReferenceTypeBaseTitle   : string | undefined = undefined;

    description   : string | undefined = undefined;

    debitAccountHeadId   : number | undefined = undefined
    debitAccountReferenceGroupId   : number | undefined = undefined
    debitAccountReferenceId   : number | undefined = undefined

    debitAccountHeadTitle   : string | undefined = undefined
    debitAccountReferenceGroupTitle   : string | undefined = undefined
    debitAccountReferenceTitle   : string | undefined = undefined
    debitAccountReferenceCode   : string | undefined = undefined

    creditAccountHeadId   : number | undefined = undefined
    creditAccountReferenceGroupId   : number | undefined = undefined
    creditAccountReferenceId   : number | undefined = undefined

    creditAccountHeadTitle   : string | undefined = undefined
    creditAccountReferenceGroupTitle   : string | undefined = undefined
    creditAccountReferenceTitle   : string | undefined = undefined
    creditAccountReferenceCode  : string | undefined = undefined

    currencyTypeBaseId   : number | undefined = undefined
    currencyTypeBaseTitle   : string | undefined = undefined

    chequeSheetId   : number | undefined = undefined
    amount   : number | undefined = undefined
    sowiftCode   : number | undefined = undefined
    deliveryOrderCode   : number | undefined = undefined
    registrationCode   : number | undefined = undefined
    paymentCode   : number | undefined = undefined
    isRial   : boolean | undefined = undefined
    nonRialStatus   : number | undefined = undefined
    chequeSheet   : ChequeSheet | undefined = undefined
    sheetSeqNumber :string|undefined = undefined
    modifyName:string | undefined = undefined
    createName:string|undefined = undefined
}

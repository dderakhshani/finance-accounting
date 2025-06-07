import { CreateChequeSheetCommand } from "../repositories/financial-request/receipt-cheque/commands/create-cheque-sheet-command";
import { ChequeSheet } from "./cheque-sheet";

export class FinancialPaymentPartial {

financialRequestId : number | undefined = undefined;
creditAccountHeadId : number | undefined = undefined;
creditAccountReferencesGroupId : number | undefined = undefined;
creditReferenceId : number | undefined = undefined;
debitAccountHeadId : number | undefined = undefined;
debitAccountReferencesGroupId : number | undefined = undefined;
debitReferenceId : number | undefined = undefined;
amount : number | undefined = undefined;
requestDate : Date | undefined = undefined;
issueDate : Date | undefined = undefined;
realDate : Date | undefined = undefined;
isDeleted : boolean | undefined = undefined;
chequeSheetId : number | undefined = undefined;
trackingCode : string | undefined = undefined;
isAccumulativeSelectStatus : boolean | undefined = undefined;
orderIndex : number | undefined = undefined;
chequeSheet: ChequeSheet | undefined = undefined;
}

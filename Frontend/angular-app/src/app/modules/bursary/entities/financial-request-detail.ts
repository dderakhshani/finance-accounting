export class FinancialRequestDetail {
  id!: number;
  financialRequestId!: number;
  documentTypeBaseId!: number;
  creditAccountHeadId!: number;
  creditAccountReferenceGroupId!: number;
  creditAccountReferenceId!: number;
  debitAccountHeadId!: number;
  debitAccountReferenceGroupId!: number;
  debitAccountReferenceId!: number;
  currencyTypeBaseId!: number;
  currencyFee!: number;
  currencyAmount!: number;
  chequeSheetId!: number;
  amount!: number;
  description!: string;
  financialReferenceTypeBaseId!: number;
  registrationCode!: string;
  paymentCode!: string;
  isRial!: boolean;
  nonRialStatus!: number;
}

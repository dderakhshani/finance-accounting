export class PayableDocument {
  state: number | undefined = undefined;
  payTypeId: number | undefined = undefined;
  chequeTypeId: number | undefined = undefined;
  chequeBookSheetId: number | undefined = undefined;
  bankAccountId: number | undefined = undefined;
  monetarySystemId: number | undefined = undefined;
  creditAccountHeadId: number | undefined = undefined;
  creditReferenceGroupId: number | undefined = undefined;
  creditReferenceId: number | undefined = undefined;
  currencyTypeBaseId: number | undefined = undefined;
  dueDate: string | undefined = undefined;
  draftDate: string | undefined = undefined;
  documentDate: string | undefined = undefined;
  documentNo: string | undefined = undefined;
  subjectId: number | undefined = undefined;
  currencyRate: number | undefined = undefined;
  currencyAmount: number | undefined = undefined;
  referenceNumber: number | undefined = undefined;
  trackingNumber: number | undefined = undefined;
  amount: number | undefined = undefined;
  descp: string | undefined = undefined;
  showCredit:boolean | undefined = undefined;
  showDebit:boolean | undefined = undefined;
  accounts: PayableDocumentAccount[] = [];
  payOrders: PayableDocumentPayOrder[] = [];
}

export class PayableDocumentAccount {
  id: number | undefined = undefined;
  documentId: number | undefined = undefined;
  referenceId: number | undefined = undefined;
  referenceGroupId: number | undefined = undefined;
  accountHeadId: number | undefined = undefined;
  rexpId: number | undefined = undefined;
  descp: string | undefined = undefined;
  amount: number | undefined = undefined;
  isEditableAccountHeads: boolean | undefined = undefined;
  isEditableAccountReferences: boolean | undefined = undefined;
  isEditableAccountReferenceGroups: boolean | undefined = undefined;
  isEditableDebit: boolean | undefined = undefined;
  isEditableRow: boolean | undefined = undefined;
  selected: boolean | undefined = undefined;
  rowIndex: number | undefined = undefined;
}

export class PayableDocumentPayOrder {
  payOrderId: number | undefined = undefined;
}

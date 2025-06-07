export class ChequeSheet {
  public id: number | undefined = undefined
  public payChequeId: number | undefined = undefined
  public sheetSeqNumber: number | undefined = undefined
  public sheetUniqueNumber: number | undefined = undefined
  public sheetSeriNumber: number | undefined = undefined
  public totalCost!: number
  public accountReferenceId: number | undefined = undefined
  public bankBranchId: number | undefined = undefined
  public issueDate: Date | undefined = undefined
  public receiptDate: Date | undefined = undefined
  public issuerEmployeeId: number | undefined = undefined
  public description: string | undefined = undefined
  public isActive: number | undefined = undefined
  public bankId: number | undefined = undefined
  public accountNumber: string | undefined = undefined
  public branchName: string | undefined = undefined
  public title: string | undefined = undefined
  public chequeTypeBaseId: number | undefined = undefined
  public chequeSheetId: number | undefined = undefined

  public creditAccountReferenceTitle: string | undefined = undefined
  public creditAccountReferenceCode: string | undefined = undefined
  public debitAccountReferenceTitle: string | undefined = undefined
  public debitAccountReferenceCode: string | undefined = undefined
  public creditAccountReferenceGroupTitle: string | undefined = undefined
  public createName: string | undefined = undefined

  public creditAccountHeadId: number | undefined = undefined
  public creditAccountReferenceGroupId: number | undefined = undefined
  public creditAccountReferenceId: number | undefined = undefined
  public debitAccountHeadId: number | undefined = undefined
  public debitAccountReferenceGroupId: number | undefined = undefined
  public debitAccountReferenceId: number | undefined = undefined
  public financialRequestDescription: string | undefined = undefined
  public chequeDocumentStateTitle: string | undefined = undefined
  public chequeDocumentState: number | undefined = undefined

  public financialRequestId : number | undefined = undefined

  public issueReferenceBankTitle : string | undefined = undefined
  public issueReferenceBankId : number | undefined = undefined

  public chequeTypeTitle : string | undefined = undefined

  public voucherHeadId :number|undefined = undefined


}

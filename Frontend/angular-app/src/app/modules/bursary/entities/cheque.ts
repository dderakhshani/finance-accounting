export class Cheque {
  public id: number | undefined = undefined;
  public sheba: string | undefined = undefined;
  public bankBranchId: number | undefined = undefined;
  public accountNumber: string | undefined = undefined;
  public sheetsCount: number | undefined = undefined;
  public chequeNumberIdentification: string | undefined = undefined;
  public ownerEmployeeId: number | undefined = undefined;
  public setOwnerTime: Date | undefined = undefined;
  public isFinished: boolean | undefined = undefined;
  public bankAccountId: number | undefined = undefined;
}


// for prototype ...
export class ChequeModel {
  public amount: number | undefined = undefined;
  public number: number | undefined = undefined;
  public dueDate: string | undefined = undefined;
  public accountNumber: string | undefined = undefined;
  public bankName: number | undefined = undefined;
  public branchBank: number | undefined = undefined;
  public branchCode: number | undefined = undefined;
  public chequeType: number | undefined = undefined;
  public chequeSerial: number | undefined = undefined;

}

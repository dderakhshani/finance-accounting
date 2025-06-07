export class AccountHead {
  public id!:number;
  public title!:string;
  public parentId!:number;
  public levelCode!:string;
  public companyId!:number;
  public codeLevel!: number;
  public code!:string;
  public fullCode!:string;
  public codeLength!:number;
  public balanceId!:number;
  public balanceName!:string;
  public balanceBaseId!:number;
  public balanceBaseTitle!:string;
  public transferId!:number;
  public transferName!:string;
  public groupId!:number;
  public currencyBaseTypeId!:number;
  public currencyBaseTypeTitle!:string;
  public lastLevel:boolean = false;
  public currencyFlag:boolean = false;
  public exchengeFlag:boolean = false;
  public traceFlag:boolean = false;
  public quantityFlag:boolean = false;
  public isActive:boolean = false;
  public description!:string;
  public groupTitle!:string;
  public accountReferenceGroupsIds: number[] = []

  //public accountHeadRelReferenceGroups:AccountHeadRelReferencesGroup[] = []
}

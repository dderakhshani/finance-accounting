export class AccountReference {
  public id !: number;
  public accountReferenceGroupId!: number;
  public title!: string;
  public code!: string;
  public description!: string;
  public isActive!: boolean;
  public status!: string;
  public nationalNumber!: string;
  public firstName!: string;
  public lastName!: string;
  public personId!: number;
  public personPhotoUrl!: string;
  public accountReferencesGroupsIdList!: number[]
  public personalGroupId!: number;
  public personalGroupAccountHeadIds!: number[];
  public createdAt!: Date;
  public modifiedAt!: Date;
  public employeeCode!: string;
  public employeeTitle!: string;
  public depositId!:string;
}


export class Role {
  public id!:number;
  public title!:string;
  public description!:string;
  public uniqueName!:string;
  public levelCode!:string;
  public parentId!:number;
  public copy !: string;
  public permissionsId: number[] = [];
}

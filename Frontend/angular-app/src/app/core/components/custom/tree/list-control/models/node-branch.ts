export class NodeBranch {
  public id!:any;
  public parentId!:any;
  public title!:string;
  public children!:NodeBranch[];
  public _toggled!:boolean;
}

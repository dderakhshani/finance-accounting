export class Node {
  public id!:any;
  public parentId!:number;
  public children:Node[] = [];
  public title!:string;
  public code!:string;
  public value!:any;
  public level!:string;
  public hasChild:boolean = false;
  public _expanded:boolean = false;
  public _selected:boolean = false;

}

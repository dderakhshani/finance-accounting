import {BomItem} from "./bom-Item";

export default class Bom {
  public title!:string;
  public id!:number;
  public parentId!:number;
  public rootId!:number;
  public description!:string;
  public isActive!:boolean;
  public levelCode!:string;
  public commodityCategoryId!: number;
  public commodityCategoryTitle!: string;
  public items!:BomItem[];
}

import {CommodityCategoryProperty} from "./commodity-category-property";

export class CommodityCategory {
  public id!: number;
  public parentId!: number;
  public levelCode!: string;
  public code!: string;
  public codingMode!: number;
  public uniqueName!: string;
  public title!: string;
  public measureId!: number;
  public orderIndex!: number;
  public isReadOnly!: boolean;
  public requireParentProduct!: boolean;
  public categoryProperties: CommodityCategoryProperty[] = []
  public children: CommodityCategory[] = []
}









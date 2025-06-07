import {CommodityCategoryPropertyItem} from "./commodity-category-property-item";

export class CommodityCategoryProperty {
  id!: number;
  parentId!: number;
  categoryId!: number;
  levelCode!: string;
  uniqueName!: string;
  title!: string;
  measureId!: number;
  propertyTypeBaseId!: number;
  orderIndex!: number;
  measureTitle!: string;
  propertyTypeBaseTitle!: string;
  value!: string;
  valuePropertyItemId!: string;
  items: CommodityCategoryPropertyItem[] = []
}

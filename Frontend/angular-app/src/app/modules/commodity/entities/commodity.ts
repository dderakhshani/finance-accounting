import {CommodityPropertyValue} from "./commodity-property-value";

export class Commodity {
  public id!:number;
  public parentId!:number;
  public commodityCategoryId!:number;
  public commodityCategoryTitle!:string;
  public levelCode!:string;
  public code!:string;
  public tadbirCode!:string;
  public compactCode!:string;
  public title!:string;
  public descriptions!:string;
  public measureId!:number;
  public minimumQuantity!:number;
  public maximumQuantity!:number;
  public orderQuantity!:number;
  public pricingTypeBaseId!: number;
  public commodityNationalTitle!: string;
  public isConsumable!: boolean;
  public isHaveWast!: boolean;
  public isAsset!: boolean;
  public measureTitle!: string;
  public isWrongMeasure!: boolean;
  public bomsCount!: number;
  public isHaveForceWast!: boolean;
  public isActive!: boolean;
  public propertyValues:CommodityPropertyValue[] = []
}



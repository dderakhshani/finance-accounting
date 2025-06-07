
export class WarehouseLayout {
  public id: number | undefined = undefined;
  public warehouseId: number | undefined = undefined;
  public parentId: number | undefined = undefined;
  public levelCode: string | undefined = undefined;
  public lastLevel: boolean | undefined = undefined;
  public title!: string;
 
  public unitBaseTypeId: number | undefined = undefined;
  public orderIndex: number | undefined = undefined;
  public entryMode: number | undefined = undefined;
  public childCount: number | undefined = undefined;
  public status: number | undefined = undefined;
  public capacity: number | undefined = undefined;
  public capacityAvailable: number | undefined = undefined;
  public capacityUsed: number | undefined = undefined;
  public capacityUsedPercent: number | undefined = undefined;
  public capacityNeed: number | undefined = undefined;
  public allowInput: boolean | undefined = undefined;
  public isDefault: boolean | undefined = undefined;
  public allowOutput: boolean | undefined = undefined;
  public commodityId: number | undefined = undefined;
  public commodityTitle: string | undefined = undefined;
  
  public parentNameString = [];
  public Items: WarhousteCategoryItems[] = [];
  public warehouseLayoutProperties: WarehouseLayoutProperty[] = [];
  public categoreis: WarehouseLayoutCategory[] = [];

}
export class WarhousteCategoryItems {
  public warehouseLayoutId: number | undefined = undefined;
  public commodityCategoryId: number | undefined = undefined;
  public categoryPropertyId: number | undefined ;
  public categoryPropertyItemId: number | undefined = undefined;
  public warehouseLayoutPropertiesId: number | undefined = undefined;
  public ValueItem: number | undefined = undefined;


}

export class _WarhousteCategoryItems {
  public commodityCategoryId: number | undefined = undefined;
  public categoryPropertyId: number  | undefined = undefined;
  public categoryPropertyItemId: number | undefined = undefined;
  public ValueItem: number | undefined = undefined;
  public disable:boolean=false;
  public valid:boolean=false;
  public Items:_WarhousteCategoryItems[]=[];
}
export class _WarhousteCategoryItemsEdit{

  public commodityCategoryId: number | undefined = undefined;
  public categoryPropertyId: number | undefined ;
  public categoryPropertyItemId: number | undefined = undefined;
  public ValueItem: number | undefined = undefined;
  public disable:boolean=false;
  public valid:boolean=false;
  public warehouseLayoutCategoriesId: number | undefined = undefined;
  public warehouseLayoutPropertiesId: number | undefined = undefined;
  public items:_WarhousteCategoryItemsEdit[]=[];

}


export class WarehouseLayoutProperty {
  id: number | undefined = undefined;
  warehouseLayoutId: number | undefined = undefined;
  categoryPropertyId: number | undefined = undefined;
  categoryPropertyTitle: string | undefined = undefined;
  categoryPropertyItemId: number | undefined = undefined;
  categoryPropertyItemTitle: string | undefined = undefined;
  categoryId: number | undefined = undefined;
}
export class WarehouseLayoutCategory {
  warehouseLayoutId: number | undefined = undefined;
  categoryId: number | undefined = undefined;
  categoryTitle: string | undefined = undefined;
  id: number | undefined = undefined;
}

export interface WarehouseLayoutTree {
  id: number;
  warehouseId: number;
  parentId: number;
  levelCode: string;
  lastlevel: boolean;
  title: string;
  capacity: number;
  unitBaseTypeId: number;
  orderIndex: number;
  entryMode: number;
  children ?: WarehouseLayoutTree[],
}

export interface CommodityBoms {
  bomsId: number;
  bomsHeaderId: number;
  bomWarehouseId: number;
  rootId: number | null;
  levelCode: string;
  commodityCategoryId: number | null;
  title: string;
  name: string;
  isActive: boolean | null;
  commodityId: number | null;
  bomDate: string;
}
export interface DocumentItemsBom {
  id: number | undefined ,
  commodityId: number | undefined,
  commodityTitle: string | undefined,
  commodityCode: string | undefined,
  mainMeasureId: number | undefined,
  measureTitle: string | undefined,
  unitPrice: number | undefined,
  productionCost: number | undefined,
  quantity: number,
  documentItemId: number | undefined,
}

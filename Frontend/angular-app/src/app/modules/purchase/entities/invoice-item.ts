import { Commodity } from "../../accounting/entities/commodity";



export interface InvoiceItem {
  id: number;
  documentHeadId: number;
  documentItemId: number | null;
  yearId: number;
  commodityId: number;
  commoditySerial: string | null;
  unitPrice: number;
  unitBasePrice: number;
  totalPrice: number;
  productionCost: number;
  weight: number;
  quantity: number;
  quantityChose: number | null;
  quantityUsed: number | null;
  secondaryQuantity: number | null;//تعداد فرعی بر اساس واحد ورودی در انبار
  currencyBaseId: number | null;
  currencyBaseTitle: string;
  documentMeasureId: number;
  documentMeasureTitle: string;
  description: string;
  measureUnitConversionId: number;
  mainMeasureId: number;
  conversionRatio: number;
  currencyPrice: number | null;
  discount: number;
  documentNo: number | null;
  requestNo: number | null;
  isWrongMeasure: number | null;
 
  commodity: Commodity
  warehouseLayoutQuantity: WarehouseLayoutQuantity;
  warehouseHistories: WarehouseHistory[];
  measureUnitConversions: measureUnit[];
  commodityMeasureUnits: measureUnit[];
}


export interface WarehouseLayoutQuantity {
  warehouseLayoutId: number;
  id: number | null;
  warehouseLayoutTitle: string;
  commodityId: number;
  quantity: number | null;
  quantityAvailable: number | null;
  quantityTotal: number | null;
  quantityNeed: number | null;
 
  
}
export interface WarehouseHistory {
  id: number | null;
  commodityld: number;
  warehouseLayoutId: number;
  quantity: number;
  mode: number;
  documentItemId: number | null;
  warehouseLayoutTitle: string;
  modeTitle: string;
}
export interface addModelArgs {
  warhouseLayoutId: number,
  quantity: number
}
export interface measureUnit {
  Id: number,
  title: number
}


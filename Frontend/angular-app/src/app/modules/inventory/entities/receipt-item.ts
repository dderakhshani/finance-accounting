import { Commodity } from "../../accounting/entities/commodity";
import { AssetsSerial } from "./Assets";
import { ConsumptionCommodityRequester } from "./consumption-commodity-requester";
import { WarehouseLayoutsCommodity } from "./warehouse-layouts-commodity";




export class ReceiptItem {
  public id: number | undefined;
  public documentHeadId: number | undefined;
  public documentItemId: number | undefined;
  public bomValueHeaderId: number | undefined;
  public yearId: number | undefined;
  public commodityId: number | undefined;
  public commodityCode: string | undefined;
  public commodityTitle: string | undefined;
  public commoditySerial: string  | undefined;
  public unitPrice: number | undefined;
  public unitBasePrice: number | undefined;
  public totalPrice: number | undefined;
  public productionCost: number | undefined;
  public weight: number | undefined;
  public quantity: number | undefined;
  public quantityChose: number  | undefined;
  public quantityUsed: number | undefined;
  public remainQuantity: number | undefined ;
  public secondaryQuantity: number  | undefined;//تعداد فرعی بر اساس واحد ورودی در انبار
  public currencyBaseId: number  | undefined;
  public currencyBaseTitle: string | undefined;
  public documentMeasureId: number | undefined;
  public documentMeasureTitle: string | undefined;
  public measureTitle: string | undefined;
  public description: string | undefined;
  public measureUnitConversionId: number | undefined;
  public mainMeasureId: number | undefined;
  public conversionRatio: number | undefined;
  public currencyPrice: number  | undefined;
  public discount: number | undefined;
  public documentNo: number  | undefined;
  public requestNo: number | undefined;
  public isWrongMeasure: boolean | undefined;
  public selected: boolean | undefined = false;
  public hasPermissionEditQuantity: boolean | undefined;
  
  
  public commodityQuota: number | undefined; 
  public commodityQuotaUsed: number | undefined;

  public assetsSerials: AssetsSerial[] = []
  public commodityMeasureUnits: measureUnit[] = []
  public layouts: WarehouseLayoutsCommodity[] = []
  public measureUnitConversions: measureUnit[] = []
  public warehouseHistories: WarehouseHistory[] = []
  public consumptionCommodity: ConsumptionCommodityRequester[] = []
  
  public commodity: Commodity | undefined = undefined;
  public warehouseLayoutQuantity: WarehouseLayoutQuantity | undefined =undefined;
}


export interface WarehouseLayoutQuantity {
  warehouseLayoutId: number;
  id: number  | undefined;
  warehouseLayoutTitle: string;
  commodityId: number;
  quantity: number  | undefined;
  quantityAvailable: number  | undefined;
  quantityTotal: number  | undefined;
  quantityNeed: number  | undefined;
 
  
}
export interface WarehouseHistory {
  id: number  | undefined;
  commodityld: number;
  warehouseLayoutId: number;
  quantity: number;
  mode: number;
  documentItemId: number  | undefined;
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



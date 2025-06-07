export interface WarehouseStock {
  id: number;
  warehouseId: number;
  warehouseTitle: string;
  commodityId: number;
  commodityTitle: string;
  commodityCode: string;
  commodityTadbirCode: string;
  quantity: number;
  reservedQuantity: number;
  availableQuantity: number;
  price: number;
}

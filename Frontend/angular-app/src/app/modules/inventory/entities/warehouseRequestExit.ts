export interface WarehouseRequestExitView {
  id: number;
  warehouseTitle: string;
  documentNo: number | null;
  requestNo: string;
  documentDate: string | null;
  exitQuantity: number | null;
  itemsQuantity: number | null;
  requestQuantity: number | null;
  remainedQuantity: number | null;
  commodityTitle: string;
  commodityCode: string;
  warehouseLayoutsTitle: string;
  statusTitle: string;
}

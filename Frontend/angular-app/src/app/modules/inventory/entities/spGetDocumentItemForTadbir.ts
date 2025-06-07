
export interface spGetDocumentItemForTadbir {
  documentNo: number | null;
  date: string;
  documentType: string;
  commodityCode: string;
  quantity: number;
  tadbirCode: number | null;
  tahvilCode: string;
  darkhastCode: string;
  shenavarCode: string;
  price: number;
  codeVoucherGroupTitle: string;
  warehouseTitle: string;
}
export interface spReportControlsWarehouseLayoutQuantitiesByTadbir {
  layoutQuantities: number | null;
  tadbirQuantity: number | null;
  price: number | null;
  warehouseLayoutId: number | null;
  commodityId: number | null;
  commodityCode: string;
  warehouseTitle: string;
  warehouseLayoutTitle: string;
  commodityTitle: string;
  createDate: string;

}

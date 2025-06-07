

export interface WarehouseLayoutsCommodity {
  id: number | null;
  warehouseId: number | null;
  warehouseLayoutId: number | null;
  warehousesParentId: number | null;
  commodityId: number | null;
  serialNumber: number | null;
  
  Commodityld: number | null;//در جدول WarehouseHistories اشتباه ثبت شده است
  quantity: number | null;
  quantityChose: number | null;
  warehouseLayoutParentId: number | null;
  warehouseLayoutCapacity: number | null;
  warehouseLayoutAvailableQuantity: number | null;
  warehouseTitle: string;
  warehousesTitle: string;
  warehousesLevelCode: string;
  warehouseLayoutTitle: string;
  warehouseLayoutLevelCode: string;
  commodityCompactCode: string;
  commodityTadbirCode: string;
  commodityTitle: string;
  commodityCode: string;
  measureTitle: string;
  mode: number | null;
  modeTitle: string;
  createdDate: Date
  createdTime: string;
  totalQuantity: number | null;
  allowInput: boolean | null;
  allowOutput: boolean | null;
  disabled: boolean | null;
  documentId: number | null;
  //-------------Document Info
  requestNo: string;
  requestNoPurchaseForExit: string;
  documentNo: string;
  documentDate: Date;
  requestDate: Date;
  documentItemId: number | null;
  creditReferenceTitle: string;
  debitReferenceTitle: string;
  codeVoucherGroupTitle: string;
  requesterReferenceTitle: string;
  creditAccountReferenceId: number
  debitAccountReferenceId: number
  creditAccountHeadId: number
  debitAccountHeadId: number
  codeVoucherGroupId: number;
  invoiceNo: string;
  documentHeadId: number | null;
  itemUnitPrice: number | null;
  totalProductionCost: number | null;
  extraCost: number | null;
  //------------
  quantityInput: number | null;
  quantityOutput: number | null;
  trasctionType: string;

  //-----------------------
  documentStateBaseTitle: string | null;
  documentStateBaseId: number | null;

  selected: boolean| null;
  isRead: boolean | null;
  isDocumentIssuance: boolean | null;
  descriptionItems: string | null;
}
export interface StocksCommoditiesModel {
  warehouseLayoutsCommoditiesModel: WarehouseLayoutsCommodity[];
  firstQuantity: number | null;
  modifyQuantity: number | null;
}

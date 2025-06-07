export interface spCommodityReports {
  prefix_Quantity: number | null;
  prefix_ItemUnitPrice: number | null;
  prefix_TotalItemPrice: number | null;
  current_Enter_Quantity: number | null;
  current_Enter_ItemUnitPrice: number | null;
  current_Enter_TotalItemPrice: number | null;
  current_Exit_Quantity: number | null;
  current_Exit_ItemUnitPrice: number | null;
  current_Exit_TotalItemPrice: number | null;
  postfix_Quantity: number | null;
  postfix_ItemUnitPrice: number | null;
  postfix_TotalItemPrice: number | null;
  commodityCode: string;
  commodityTitle: string;
  warehouseId: number | null;
  commodityId: number | null;
  rowsCount: number | undefined;
  measureTitle: string;
  compactCode: string;
}

export interface spCommodityReportsSumAll {
  prefix_Quantity: number | null;
  prefix_ItemUnitPrice: number | null;
  prefix_TotalItemPrice: number | null;
  current_Enter_Quantity: number | null;
  current_Enter_ItemUnitPrice: number | null;
  current_Enter_TotalItemPrice: number | null;
  current_Exit_Quantity: number | null;
  current_Exit_ItemUnitPrice: number | null;
  current_Exit_TotalItemPrice: number | null;
  postfix_Quantity: number | null;
  postfix_ItemUnitPrice: number | null;
  postfix_TotalItemPrice: number | null;
 
}


export interface spCommodityReportsWithWarehouse {
  warehouseId: number | null;
  warehouseTitle: string;
  prefix_TotalItemPrice: number | null;
  current_Enter_Used_TotalItemPrice: number | null;
  current_Enter_Purchase_TotalItemPrice: number | null;
  current_Exit_TotalItemPrice: number | null;
  postfix_TotalItemPrice: number | null;
  prefix_Quantity: number | null;
  current_Enter_Used_Quantity: number | null;
  current_Enter_Purchase_Quantity: number | null;
  current_Exit_Quantity: number | null;
  postfix_Quantity: number | null;
}

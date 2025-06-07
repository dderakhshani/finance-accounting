import { WarehouseLayoutsCommodity } from "./warehouse-layouts-commodity";


export interface RequestCommodityWarehouse {
  request: RequestResult;
  items: RequestItemCommodity[];
}

export interface RequestResult {
  id: number;
  requesterTitle: string;
  requesterId: string;
  requestDate: string;
  requestDate_Jalali: string;
  statusId: number;
  statusTitle: string;
  warehouseCourierTitle: string;
  sabtShodeTavasoteAnbar: boolean;
  force: boolean;
  requestNo: string;
  documentNo: string;
  documentId: number;
  newSearch: boolean;
  
}

export interface RequestItemCommodity {
  commodityCode: string;
  commodityId: number | null;
  commodityName: string;
  quantity: number;
  quantityExit: number;
  quantityTotal: number;
  measureTitle: string;
  measureId: number | null;
  description: string;
  placeUse: string;
  placeUseDetail: string;
  daghi: boolean;
  descriptionSupervisor: string;
  returnDaghi: boolean;
  inventory: number | null;
  requestItemId: number | null;
  documentItemsId: number | null;
  documentHeadId: number | null;
  layoutTitle: string;
  layouts: WarehouseLayoutsCommodity[];
}


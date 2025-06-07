export interface PersonsDebitedCommodities {
  id: number;
  warehousesTitle: string;
  warehouseId: number;
  assetSerial: string;
  documentDate: string;
  debitReferenceTitle: string;
  price: number;
  depreciationTypeBaseId: number | null;
  assetGroupId: number;
  depreciatedPrice: number;
  unitId: number | null;
  measureId: number | null;
  isActive: boolean;
  commodityId: number;
  commoditySerial: string;
  categoryLevelCode: string;
  categoryTitle: string;
  measureTitle: string;
  searchTerm: string;
  unitsTitle: string;
  depreciationTitle: string;
  assetGroupTitle: string;
  documentNo: number;
  
  documentItemId: number;
  personId: number;
  assetId: number | null;
  debitTypeId: number;
  expierDate: string;
  quantity: number;
  firstName: string;
  lastName: string;
  fatherName: string;
  nationalNumber: string;
  fullName: string;
  accountReferenceId: number;
  commodityTitle: string;
  commodityCode: string;
  isHaveWast: boolean;
  isAsset: boolean;
  isConsumable: boolean;
  documentItemsDescription: string;
  documentDescription: string;
  totalDescription: string;
  
}


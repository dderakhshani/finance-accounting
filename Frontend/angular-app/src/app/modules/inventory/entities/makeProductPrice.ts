export interface MakeProductPrice {
  totalWeight: number | undefined ;
  totalRawMaterial: number | undefined ;
  totalSalary: number | undefined ;
  totalOverload: number | undefined ;
  sumALL: number | undefined ;
  totalMeterage: number | undefined ;
  documentControls160: number | undefined ;
  documentControls296: number | undefined ;
  documentControls295: number | undefined ;
  lastDate: Date ;
  allowAssumeDocument: boolean | undefined;
  voucherNO: number | undefined;
  voucherId: number | undefined;
  makeProductPriceReport: MakeProductPriceReportModel[];
  
}

export interface MakeProductPriceReportModel {
  meterage: number | null;
  weight: number | null;
  thickness: number | null;
  size: string;
  rawMaterial: number;
  salary: number;
  overload: number;
  total: number;
}

export interface MakeProductPriceForDocument {
  DocumentDate: string | null;
  CodeVoucherGroupId: string | null;
  CodeVoucherGroupTitle: string | null;
  DateFrom: string;
  DateTo: string;
  Title: string;
  TotalCost: string;
  TotalSalery: string;
  TotalRawMaterials: string;
  TotalOverhead: string;
}


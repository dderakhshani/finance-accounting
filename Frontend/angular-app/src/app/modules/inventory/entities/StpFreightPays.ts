
export interface StpFreightPays {
  creditAccountReferenceId: number | null;
  creditReferenceTitle: string;
  credit: number | null;
  debit: number | null;
  documentDate: string | null;
  financialOperationNumber: string;
  description: string;
  commodityTitle: string;
}

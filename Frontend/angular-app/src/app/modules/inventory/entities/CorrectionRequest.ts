export interface CorrectionRequest {
  id: number;
  status: number;
  codeVoucherGroupId: number;
  documentId: number | null;
  oldData: string;
  verifierUserId: number;
  payLoad: any;
  apiUrl: string;
  viewUrl: string;
  description: string;
  verifierDescription: string;
  
}

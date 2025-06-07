export interface CorrectionRequestModel {
  id: number;
  status: number;
  statusTitle: string;
  codeVoucherGroupId: number;
  documentId: number | null;
  oldData: string;
  verifierUserId: number;
  payLoad: string;
  apiUrl: string;
  viewUrl: string;
  description: string;
  verifierDescription: string;
  requesterDescription: string;
  createdAt: string;
  modifiedById: number | null;
  modifiedAt: string;
  createdById: number;
  username: string;
}

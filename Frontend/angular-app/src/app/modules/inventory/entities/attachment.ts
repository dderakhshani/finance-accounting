export interface AttachmentAssets {
 
  attachmentId: number | null;
  assetsId: number | null; 
  addressUrl: string | null;
  personsDebitedCommoditiesId: number | null;
}
export interface AttachmentReceipt {
  id: number | null;
  attachmentId: number | null;
  documentHeadId: number | null;
  addressUrl: string | null;
}

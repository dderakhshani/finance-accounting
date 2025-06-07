export interface SpAudit {
  id: number;
  userId: number | null;
  subSystem: string;
  descriptionType: string;
  type: string;
  tableName: string;
  dateTime: string | null;
  createDate: Date | null;
  createDateShamsi: string;
  createTime: string;
  primaryId: number | null;
  new: string;
  title: string;
  old: string;
  description: string;
  summery: string;
  username: string;
}

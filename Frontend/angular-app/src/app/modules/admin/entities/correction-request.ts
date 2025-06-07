export class CorrectionRequest {
  public id!: number;
  public status!: number;
  public codeVoucherGroupId!: number;
  public codeVoucherGroupTitle!: string;
  public documentId!: number;
  public oldData!: string;
  public verifierUserId!: number;
  public createdById!: number;
  public payLoad!: string;
  public apiUrl!: string;
  public viewUrl!: string;
  public description!: string;
  public createdAt!: Date;
  public modifiedAt!: Date;
  public verifierUserName!: string;
  public createdUserName!: string;
}

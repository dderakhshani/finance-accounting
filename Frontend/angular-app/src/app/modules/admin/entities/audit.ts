import {AuditValueChanges} from "./audit-value-changes";

export class Audit {
  public id!: number;
  public userId!: number;
  public subSystem!: null;
  public menueId!: number;
  public type!: string;
  public tableName!: string;
  public transactionId!: string;
  public dateTime!: Date;
  public primaryId!: number;
  changesValuesCollection:AuditValueChanges [] = [];
}


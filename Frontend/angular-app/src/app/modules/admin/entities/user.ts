import {Person} from "./person";
import {Company} from "./company";

export class User {
  public id!:number;
  public personId!:number;
  public username!:string;
  public lastOnlineTime!:string;
  public unitPositionTitle!:string;
  public failedCount!:string;
  public isBlocked!:boolean;
  public blockedReason!:string;
  public blockedReasonBaseId!:number;
  public rolesIdList!:number[];
  public roleTitle!:string;
  public person!:Person;
  public firstName!:string;
  public lastName!:string;
  public fullName!:string;
  public nationalNumber!:string;
  public userYears:number[] = [];
  public userCompanies:number[] = [];
  public companies:Company[] = [];
  private _deleteed: unknown;

}

import {EntityStates} from "../enums/entity-states";

export abstract class AuditableEntity {

  public id:number = 0;
  public ownerRoleId:number = 0;
  public createdById:number = 0;
  public createdAt:Date = new Date(0);
  public modifiedById:number = 0;
  public modifiedAt:Date = new Date(0);
  public isDeleted:boolean = false;
  public entityState:number = EntityStates.Unchanged;

}

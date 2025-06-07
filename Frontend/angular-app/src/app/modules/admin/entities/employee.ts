import {Person} from "./person";

export class Employee {
  public id!:number;
  public personId!:number;
  public nationalNumber!: string;
  public firstName!:string;
  public lastName!:string;
  public accountReferenceCode!: string;
  public branchId!:number;
  public unitId!:number;
  public unitPositionId!:number;
  public employeeCode!:number;
  public employmentDate!:string;
  public leaveDate!:string;
  public person!:Person;
  public legalTitle!:string;
  public governmentalTitle!:string;
}

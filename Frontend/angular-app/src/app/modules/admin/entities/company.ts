import {Year} from "./year";

export class Company {
  public id!:number;
  public title!:string;
  public uniqueName!:string;
  public expireDate!:Date;
  public maxNumOfUsers!:number;
  public logo!:string;
  public yearsId!:number[];
  // public years!:Year[];
}

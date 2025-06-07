import {Node} from "./node";

export class ControllerAction {
  public type!:string;
  public conditionPropertyName?:string;
  public conditionAcceptedIf?:boolean;
  public payload?:Node;
}

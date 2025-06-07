export class ControllerOptions {
  public closeDescendantsOnCollapse:boolean = true;
  public closeSiblingsOnExpand:boolean = true;

  public selectParentsOnSelect:boolean = false;
  public selectParentsIfAllChildsSelected:boolean = false;
  public selectChildrenOnSelect:boolean = false;

  public showChildrenConditionPropertyName?:string;
  public showChildrenConditionAcceptedIf?:boolean;
}

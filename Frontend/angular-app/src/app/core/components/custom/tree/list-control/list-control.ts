import {Node} from "./models/node";
import {ControllerAction} from "./models/controller-action";
import {BehaviorSubject, noop} from "rxjs";
import {ControllerOptions} from "./models/controller-options";
import {NodeBranch} from "./models/node-branch";
import {EventEmitter} from "@angular/core";

export class ListControl {


  public controllerTitle!: string;
  public nodeBranches: NodeBranch[] = [];
  public activeBranch!: NodeBranch;
  public nestedNodes: Node[] = [];

  public flatNodes: Node[] = [];
  public flatNodesChange = new BehaviorSubject<Node[]>([]);

  public controllerOptions:ControllerOptions = new ControllerOptions();
  public controllerActions:ControllerAction[] = [];

  public refetchData: EventEmitter<any> = new EventEmitter<any>();

  constructor() {
    this.flatNodesChange.asObservable().subscribe(flatNodes => {
      if (flatNodes?.length > 0) {
        this.createNestedTree();
      }
    });
  }

  addUniqueFlatNode(node:Node){
    let existingNode = this.flatNodes.find(x => x.id === node.id);
    if (!existingNode) {
      this.flatNodes = [...this.flatNodes,node];
    } else {
      existingNode.id = node.id;
      existingNode.parentId = node.parentId;
      existingNode.title = node.title;
      existingNode.code = node.code;
      existingNode.level = node.level;
      existingNode.value = node.value;
    }
  }
  createNestedTree(){
    this.nestedNodes = [];

    this.flatNodes.filter(x => !x.parentId).forEach(flatNode => {
      this.nestedNodes.push(flatNode);
    });

    this.nestedNodes.forEach(node => {
      this.findChildren(node);
    });


  }

  findChildren(node:Node) {
     node.hasChild = false;
     node.children = [];
     let children = this.getChildren(node);
     if(children.length > 0 && node._expanded) {
       node.hasChild = true;
       children.forEach(childNode => {
         node.children.push(childNode);
         this.findChildren(childNode);
       });
     }
  }

  render(){
    this.flatNodesChange.next(this.flatNodes);
  }

  reset() {
    this.collapseAll();
    this.deselectAll();
    this.render();
  }

  flush () {
    this.flatNodes = [];
    this.nestedNodes = [];
    this.render();
  }


  toggle(node:Node) {
    if (this.controllerOptions.closeSiblingsOnExpand) {
      let expandedSibling =  this.getSiblings(node).find(x => x._expanded);
      if(expandedSibling) {
        this.collapse(expandedSibling);
      }
    }
    node._expanded ? this.collapse(node) : this.expand(node);
    this.render();
  }
  expand(node:Node) {
      node._expanded = true;
  }
  collapse(node:Node) {
      node._expanded = false;
      if (this.controllerOptions.closeDescendantsOnCollapse) {
        this.collapseDescendants(node);
      }
  }

  collapseDescendants(node:Node) {
    this.getDescendants(node).filter(x => x._expanded).forEach(descendantNode => {
      this.collapse(descendantNode);
    })
  }

  selectHandler(node:Node) {
    if (node._selected) {
      if (this.controllerOptions.selectChildrenOnSelect){
        this.selectAllDescendants(node);
      }
      if(this.controllerOptions.selectParentsOnSelect){
        this.selectAllParents(node);
      }
    } else if(!node._selected) {
      if(this.controllerOptions.selectChildrenOnSelect) {
        this.deselectAllDescendants(node);
      }
    }
  }
  select(node:Node) {
    node._selected = true;
  }
  selectByIds(ids:any[]) {
    ids.forEach(x => {
      let node :any = this.flatNodes.filter((y:any) => y.id === x)
      if (node) node._selected = true;
    })
  }
  deselect(node:Node) {
    node._selected = false;
  }
  selectAll() {
    this.flatNodes.forEach(node => {
      node._selected = true;
    })
  }
  deselectAll() {
    this.flatNodes.forEach(node => {
      node._selected = false;
    })
  }
  selectAllDescendants(node :Node) {
    this.getDescendants(node).forEach(descendantNode => {
      this.select(descendantNode);
    })
  }
  deselectAllDescendants(node :Node) {
    this.getDescendants(node).forEach(descendantNode => {
        this.deselect(descendantNode);
    })
  }
  selectAllParents(node: Node) {
    if (node.parentId) {
      let parent = this.getParent(node);
      if(parent) {
        parent._selected = true;
        if(parent.parentId) {
          this.selectAllParents(parent);
        }
      }
    }
  }

  getSelectedNodes() {
    return this.flatNodes.filter(x => x._selected);
  }


  getSiblings(node:Node) {
    return this.flatNodes.filter(x =>  x.parentId === node.parentId && x != node);
  }
  getChildren(node:Node){
    return this.flatNodes.filter(x =>  x.parentId === node.id);
  }
  getDescendants(node:Node) {
    return this.flatNodes.filter(x => x.level?.startsWith(node.level));
  }
  getParent(node: Node){
    return this.flatNodes.find(x => x.id === node.parentId);
  }

  collapseAll(){
    this.flatNodes = this.flatNodes.map(node => {
      node._expanded = false;
      return node;
    });
  }

  doesHaveAction(actionName: string, node?: Node): ControllerAction | undefined {
    let action = this.controllerActions.find(x => x.type === actionName);
    if (!node || !action?.conditionPropertyName) {
      return action
    } else {
        // @ts-ignore
        if (node.value[action.conditionPropertyName] === action.conditionAcceptedIf) {
          return action
        } else {
          return undefined;
        }
    }
  }
}

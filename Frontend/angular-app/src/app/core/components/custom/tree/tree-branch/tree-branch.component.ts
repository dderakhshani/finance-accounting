import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ListControl} from "../list-control/list-control";
import {NodeBranch} from "../list-control/models/node-branch";
import {Node} from "../list-control/models/node";

@Component({
  selector: 'app-tree-branch',
  templateUrl: './tree-branch.component.html',
  styleUrls: ['./tree-branch.component.scss']
})
export class TreeBranchComponent implements OnInit {

  @Input() listControl!:ListControl;
  @Output() branchChanged : EventEmitter<NodeBranch> = new EventEmitter<NodeBranch>();
  nestedBranches:NodeBranch[] = [];
  constructor() { }

  ngOnInit(): void {
    this.createNestedBranches();
  }

  createNestedBranches() {
    this.listControl.nodeBranches.filter(x => !x.parentId).forEach(node => {
      this.nestedBranches.push(node);
    });


    this.nestedBranches.forEach(node => {
      this.findChildren(node);
    });
  }

  findChildren(node:NodeBranch) {
    node.children = [];
    let children = this.getChildren(node);
    if(children.length > 0) {
      children.forEach(childNode => {
        node.children.push(childNode);
        this.findChildren(childNode);
      });
    }
  }

  getChildren(node:NodeBranch){
    return this.listControl.nodeBranches.filter(x =>  x.parentId === node.id);
  }
}

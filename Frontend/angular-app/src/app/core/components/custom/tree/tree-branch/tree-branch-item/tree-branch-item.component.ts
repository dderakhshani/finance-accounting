import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NodeBranch} from "../../list-control/models/node-branch";
import {Branch} from "../../../../../../modules/admin/entities/branch";

@Component({
  selector: 'app-tree-branch-item',
  templateUrl: './tree-branch-item.component.html',
  styleUrls: ['./tree-branch-item.component.scss']
})
export class TreeBranchItemComponent implements OnInit {

  @Input() branch!: NodeBranch;
  @Output() branchChanged : EventEmitter<NodeBranch> = new EventEmitter<NodeBranch>();
  @Input() subLevel:number = 1;
  constructor() { }

  ngOnInit(): void {
  }
  nodeClicked(node: NodeBranch) {
    if (node.children?.length > 0) {
      node._toggled = !node._toggled;
      this.branchChanged.emit(node);

    } else {
      this.branchChanged.emit(node);
    }
  }
}

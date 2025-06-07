import {Component} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {BranchDialogComponent} from "./branch-dialog/branch-dialog.component";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {GetBranchesQuery} from "../../repositories/branch/queries/get-branches-query";
import {Branch} from "../../entities/branch";
import {PageModes} from "../../../../core/enums/page-modes";

@Component({
  selector: 'app-branches',
  templateUrl: './branches.component.html',
  styleUrls: ['./branches.component.scss']
})
export class BranchesComponent extends BaseComponent {

  branches: Branch[] = [];

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    await this.get()
  }

  async get() {
    await this._mediator.send(new GetBranchesQuery(0, 0, undefined, "id ASC")).then(res => {
      this.branches = res.data;
    })
  }

  add(node: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      node: node,
      pageMode: PageModes.Add
    };
    let dialogReference = this.dialog.open(BranchDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.branches.push(response)
        this.branches = [...this.branches]
      }
    })
  }

  update(node: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      node: node,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(BranchDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          let branch = this.branches.find(x => x.id === response.id)
          if (branch) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              branch[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let branchToRemove = this.branches.find(x => x.id === response.id)
          if (branchToRemove) {
            this.branches.splice(this.branches.indexOf(branchToRemove), 1)
            this.branches = [...this.branches]
          }
        }
      }
    })
  }


  initialize() {

  }


  delete() {

  }

  close() {

  }

}

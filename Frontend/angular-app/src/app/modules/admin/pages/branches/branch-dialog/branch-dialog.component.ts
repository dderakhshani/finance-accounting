import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../core/enums/page-modes";
import {BaseValueType} from "../../../entities/base-value-type";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {CreateBranchCommand} from "../../../repositories/branch/commands/create-branch-command";
import {UpdateBranchCommand} from "../../../repositories/branch/commands/update-branch-command";
import {DeleteBranchCommand} from "../../../repositories/branch/commands/delete-branch-command";


@Component({
  selector: 'app-branches-dialog',
  templateUrl: './branch-dialog.component.html',
  styleUrls: ['./branch-dialog.component.scss']
})
export class BranchDialogComponent extends BaseComponent {
  pageModes = PageModes;
  node!: BaseValueType;


  constructor(
    private _mediator:Mediator,
    private dialogRef: MatDialogRef<BranchDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.node = data.node;
    this.pageMode = data.pageMode;
  }

 async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    this.initialize()
  }

  initialize() {
    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreateBranchCommand()
      if (this.node) {
        newRequest.parentId = this.node.id;
      }
      this.request = newRequest;
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateBranchCommand().mapFrom(this.node)
    }
  }
 async add(param?: any) {
    let response = await this._mediator.send(<CreateBranchCommand>this.request);
   this.dialogRef.close({
     response:response,
     pageMode:this.pageMode
   })
  }

  async update(entity?: any) {
    let response = await this._mediator.send(<UpdateBranchCommand>this.request);
    this.dialogRef.close({
      response:response,
      pageMode:this.pageMode
    })
  }



   async delete(entity?: any){
     let response = await this._mediator.send(new DeleteBranchCommand((<UpdateBranchCommand>this.request).id ?? 0));
     this.dialogRef.close({
       response:response,
       pageMode:PageModes.Delete
     })
  }

  get(id?: number): any {
  }



  close(): any {
  }


}

import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {FormControl} from "@angular/forms";
import {
  SubmitMoadianInvoicesByIdsCommand
} from "../../../../../repositories/moadian/moadian-invoice-header/commands/submit-moadian-invoices-by-Ids-command";

@Component({
  selector: 'app-moadian-verification-code-dialog',
  templateUrl: './moadian-verification-code-dialog.component.html',
  styleUrls: ['./moadian-verification-code-dialog.component.scss']
})
export class MoadianVerificationCodeDialogComponent extends BaseComponent {




  constructor(
    private dialogRef: MatDialogRef<MoadianVerificationCodeDialogComponent>,
    private mediator: Mediator,
    @Inject(MAT_DIALOG_DATA) data : SubmitMoadianInvoicesByIdsCommand
  ) {
    super();
    data.generateCode = false;
    this.request = data
  }

  async add() {
    this.isLoading = true;
    await this.mediator.send(this.request).then(res => {
      this.isLoading = false;
      this.dialogRef.close(true)
    }).catch(() => {
      this.isLoading = false;
    })
  }
  ngOnInit(): void {
  }



  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

  initialize(params?: any): any {
  }

  resolve(params?: any): any {
  }

  update(param?: any): any {
  }

}

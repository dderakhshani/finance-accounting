import {Component, Inject, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../../../../core/enums/page-modes";
import {ChequeBooksSheet} from "../../../../../../entities/cheque-books-sheet";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Mediator} from "../../../../../../../../core/services/mediator/mediator.service";
import {
  UpdatePayablesChequeBooksSheetCommand
} from "../../../../../../repositories/payables_cheque_book-sheets/commands/update-payables-cheque-books-sheet-command";
import {
  CancelPayablesChequeBooksSheetCommand
} from "../../../../../../repositories/payables_cheque_book-sheets/commands/cancel-payables-cheque-books-command";

@Component({
  selector: 'app-cancel-cheque-books-sheets-dialog',
  templateUrl: './cancel-cheque-books-sheets-dialog.component.html',
  styleUrls: ['./cancel-cheque-books-sheets-dialog.component.scss']
})
export class CancelChequeBooksSheetsDialogComponent extends BaseComponent {

  pageModes = PageModes;
  payload: any
  chequeSheet!: ChequeBooksSheet;

  constructor(
    private dialogRef: MatDialogRef<CancelChequeBooksSheetsDialogComponent>,
  @Inject(MAT_DIALOG_DATA) data: any,
    private mediator:Mediator
) {
    super();
    this.payload = data;
  }


  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.initialize()
  }

  initialize(params?: any): any {
    this.pageMode = +this.payload.pageMode;
    this.chequeSheet = this.payload.chequeSheet;

    if (this.pageMode === PageModes.Update) {
      this.request = new CancelPayablesChequeBooksSheetCommand().mapFrom(this.payload.chequeSheet);
    }

  }

  async add(param?: any) {


  }

  async update(param?: any) {
    await this.mediator.send(<CancelPayablesChequeBooksSheetCommand>this.request).then(res => {
      this.dialogRef.close(res)
    })
  }
  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

  changeSheetsCount() {

  }
}

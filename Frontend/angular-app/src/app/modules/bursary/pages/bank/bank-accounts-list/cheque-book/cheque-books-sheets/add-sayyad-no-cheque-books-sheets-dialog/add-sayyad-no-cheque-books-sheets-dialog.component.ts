import {Component, Inject, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../../../../core/enums/page-modes";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Mediator} from "../../../../../../../../core/services/mediator/mediator.service";

import {ChequeBooksSheet} from "../../../../../../entities/cheque-books-sheet";
import {
  UpdatePayablesChequeBooksCommand
} from "../../../../../../repositories/payables_cheque_books/commands/update-payables-cheque-books";
import {
  UpdatePayablesChequeBooksSheetCommand
} from "../../../../../../repositories/payables_cheque_book-sheets/commands/update-payables-cheque-books-sheet-command";



@Component({
  selector: 'app-add-sayyad-no-cheque-books-sheets-dialog',
  templateUrl: './add-sayyad-no-cheque-books-sheets-dialog.component.html',
  styleUrls: ['./add-sayyad-no-cheque-books-sheets-dialog.component.scss']
})
export class AddSayyadNoChequeBooksSheetsDialogComponent extends BaseComponent {

  pageModes = PageModes;
  payload: any
  chequeSheet!: ChequeBooksSheet;

  constructor(
    private dialogRef: MatDialogRef<AddSayyadNoChequeBooksSheetsDialogComponent>,
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
      this.request = new UpdatePayablesChequeBooksSheetCommand().mapFrom(this.payload.chequeSheet);
    }

  }

  async add(param?: any) {


  }

  async update(param?: any) {
    await this.mediator.send(<UpdatePayablesChequeBooksSheetCommand>this.request).then(res => {
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

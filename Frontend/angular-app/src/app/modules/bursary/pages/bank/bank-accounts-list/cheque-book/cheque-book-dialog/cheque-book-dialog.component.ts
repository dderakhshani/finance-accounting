import {Component, Inject, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";

import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {
  CreatePayablesChequeBooksCommand
} from "../../../../../repositories/payables_cheque_books/commands/create-payables-cheque-books";
import {
  UpdatePayablesChequeBooksCommand
} from "../../../../../repositories/payables_cheque_books/commands/update-payables-cheque-books";
import {
  DeletePayablesChequeBooksCommand
} from "../../../../../repositories/payables_cheque_books/commands/delete-payables-cheque-books-command";
import {
  ConfirmDialogComponent,
  ConfirmDialogIcons
} from "../../../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";
import {
  UnCancelPayablesChequeBooksSheetCommand
} from "../../../../../repositories/payables_cheque_book-sheets/commands/un-cancel-payables-cheque-books-sheet-command";

@Component({
  selector: 'app-cheque-book-dialog',
  templateUrl: './cheque-book-dialog.component.html',
  styleUrls: ['./cheque-book-dialog.component.scss']
})
export class ChequeBookDialogComponent extends BaseComponent {

  pageModes = PageModes;
  payload: any
  bankAccountId: number = 0;
  dialogHeader :string = 'ثبت دسته چک';
  sheetsCountCount = [
    {
      title: '10 برگی',
      value : 10,
      id: 1
    },
    {
      title: '25 برگی',
      value : 25,
      id: 2
    },

    {
      title: '50 برگی',
      value : 50,
      id: 3
    }
  ]
  constructor(
    private dialogRef: MatDialogRef<ChequeBookDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,
    private mediator:Mediator,
    public dialog: MatDialog
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

    let request;
    if (this.pageMode === PageModes.Add) {
      this.dialogHeader ='ثبت دسته چک'
      let request = new CreatePayablesChequeBooksCommand();
      request.bankAccountId = +this.payload.bankAccountId;
      this.request = request;
    }
    if (this.pageMode === PageModes.Update) {
      this.dialogHeader ='ویرایش دسته چک'
      this.request = new UpdatePayablesChequeBooksCommand().mapFrom(this.payload.ChequeBookDetail);
    }

  }

  async add(param?: any) {
    await this.mediator.send(<CreatePayablesChequeBooksCommand>this.request).then(res => {
      this.dialogRef.close(res)
    })

  }

  async update(param?: any) {
    await this.mediator.send(<UpdatePayablesChequeBooksCommand>this.request).then(res => {
      this.dialogRef.close(res)
    })

  }
  close(): any {
  }

  async delete(param?: any) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'اخطار حذف دسته چک',
        message: 'آیا از حذف دسته چک انتخاب شده مطمئن هستید ؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: {title: 'بله', show: true}, cancel: {title: 'خیر', show: true}
        }
      }
    });
    dialogRef.afterClosed().subscribe(async (result: boolean) => {
      if (result) {
        await this.mediator.send(new DeletePayablesChequeBooksCommand(this.payload.ChequeBookDetail.id )).then(res => {
          this.dialogRef.close(res)
        })
      }
    });

  }

  get(param?: any): any {
  }

  changeSheetsCount() {

  }
}

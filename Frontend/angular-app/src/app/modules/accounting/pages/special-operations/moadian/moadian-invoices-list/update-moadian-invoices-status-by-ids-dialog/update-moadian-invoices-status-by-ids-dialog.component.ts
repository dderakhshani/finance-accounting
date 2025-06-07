import {Component, Inject, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {
  ImportMoadianInvoicesByExcelCommand
} from "../../../../../repositories/moadian/moadian-invoice-header/commands/import-moadian-invoices-by-excel-command";
import {
  UpdateMoadianInvoicesStatusByIdsCommand
} from "../../../../../repositories/moadian/moadian-invoice-header/commands/update-moadian-invoices-status-by-ids-command";

@Component({
  selector: 'app-update-moadian-invoices-status-by-ids-dialog',
  templateUrl: './update-moadian-invoices-status-by-ids-dialog.component.html',
  styleUrls: ['./update-moadian-invoices-status-by-ids-dialog.component.scss']
})
export class UpdateMoadianInvoicesStatusByIdsDialogComponent extends BaseComponent {

  selectedIds: number[] = [];

  statuses : any[] = []
  constructor(
    private dialogRef: MatDialogRef<UpdateMoadianInvoicesStatusByIdsDialogComponent>,
    private mediator: Mediator,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.selectedIds = data.selectedIds
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.statuses = [
      {
        title:'ارسال شده',
        value:'SENT'
      },
      {
        title:'ثبت شده',
        value:'SUCCESS'
      },
      {
        title:'خطا',
        value:'FAILED'
      },
      {
        title:'در انتظار ثبت',
        value:'IN_PROGRESS'
      },
      {
        title:'اطلاعات نامعتبر',
        value:'INVALID_DATA'
      },
      {
        title:'رد شده',
        value:'DECLINED'
      },
      {
        title:'ارسال نشده',
        value:null
      },
    ]
    this.initialize()
  }

  initialize(params?: any): any {
    let request = new UpdateMoadianInvoicesStatusByIdsCommand();
    request.ids = this.selectedIds;
    this.request = request;
  }
  async add(param?: any) {
    this.isLoading = true;
    await this.mediator.send(this.request).then(res => {
      this.isLoading = false;
      this.dialogRef.close(true)
    }).catch(() => {
      this.isLoading = false;
    })
  }



  update(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }


}

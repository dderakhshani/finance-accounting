import {Component, Inject} from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {
  UpdateMoadianInvoiceDetailCommand
} from "../../../../../repositories/moadian/moadian-invoice-detail/commands/update-moadian-invoice-detail-command";
import {
  CreateMoadianInvoiceDetailCommand
} from "../../../../../repositories/moadian/moadian-invoice-detail/commands/create-moadian-invoice-detail-command";

@Component({
  selector: 'app-moadian-invoice-header-dialog',
  templateUrl: './moadian-invoice-detail-dialog.component.html',
  styleUrls: ['./moadian-invoice-detail-dialog.component.scss']
})
export class MoadianInvoiceDetailDialogComponent extends BaseComponent {

  pageModes = PageModes;
  payload: any

  constructor(
    private dialogRef: MatDialogRef<MoadianInvoiceDetailDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
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
    this.pageMode = this.payload.pageMode;

    if (this.pageMode === PageModes.Add) {
      this.request = new CreateMoadianInvoiceDetailCommand();
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateMoadianInvoiceDetailCommand().mapFrom(this.payload.invoiceDetail.getRawValue());
    }
  }

  add(param?: any): any {
    this.dialogRef.close(this.form.getRawValue())
  }

  update(param?: any): any {
    this.dialogRef.close(this.form.getRawValue())
  }
  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

}

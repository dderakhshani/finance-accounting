import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { UpdateStatusReceiptInsertedByCustomersCommand } from 'src/app/modules/bursary/repositories/financial-request/receipts-list-inserted-by-customers/commands/update-status-receipt-inserted-by-customers-command';

@Component({
  selector: 'app-remove-receipt-with-message-dialog',
  templateUrl: './remove-receipt-with-message-dialog.component.html',
  styleUrls: ['./remove-receipt-with-message-dialog.component.scss']
})
export class RemoveReceiptWithMessageDialogComponent extends BaseComponent {
  receiptId !: number

  constructor(public dialogRef: MatDialogRef<RemoveReceiptWithMessageDialogComponent>,
    private _mediator: Mediator,
    private router: Router,
    private route: ActivatedRoute,
    @Inject(MAT_DIALOG_DATA) public value: any) {
      super(route, router);
  }

  ngOnInit(): void {
    this.resolve()
  }

  async resolve(params?: any) {
    this.receiptId = JSON.parse(this.value.data);
    await this.initialize();
  }

  async removeReceipt() {
    let result =  await this._mediator.send(<UpdateStatusReceiptInsertedByCustomersCommand>this.request);
    this.dialogRef.close();
  }

  async initialize(params?: any) {
    this.request = new UpdateStatusReceiptInsertedByCustomersCommand(this.receiptId);
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
  get(param?: any) {
    throw new Error('Method not implemented.');
  }
  update(param?: any) {
    throw new Error('Method not implemented.');
  }
  delete(param?: any) {
    throw new Error('Method not implemented.');
  }
  close() {
    throw new Error('Method not implemented.');
  }

}

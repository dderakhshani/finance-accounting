import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { PageModes } from '../../../../../core/enums/page-modes';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { UpdateQuantityCommand } from '../../../repositories/receipt/commands/reciept/update-quantity-command';


@Component({
  selector: 'app-update-quantity-dialog',
  templateUrl: './update-quantity-dialog.component.html',
  styleUrls: ['./update-quantity-dialog.component.scss']
})
export class UpdateQuantityDialogComponent extends BaseComponent {


  pageModes = PageModes;
  fomDocumentItem: any;

  

  constructor(
    private _mediator: Mediator,
    public Service: PagesCommonService,
    private dialogRef: MatDialogRef<UpdateQuantityDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    
    this.fomDocumentItem = data.DocumentItem;
    this.pageMode = data.pageMode;
    this.request = new UpdateQuantityCommand();
    
  }


  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {


    await this.initialize()
  }

  async initialize() {
    
    var request = new UpdateQuantityCommand();
    request.id = this.fomDocumentItem.controls['id'].value;
    request.commodityTitle = this.fomDocumentItem.controls['commodityTitle'].value;
    request.commodityCode = this.fomDocumentItem.controls['commodityCode'].value;
    request.quantity = this.fomDocumentItem.controls['quantity'].value;
    
    this.request = request;
   
    this.disableControls()
  }

  async update(entity?: any) {
    this.fomDocumentItem.controls['quantity'].setValue(this.form.controls['quantity'].value)
    await this._mediator.send(<UpdateQuantityCommand>this.request).then(res => {
      this.dialogRef.close({
        response: this.fomDocumentItem,
        pageMode: this.pageMode
      })
    });
  }

  

  personSelect(item: any) {

    this.form.controls.accountReferenceId.setValue(item?.id);

  }
  disableControls() {

    if (this.fomDocumentItem.controls.hasPermissionEditQuantity.value == false) {
      this.form.controls['quantity'].disable();
    }
    this.form.controls['commodityTitle'].disable();
    this.form.controls['commodityCode'].disable();
  }

  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }
  delete(param?: any) {
    throw new Error('Method not implemented.');
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
}


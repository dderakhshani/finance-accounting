import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { PageModes } from '../../../../../core/enums/page-modes';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { SplitCommodityQuantityCommand } from '../../../repositories/receipt/commands/reciept/split-commodity-quantity';
import { Quantities } from '../../../entities/receipt';

@Component({
  selector: 'app-split-quantity-dialog',
  templateUrl: './split-quantity-dialog.component.html',
  styleUrls: ['./split-quantity-dialog.component.scss']
})
export class SplitCommodityQuantityDialogComponent extends BaseComponent {


  pageModes = PageModes;
  fomDocumentItem: any;
  public quantities: Quantities[] = [];
  public total:number=0
  

  constructor(
    private _mediator: Mediator,
    public Service: PagesCommonService,
    private dialogRef: MatDialogRef<SplitCommodityQuantityDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    
    this.fomDocumentItem = data.DocumentItem;
    this.pageMode = data.pageMode;
    this.request = new SplitCommodityQuantityCommand();
    
  }


  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {


    await this.initialize()
  }

  async initialize() {
    
    var request = new SplitCommodityQuantityCommand();
    request.documentItemId = this.fomDocumentItem.controls['id'].value;
   
    
    this.request = request;
   
   
  }

  async update(entity?: any) {

    var request = new SplitCommodityQuantityCommand();
    request.documentItemId = this.fomDocumentItem.controls['id'].value;
    request.quantities = this.quantities;

    await this._mediator.send(<SplitCommodityQuantityCommand>request).then(res => {
      this.dialogRef.close({
        response: this.fomDocumentItem,
        pageMode: this.pageMode
      })
    });
  }

  AddItem() {
    this.quantities.push({ quantity :0})
    this.CalculateSum()
    
  }

 
  CalculateSum() {

    this.total = 0;
    this.quantities.forEach(a => this.total += Number(a.quantity));
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


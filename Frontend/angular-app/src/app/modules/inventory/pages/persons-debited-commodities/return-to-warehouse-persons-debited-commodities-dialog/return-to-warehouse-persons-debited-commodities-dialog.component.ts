import { Component, Inject } from '@angular/core';

import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { PageModes } from "../../../../../core/enums/page-modes";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { Units } from '../../../entities/units';
import { UpdateReturnToWarehousePersonsDebitedCommand } from '../../../repositories/persons-debited-commodities/commands/update-retuen -to -warehouse-persons-debited-command';
import { PersonsDebitedCommodities } from '../../../entities/persons-debited-commodities';
import { DeletePersonsDebitedCommand } from '../../../repositories/persons-debited-commodities/commands/delete-persons-debited-commodities-command';
import { Warehouse } from '../../../entities/warehouse';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';

@Component({
  selector: 'app-return-to-warehouse-persons-debited-commodities-dialog',
  templateUrl: './return-to-warehouse-persons-debited-commodities-dialog.component.html',
  styleUrls: ['./return-to-warehouse-persons-debited-commodities-dialog.component.scss']
})
export class UpdateReturnToWarehousePersonsDebitedDialogComponent extends BaseComponent {
  add(param?: any) {
      throw new Error('Method not implemented.');
  }

  pageModes = PageModes;
  PersonsDebitedCommodity!: PersonsDebitedCommodities;
  
  Units: Units[] = [];

  constructor(
    private _mediator: Mediator,
    public Service: PagesCommonService,
    private dialogRef: MatDialogRef<UpdateReturnToWarehousePersonsDebitedDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();

    this.PersonsDebitedCommodity = data.PersonsDebitedCommodity;
    this.pageMode = data.pageMode;
    this.request = new UpdateReturnToWarehousePersonsDebitedCommand();

  }


  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    
   
    await this.initialize()
  }

  async initialize() {

   
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateReturnToWarehousePersonsDebitedCommand().mapFrom(this.PersonsDebitedCommodity)
    }
    
    this.disableControls();
  }

  

  async update(entity?: any) {
    await this._mediator.send(<UpdateReturnToWarehousePersonsDebitedCommand>this.request).then(res => {
      this.dialogRef.close({
        response: res,
        pageMode: this.pageMode
      })
    });
  }

  async delete() {
    await this._mediator.send(new DeletePersonsDebitedCommand(this.PersonsDebitedCommodity.id)).then(res => {

      this.dialogRef.close({
        response: res,
        pageMode: PageModes.Delete
      })

    });
  }
  //-----------------انبار تحویل دهنده-------------------------------
  async WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);


  }
  
  disableControls() {
    this.form.controls['fullName'].disable();
    this.form.controls['commodityTitle'].disable();
    this.form.controls['commodityCode'].disable();
  }
  
  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

}


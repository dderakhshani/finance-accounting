import { Component, Inject } from '@angular/core';

import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { PageModes } from "../../../../../core/enums/page-modes";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { Units } from '../../../entities/units';
import { UpdateAssetSerialPersonsDebitedCommand } from '../../../repositories/persons-debited-commodities/commands/update-asset-serial-persons-debited-command';
import { PersonsDebitedCommodities } from '../../../entities/persons-debited-commodities';
import { DeletePersonsDebitedCommand } from '../../../repositories/persons-debited-commodities/commands/delete-persons-debited-commodities-command';
import { Warehouse } from '../../../entities/warehouse';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { Assets, AssetsSerial } from '../../../entities/Assets';
import { GetAssetsesQuery } from '../../../repositories/assets/queries/get-assetses-query';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';

@Component({
  selector: 'app-update-barcode-dialog',
  templateUrl: './update-barcode-dialog.component.html',
  styleUrls: ['./update-barcode-dialog.component.scss']
})
export class UpdateBarcodeDialogComponent extends BaseComponent {
  delete(param?: any) {
      throw new Error('Method not implemented.');
  }
  add(param?: any) {
      throw new Error('Method not implemented.');
  }

  pageModes = PageModes;
  PersonsDebitedCommodity!: PersonsDebitedCommodities;

  assetsSerials: Assets[]=[]
  
 
  constructor(
    private _mediator: Mediator,
    public Service: PagesCommonService,
    private dialogRef: MatDialogRef<UpdateBarcodeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();

    this.PersonsDebitedCommodity = data.PersonsDebitedCommodity;
    this.pageMode = data.pageMode;
    this.request = new UpdateAssetSerialPersonsDebitedCommand();

  }


  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    
   
    await this.initialize()
  }

  async initialize() {

    let searchQueries: SearchQuery[] = []
    searchQueries.push(new SearchQuery({
      propertyName: "commodityId",
      values: [this.PersonsDebitedCommodity.commodityId],
      comparison: "equal",
      nextOperand: "and"
    }))
    searchQueries.push(new SearchQuery({
      propertyName: "isActive",
      values: [true],
      comparison: "equal",
      nextOperand: "and"
    }))
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateAssetSerialPersonsDebitedCommand().mapFrom(this.PersonsDebitedCommodity)
    }
    await this._mediator.send(new GetAssetsesQuery(undefined, undefined, 0, 0, searchQueries,'')).then(res => {
      this.assetsSerials = res.data

      this.assetsSerials.forEach(a => a.title = a.assetSerial);
    });
    this.disableControls();
  }

  

  async update(entity?: any) {
    await this._mediator.send(<UpdateAssetSerialPersonsDebitedCommand>this.request).then(res => {
      this.dialogRef.close({
        response: res,
        pageMode: this.pageMode
      })
    });
  }

  assetIdSelect(id: number) {
    
    this.form.controls.assetId.setValue(id);

    
  }
 
  disableControls() {
    this.form.controls['fullName'].disable();
    this.form.controls['commodityTitle'].disable();
    this.form.controls['commodityCode'].disable();
    this.form.controls['assetSerial'].disable();
  }
  
  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

}


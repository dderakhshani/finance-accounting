import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from '../../../../../../core/abstraction/base.component';
import { PageModes } from '../../../../../../core/enums/page-modes';
import { Mediator } from '../../../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { CommodityCategory } from '../../../../../commodity/entities/commodity-category';
import { Assets } from '../../../../entities/Assets';
import { BaseValueModel } from '../../../../entities/base-value';
import { UpdateAssetsCommand } from '../../../../repositories/assets/commands/update-assets-command';
import { GetDepreciationTypeBaseValueQuery } from '../../../../repositories/base-value/get-base-value-depreciation-type-query';
import { GetCategoresCodeAssetGroupQuery } from '../../../../repositories/commodity-categories/get-commodity-categories-asset-query';


@Component({
  selector: 'app-assets-commodity-serial-dialog',
  templateUrl: './assets-commodity-serial-dialog.component.html',
  styleUrls: ['./assets-commodity-serial-dialog.component.scss']
})
export class AssetsCommoditySerialDialog extends BaseComponent {


  pageModes = PageModes;
  
  depreciationTypeBaseValue: BaseValueModel[] = [];
  commodityCategoreis: CommodityCategory[] = [];
  
  public Assets =new Assets();


  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<AssetsCommoditySerialDialog>,
    public Service: PagesCommonService,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();

   
    if (data.Assets != undefined) {
      this.Assets = data.Assets ;
      
    }
    this.pageMode = data.pageMode;
    this.request = new UpdateAssetsCommand();
    
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {

   // if (this.pageMode === PageModes.Update) {
     
    //}
    await this.initialize()
  }

  async initialize() {
    this.request = new UpdateAssetsCommand().mapFrom(this.Assets)
    this.disableControls();

    await this._mediator.send(new GetDepreciationTypeBaseValueQuery()).then(res => {
       this.depreciationTypeBaseValue = res.data
    });
    await this._mediator.send(new GetCategoresCodeAssetGroupQuery()).then(res => {
      this.commodityCategoreis = res;
    });
    
  
    
  }
  disableControls() {
    
    this.form.controls['commodityTitle'].disable();
   
  }
  async add() {
    
  }

 
  depreciationSelect(id:number) {
    if (this.Assets != undefined) {
      this.form.controls.depreciationTypeBaseId.setValue(id);
      
    }
  }
  assetGroupIdSelect(id: number) {
   
    if (this.Assets != undefined) {
    
      this.form.controls.assetGroupId.setValue(id);
     
      
    }
    
  }
  async update(entity?: any) {

    await this._mediator.send(<UpdateAssetsCommand>this.request).then(res => {

      this.dialogRef.close({

        response: res,
        pageMode: this.pageMode
      })

    });
   
  }

  async delete() {

  }



  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

}


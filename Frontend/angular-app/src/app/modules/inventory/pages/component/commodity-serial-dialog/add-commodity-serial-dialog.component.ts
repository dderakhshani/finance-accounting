import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { PageModes } from '../../../../../core/enums/page-modes';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { CommodityCategory } from '../../../../commodity/entities/commodity-category';
import { Assets, AssetsSerial } from '../../../entities/Assets';
import { BaseValueModel } from '../../../entities/base-value';
import { GetLastNumberQuery } from '../../../repositories/assets/queries/get-assets-last-number';
import { GetDepreciationTypeBaseValueQuery } from '../../../repositories/base-value/get-base-value-depreciation-type-query';
import { GetCategoresCodeAssetGroupQuery } from '../../../repositories/commodity-categories/get-commodity-categories-asset-query';
import { AddItemsCommand } from '../../../repositories/receipt/commands/receipt-items/add-receipt-items-command';
import { GetDuplicateAssetsQuery } from '../../../repositories/assets/queries/get-duplicate-assets';


@Component({
  selector: 'app-add-commodity-serial-dialog',
  templateUrl: './add-commodity-serial-dialog.component.html',
  styleUrls: ['./add-commodity-serial-dialog.component.scss']
})
export class AddCommoditySerialDialog extends BaseComponent {


  pageModes = PageModes;
  ReceiptItem = new AddItemsCommand();
  depreciationTypeBaseValue: BaseValueModel[] = [];
  commodityCategoreis: CommodityCategory[] = [];
  public AssetsSerials: AssetsSerial[] = [];
  public assets = new Assets();


  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<AddCommoditySerialDialog>,
    public Service: PagesCommonService,
    @Inject(MAT_DIALOG_DATA) data: any,
    public _notificationService: NotificationService,

  ) {
    super();

    this.AssetsSerials = [];
    this.ReceiptItem.commodityCode = data.commodityCode;
    this.ReceiptItem.commodityId = data.commodityId;
    this.ReceiptItem.commodityTitle = data.commodityTitle;
    this.ReceiptItem.quantity = data.quantity;
    this.ReceiptItem.id = data.documentItemId;
    if (data.assets != undefined) {

      this.assets = data.assets;
      this.AssetsSerials = data?.assets?.assetsSerials;
      this.ReceiptItem.quantity = this.AssetsSerials.length;

    }


  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    await this.initialize()
  }

  async initialize() {

    await this._mediator.send(new GetDepreciationTypeBaseValueQuery()).then(res => {
      this.depreciationTypeBaseValue = res.data
    });
    await this._mediator.send(new GetCategoresCodeAssetGroupQuery()).then(res => {
      this.commodityCategoreis = res;

      if (this.assets != undefined) {
        this.assets.prefix = this.commodityCategoreis.find(a => a.id == this.assets.assetGroupId)?.code;
      }
    });

    this.GenerateAssetsSerials();

  }
  //تولید شماره سریال ها بر اساس تعداد کالا
  GenerateAssetsSerials() {
    var ls: number = this.AssetsSerials.length > 0 ? this.AssetsSerials.length : 0;

    var length: number = Number(this.ReceiptItem.quantity) - ls;

    if (length >= 0) {
      for (let i = 0; i < length; i++) {
        this.AssetsSerials.push({
          commodityId: Number(this.ReceiptItem.commodityId),
          serial: undefined,
          id: undefined,
          selected: undefined,
          title: undefined,
          commoditySerial: undefined,
          description: undefined,
        })

      }
    }
    else {
      //از آخر اموالی که حذف شده است را پاک می کند
      this.AssetsSerials.reverse().splice(0, -1 * length)
      this.AssetsSerials.reverse();
    }

  }
  async add() {

   
    this.assets.assetsSerials = this.AssetsSerials;
    this.assets.documentItemId = this.ReceiptItem.id;

    var request= new GetDuplicateAssetsQuery();
    request.AssetsSerial = this.AssetsSerials;
    

    if (this.AssetsSerials.filter(a => a.serial == undefined).length == 0 && this.AssetsSerials.length == this.ReceiptItem.quantity) {

      await this._mediator.send(<GetDuplicateAssetsQuery>request).then(res => {
        if (res.serial === undefined) {
          this.dialogRef.close({

            response: this.assets,
          })
        }
        else {
          this.Service.showHttpFailMessage(`شماره کد اموال ${res.serial} تکراری است`)
         
        }
       
      })

     
    }
    else if (this.AssetsSerials.filter(a => a.serial == undefined).length > 0) {
      this.Service.showHttpFailMessage('همه شماره سریال ها را به طور کامل وارد نمایید.')
    }
    else if (this.AssetsSerials.length != this.ReceiptItem.quantity) {
      this.Service.showHttpFailMessage('تعداد شماره سریال های وارد شده با تعداد کالاهای موجود تطابق ندارد ، مجددا عمل تولید شماره سریال را انجام دهید')
    }
  }

  CreateCode() {
    var index = 0;
    this.GenerateAssetsSerials();
    if (this.assets.startWithNumber != undefined)
      this.AssetsSerials.forEach(a => {

        let Serial = (Number(this.assets.startWithNumber) + index);
        let prefix = this.assets.prefix != undefined ? this.assets.prefix : ""

        a.serial = prefix + Serial.toString();

        index = index + 1;
      })
  }
  depreciationSelect(id: number) {
    if (this.assets != undefined) {
      this.assets.depreciationTypeBaseId = id
    }
  }
  async assetGroupIdSelect(id: number) {

    if (this.assets != undefined) {
      this.assets.assetGroupId = id
      this.assets.prefix = this.commodityCategoreis.find(a => a.id == id)?.code;
      if (id != undefined) {
        await this._mediator.send(new GetLastNumberQuery(id)).then(res => {

          if (res != undefined) {
            try {
              var text = this.assets.prefix == undefined ? '' : this.assets.prefix;
              this.assets.startWithNumber = Number(res.replace(text, '')) + 1
            }
            catch {

            }

          }

        });
      }



    }

  }
  print(): void {
    var barcode: string = "";
    this.AssetsSerials.forEach(a => {

      barcode += `
      <div style='width: 190px; height: 75px; border-style: double;  text-align: center; border-radius: 5px; align-items: center; justify-content: center; display: flex;margin:5px; '>
        <h1 style='text-align:center'>${a.serial}</h1>
      </div>`
      
    });
    this.Service.onPrintLable(barcode);

  }
  async update(entity?: any) {

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


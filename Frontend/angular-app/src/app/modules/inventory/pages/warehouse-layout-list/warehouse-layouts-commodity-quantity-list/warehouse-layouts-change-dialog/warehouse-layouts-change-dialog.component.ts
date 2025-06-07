import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import { BaseComponent } from '../../../../../../core/abstraction/base.component';
import { PageModes } from '../../../../../../core/enums/page-modes';
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { SearchQuery } from '../../../../../../shared/services/search/models/search-query';
import {  WarehouseLayout, WarehouseLayoutTree } from '../../../../entities/warehouse-layout';
import { WarehouseLayoutsCommodityQuantity } from '../../../../entities/warehouse-layouts-commodity-quantity';
import { ChangeWarehouseLayoutCommand } from '../../../../repositories/warehouse-layout/commands/change-warehouse-layout-command';
import { GetWarehouseLayoutsQuery } from '../../../../repositories/warehouse-layout/queries/get-warehouse-layouts-query';

@Component({
  selector: 'app-warehouse-layouts-change-dialog',
  templateUrl: './warehouse-layouts-change-dialog.component.html',
  styleUrls: ['./warehouse-layouts-change-dialog.component.scss']
})
export class ChangeWarehouseLayoutsDialogComponent extends BaseComponent {
  

  pageModes = PageModes;
  WarehouseLayout!: WarehouseLayoutsCommodityQuantity;
  WarehouseLayouts: WarehouseLayout[] = [];


  constructor(
    private _mediator:Mediator,
    private dialogRef: MatDialogRef<ChangeWarehouseLayoutsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any,
    public _notificationService: NotificationService,
  ) {
    super();
   
    this.WarehouseLayout = data.warehouseLayout;
   
    this.request = new ChangeWarehouseLayoutCommand();


  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    
   
    await this.initialize()
  }

  async initialize() {

    let newRequest = new ChangeWarehouseLayoutCommand()

        newRequest.warehouseLayoutTitle = this.WarehouseLayout.warehouseLayoutTitle;
        newRequest.commodityTitle = this.WarehouseLayout.commodityTitle;
        newRequest.commodityId = Number(this.WarehouseLayout.commodityId);
        newRequest.currentWarehouseLayoutId = Number(this.WarehouseLayout.warehouseLayoutId);
        newRequest.quantity = Number(this.WarehouseLayout.quantity);
        newRequest.commodityCode = this.WarehouseLayout.commodityCode;
        newRequest.warehouseLayoutCapacity = Number(this.WarehouseLayout.warehouseLayoutCapacity);
        

    this.request = newRequest;
    this.disableControls();
    await this.getWarehouseLayout('');

  }

  async getWarehouseLayout(searchTerm: string) {
    
    
    let searchQuery = [
      new SearchQuery({
        propertyName: 'warehouseId',
        comparison: 'equal',
        values: [this.WarehouseLayout.warehouseId],
        nextOperand:'and'
      }),
      new SearchQuery({
        propertyName: 'lastLevel',
        comparison: 'equal',
        values: [true],
        nextOperand: 'and'
      }),
       
    ]
    if (searchTerm != undefined && searchTerm != '') {
     var k= new SearchQuery({
        propertyName: 'title',
        comparison: 'contains',
        values: [searchTerm],
        nextOperand: 'and'
      })
      searchQuery.push(k);
     
    }

    await this._mediator.send(new GetWarehouseLayoutsQuery(0, 25, searchQuery)).then(res => {
      this.WarehouseLayouts = res.data;
    })

  }
  
  getWarehouseLayoutId(id: number) {

    this.form.controls.warehouseLayoutId.setValue(id);

  }
  async add(){

    await this._mediator.send(<ChangeWarehouseLayoutCommand>this.request).then(res => {

      this.dialogRef.close({

        response:res,
      
      })

    });
  }
  disableControls() {
    this.form.controls['warehouseLayoutTitle'].disable();
    this.form.controls['warehouseLayoutCapacity'].disable();
    this.form.controls['quantity'].disable();
    this.form.controls['commodityTitle'].disable();
    this.form.controls['commodityId'].disable();
    this.form.controls['commodityCode'].disable();
    
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


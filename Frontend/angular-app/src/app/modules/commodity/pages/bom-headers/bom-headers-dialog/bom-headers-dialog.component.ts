import { Component, Inject, Input, OnInit } from '@angular/core';
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { PageModes } from "../../../../../core/enums/page-modes";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormArray } from '@angular/forms';
import { BomHeader } from '../../../entities/boms-header';
import { GetBomHeaderQuery } from '../../../repositories/bom-headers/queries/get-bom-header-query';
import { UpdateBomHeadersCommand } from '../../../repositories/bom-headers/commands/update-bom-headers-command';
import { CreateBomHeadersCommand } from '../../../repositories/bom-headers/commands/create-bom-headers-command';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import Bom from '../../../entities/bom';
import { CreateBomHeaderItemCommand } from '../../../repositories/bom-item-headers/commands/create-bom-hearder-item-command';
import { DeleteBomHeadersCommand } from '../../../repositories/bom-headers/commands/delete-bom-headers-command';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { GetBomItemBybomIdQuery } from '../../../repositories/bom-item/queries/get-bom-item-by-bomId-query';
import { BomItem } from '../../../entities/bom-Item';
import { GetBomsByCommodityCategoryIdQuery } from '../../../repositories/bom/queries/get-boms-commodity-categoryId';
import { GetBomHeadersAllQuery } from '../../../repositories/bom-headers/queries/get-bom-headers-all-query';

@Component({
  selector: 'app-bom-headers-dialog',
  templateUrl: './bom-headers-dialog.component.html',
  styleUrls: ['./bom-headers-dialog.component.scss']
})
export class BomHeadersDialogComponent extends BaseComponent {
  public entity!: BomHeader;
  public boms: Bom[] = [];
  public bomItem: BomItem[] = [];
  public bomHeaderValues: BomHeader[] = [];
  

  public pageModes = PageModes;
  public commodityCategoryId!: number
  public commodityId!: number
  private displayPage: string = '';
  public PageTitle: string = 'فرمول ساخت';
  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<BomHeadersDialogComponent>,
    public _notificationService: NotificationService,
    @Inject(MAT_DIALOG_DATA) data: any) {
    super();
    this.entity = data.entity;
    this.pageMode = data.pageMode;

    this.commodityCategoryId = data.commodityCategoryId;
    this.commodityId = data.commodityId;
    this.displayPage = data.displayPage;
    this.request = new CreateBomHeadersCommand();
  }

  async ngOnInit() {
    await this.resolve()
    this.initialize();
  }

  async resolve() {


    if (this.pageMode === PageModes.Add) {
      var newRequest = new CreateBomHeadersCommand();
      newRequest.commodityId = this.commodityId;
      this.request = newRequest;
      this.PageTitle = 'افزودن فرمول ساخت'

    }
    else if (this.pageMode === PageModes.Update && this.displayPage != 'copy') {

      this._mediator.send(new GetBomHeaderQuery(this.entity.id)).then(res => {

        this.entity = res;
        this.request = new UpdateBomHeadersCommand().mapFrom(this.entity)
      })
      this.PageTitle = 'ویرایش فرمول ساخت'
    }
    else if (this.pageMode === PageModes.Update && this.displayPage == 'copy') {

      this._mediator.send(new GetBomHeaderQuery(this.entity.id)).then(res => {

        this.entity = res;
        this.request = new CreateBomHeadersCommand().mapFrom(this.entity)
      })
      this.pageMode = PageModes.Add
      this.PageTitle = 'رونوشت فرمول ساخت'
    }

  }

  initialize(): any {
    this.bomFilter('');
    this.bomValueHeaderFilter('');
  }

  async add() {
    if (this.form.valid) {
      let response = await this._mediator.send(<CreateBomHeadersCommand>this.request);
      this.dialogRef.close({
        response: response,
        pageMode: this.pageMode
      })
    }
  }

  async update(entity?: any) {
    if (this.form.valid) {
      let response = await this._mediator.send(<UpdateBomHeadersCommand>this.request);
      this.dialogRef.close({
        response: response,
        pageMode: this.pageMode
      })
    }
  }

  async delete() {
    let response = await this._mediator.send(new DeleteBomHeadersCommand((<UpdateBomHeadersCommand>this.request).id ?? 0));
    this.dialogRef.close({
      response: response,
      pageMode: PageModes.Delete
    })
  }

  CommodityCategoryIdSelect(id: any) {
    this.form.controls.commodityCategoryId.setValue(id);
  }


  async bomFilter(searchTerm: string) {
    let filter: SearchQuery[] = []

    if (searchTerm != '') {

      filter.push(new SearchQuery({
        propertyName: "title",
        values: [searchTerm],
        comparison: 'equal',
        nextOperand: 'and'
      }))

    }
    if (this.pageMode === PageModes.Add) {
      filter.push(new SearchQuery({
        propertyName: "isActive",
        values: [true],
        comparison: 'equal',
        nextOperand: 'and'
      }))
    }


    await this._mediator.send(new GetBomsByCommodityCategoryIdQuery(0, 0, 25, filter, '')).then(res => {

      this.boms = res.data
    })

  }
  async bomValueHeaderFilter(searchTerm: string) {
    let filter: SearchQuery[] = []

    if (searchTerm != '') {

      filter.push(new SearchQuery({
        propertyName: "title",
        values: [searchTerm],
        comparison: 'contains',
        nextOperand: 'or '
      }))
      filter.push(new SearchQuery({
        propertyName: "commodityCode",
        values: [searchTerm],
        comparison: 'contains',
        nextOperand: 'or '
      }))
      filter.push(new SearchQuery({
        propertyName: "commodityTitle",
        values: [searchTerm],
        comparison: 'contains',
        nextOperand: 'or '
      }))

    }

    await this._mediator.send(new GetBomHeadersAllQuery(0, 0, 25, filter, '')).then(res => {

      this.bomHeaderValues = res.data
    })

  }
  async bomValueHeaderSelect(item: any) {


    this.SujestionCommodityByValueHeaderId(item);


  }
  async bomSelect(item: any) {

    this.form.controls.bomId.setValue(item);
    this.SujestionCommodityByBomId(item);


  }
  async SujestionCommodityByValueHeaderId(valueHeaderId: any) {
    var list: FormArray = <FormArray>this.form.controls['values'];
    if (valueHeaderId != undefined) {
      list.clear();
    }
   

    var bom = this.bomHeaderValues.find(a => a.id == valueHeaderId);
   
    this.form.controls.bomId.setValue(bom?.bomId);
    
    
    this._mediator.send(new GetBomHeaderQuery(valueHeaderId)).then(res => {
      var BomHeader = res.values;
      BomHeader.forEach(a => {

        var values = new CreateBomHeaderItemCommand();

        values.usedCommodityId = a.usedCommodityId;
        values.value = a.value;
        values.commodityCode=a.commodityCode
        values.bomWarehouseId=a.bomWarehouseId

        list.push(this.createForm(values, true));
      })

    })
  }
  async SujestionCommodityByBomId(bomId: any) {
    var list: FormArray = <FormArray>this.form.controls['values'];
    if (bomId != undefined) {
      list.clear();
    }
    


    let searchQueries: SearchQuery[] = []
    let request = new GetBomItemBybomIdQuery(bomId, 0, 30, searchQueries, '');

    await this._mediator.send(request).then(response => {
      this.bomItem = response.data;

      this.bomItem.forEach(a => {

        var values = new CreateBomHeaderItemCommand();

        values.usedCommodityId = a.commodityId;
        
        list.push(this.createForm(values, true));
      })
    });
  }
  
  close(): any {
  }
  get(param?: any): any {
  }
}

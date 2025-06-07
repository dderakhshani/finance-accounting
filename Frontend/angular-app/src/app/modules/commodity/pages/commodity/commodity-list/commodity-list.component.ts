import {Component, TemplateRef, ViewChild} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Router} from "@angular/router";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {Commodity} from "../../../entities/commodity";
import {MeasureUnit} from "../../../entities/measure-unit";
import { FormControl, FormGroup } from '@angular/forms';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { UpdateCommodityNationalIdCommand } from '../../../repositories/commodity/commands/update-Commodity-NationalId-command';

import { MatDialog } from '@angular/material/dialog';
import {CommodityTableComponent} from "../../commodity-common/commodity-table/commodity-table.component";






@Component({
  selector: 'app-commodity-list',
  templateUrl: './commodity-list.component.html',
  styleUrls: ['./commodity-list.component.scss']
})
export class CommodityListComponent extends BaseComponent {

  @ViewChild(CommodityTableComponent) commodityTableComponent!: CommodityTableComponent;

  commodities: any[] = []
  tableConfigurations!: TableConfigurations;
  PageMode: string = 'search';
  SearchForm = new FormGroup({
    commodityCode: new FormControl(),
    commodityCategoryId: new FormControl(),
    commodityNationalId: new FormControl(),
    isWrongMeasure: new FormControl(),

  });


  measureUnits:MeasureUnit[] = [];
  constructor(
    private router: Router,
    public dialog: MatDialog,
    private _mediator: Mediator,
    public _notificationService: NotificationService
  ) {
    super();

  }

  async ngAfterViewInit() {
    this.actionBar.actions = [

      PreDefinedActions.add(),
      PreDefinedActions.refresh(),

    ]
    await this.resolve();
    this.request = new UpdateCommodityNationalIdCommand();
  }



  async ngOnInit() {

  }

  async resolve() {

  }


  initialize(params?: any): any {
  }





  CommodityCategoryIdSelect(id: any) {

    this.SearchForm.controls.commodityCategoryId.setValue(id);
  }
  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityCode.setValue(item?.code);

  }
  onOpenEditNationalId() {
    if (this.commodities.filter(a => a.select).length == 0) {
      this._notificationService.showWarningMessage('هیچ کالایی انتخاب نشده است')
      return;
    }
    this.PageMode = 'edit';
  }
  add(param?: any): any {
  }

  close(): any {
    this.PageMode = 'search';
  }



  get(param?: any): any {
    this.commodityTableComponent.get().then(r =>
    {    });
  }
  deleteFilter(){
    this.commodityTableComponent.deleteFilter();
  }

  delete(param?: any): any {
  }

  async update(param?: any) {
    let newRequest = new UpdateCommodityNationalIdCommand();
    this.commodities.filter(a => a.select).forEach(a => {
      newRequest.ids.push(a.id);

    })
    newRequest.commodityNationalId = this.form.controls.commodityNationalId.value;
    newRequest.commodityNationalTitle = this.form.controls.commodityNationalTitle.value;
    this.request = newRequest;


    let response = await this._mediator.send(<UpdateCommodityNationalIdCommand>this.request);
    this.reset();
    await this.get();

    this.PageMode = 'search';

  }

  updateCommodityList($event: any) {
    this.commodities = $event;
    console.log(this.commodities);

  }
}

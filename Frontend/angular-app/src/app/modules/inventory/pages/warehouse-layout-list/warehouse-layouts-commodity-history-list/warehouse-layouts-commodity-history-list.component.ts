import { Component, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { FormAction } from '../../../../../core/models/form-action';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { FormControl, FormGroup } from '@angular/forms';
import { Commodity } from '../../../../commodity/entities/commodity';
import { Warehouse } from '../../../entities/warehouse';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { WarehouseLayoutsCommodityHistoryComponent } from '../../reports/warehouse-layouts-commodity-history/warehouse-layouts-commodity-history.component';


@Component({
  selector: 'app-warehouse-layouts-commodity-history-list',
  templateUrl: './warehouse-layouts-commodity-history-list.component.html',
  styleUrls: ['./warehouse-layouts-commodity-history-list.component.scss']
})
export class WarehouseLayoutsCommodityHistoryListComponent extends BaseComponent {
  child: any;
  @ViewChild(WarehouseLayoutsCommodityHistoryComponent)
  set appShark(child: WarehouseLayoutsCommodityHistoryComponent) {
    this.child = child
  };

  SearchForm = new FormGroup({
    mode: new FormControl(),
    commodityId: new FormControl(),

    warehouseId: new FormControl(),
    documentNo: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });
  constructor(
      public _mediator: Mediator
    , private router: Router
    , private route: ActivatedRoute
    , private sanitizer: DomSanitizer
    , public Service: PagesCommonService
    , public _notificationService: NotificationService,

  ) {
    super(route, router);
  }
  listActions: FormAction[] = [

    FormActionTypes.refresh,

  ]
  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityId.setValue(item?.id);

  }
  WarehouseIdSelect(item: Warehouse) {
    this.SearchForm.controls.warehouseId.setValue(item?.id);
  }
  async ngOnInit() {
    await this.initialize();
    await this.resolve();

  }
  async resolve() { }


  async ngAfterViewInit() {
    this.get();

  }

  async get() {
    await this.child.get();
  }
  async print() {
    await this.child.print();

  }
  async initialize() { }

  async update() {
  }

  async add() {
  }

  close(): any {
  }

  delete(): any {
  }


}

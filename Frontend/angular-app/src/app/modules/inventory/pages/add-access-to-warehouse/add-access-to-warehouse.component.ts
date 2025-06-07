import { Component } from '@angular/core';
import { Warehouse } from '../../entities/warehouse';
import { ActivatedRoute, Router } from "@angular/router";
import { AccessToWarehouse } from '../../entities/accessToWarehouse';
import { ReceiptAllStatusModel } from '../../entities/receipt-all-status';
import { BaseComponent } from "../../../../core/abstraction/base.component";
import { Mediator } from "../../../../core/services/mediator/mediator.service";
import { IdentityService } from "../../../identity/repositories/identity.service";
import { PagesCommonService } from '../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../shared/services/notification/notification.service';
import { CreateAccessToWarehouseCommand } from '../../repositories/accessToWarehouse/commands/create-access-to-warehouse-command';
import { GetAccessToWarehouseByUserIdQuery } from '../../repositories/accessToWarehouse/queries/get-accessToWarehouse-by-userId';
import { GetWarehouseUsersQuery } from '../../repositories/accessToWarehouse/queries/get-users';
import { SearchQuery } from '../../../../shared/services/search/models/search-query';
import { GetWarehousesQuery } from '../../repositories/warehouse/queries/get-warehouses-query';
import { GetWarehousesLastLevelQuery } from '../../repositories/warehouse/queries/get-warehouses-recursives-query';
import { DeleteAccessToWarehouseCommand } from '../../repositories/accessToWarehouse/commands/delete-access-to-warehouse-command';
import { GetALLCodeVoucherGroupsQuery } from '../../repositories/receipt/queries/receipt/get-code-voucher-groups-query';

@Component({
  selector: 'app-add-access-to-warehouse',
  templateUrl: './add-access-to-warehouse.component.html',
  styleUrls: ['./add-access-to-warehouse.component.scss']
})
export class AddAccessToWarehouseComponent extends BaseComponent {

  ReceiptAllStatus: any[] = [];
  filterReceiptAllStatus: any[] = [];
  WarehouseNodes: any[] = [];
  Warehouses: any[] = [];
  filterWarehouseNodes: any[] = [];
  public users: any[] = [];
  public accessToWarehouses: any[] = [];
  
  
  constructor(
    private router: Router,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);

    this.request = new CreateAccessToWarehouseCommand()

  }

  async ngOnInit() {
    await this.resolve()

  }

  async resolve(params?: any) {
    this.formActions = [];

    this.request = new CreateAccessToWarehouseCommand();
    await this.initialize();
  }


  async initialize(entity?: any) {

    await this._mediator.send(new GetALLCodeVoucherGroupsQuery(0, 0, undefined, "id ASC")).then(res => {

      this.ReceiptAllStatus = res.data;
    });
    await this._mediator.send(new GetWarehousesQuery(0, 0, undefined, "id ASC")).then(res => {

      this.Warehouses = res.data;
    })
    await this._mediator.send(new GetWarehouseUsersQuery(0, 0, undefined)).then(res => {

      this.users = res.data
    })
    await this._mediator.send(new GetWarehousesLastLevelQuery(0, 0, undefined)).then(async (res) => {

      this.WarehouseNodes = res.data
      
    })
  }
  
 
  //----------------ذخیره------------------------------
  async add() {

    this.filterWarehouseNodes.forEach(async a => {
      let req = new CreateAccessToWarehouseCommand();
      req.userId = this.form.controls.userId.value;
      req.description = this.form.controls.description.value;
      req.tableName = this.Service.AccessToWarehouse;
      req.warehouseId = a.id;
      await this._mediator.send(<CreateAccessToWarehouseCommand>req).then(response => {

      });
    })

    this.filterReceiptAllStatus.forEach(async a => {
      let req = new CreateAccessToWarehouseCommand();
      req.userId = this.form.controls.userId.value;
      req.description = this.form.controls.description.value;
      req.tableName =this.Service.AccessToWarehouseCodeGroup ;
      req.warehouseId = a.id;
      await this._mediator.send(<CreateAccessToWarehouseCommand>req).then(response => {

      });
    })
    this._notificationService.showSuccessMessage()

  }

  getAccessToWarehousesList(items: any[]) {

    this.filterWarehouseNodes = items;


  }
  getReceiptAllStatusList(items: AccessToWarehouse[]) {

    this.filterReceiptAllStatus = items;

  }
  
  async UsersSelect(id: any) {
   
    this.form.controls.userId.setValue(id);
    
    await this._mediator.send(new GetAccessToWarehouseByUserIdQuery(this.form.controls.userId.value, this.Service.AccessToWarehouse)).then(res => {
      let $this = this;
      this.filterWarehouseNodes = res.map(function (id, index) {
        return {
          title: $this.findWarehousesTitle(id),
          id: id

        }
      });

    });
    await this._mediator.send(new GetAccessToWarehouseByUserIdQuery(this.form.controls.userId.value, this.Service.AccessToWarehouseCodeGroup)).then(res => {
      let $this = this;
      this.filterReceiptAllStatus = res.map(function (id, index) {
        return {
          title: $this.findReceiptTitle(id),
          id: id
        }
      });

    });
  }

  findWarehousesTitle(id:any) {
    return this.Warehouses.find(a=>a.id==id)?.title
  }
  findReceiptTitle(id: any) {
    return this.ReceiptAllStatus.find(a => a.id == id)?.title
  }
  
  async onDelete(param: any, tableName: string) {
    var id =
      await this._mediator.send(new DeleteAccessToWarehouseCommand(param.id,this.form.controls.userId.value, tableName))
  }

  async delete() {
    
  }
  reset() {

  }

  close(): any {
  }
  async get(Id: number) {

  }

  async update() {

  }
  async edit() {
  }
}

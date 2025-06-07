import { Component } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { PageModes } from "../../../../../core/enums/page-modes";
import { IdentityService } from "../../../../identity/repositories/identity.service";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { Receipt } from '../../../entities/receipt';
import { TableConfigurations} from '../../../../../core/components/custom/table/models/table-configurations';
import { ReceiptItem, addModelArgs } from '../../../entities/receipt-item';
import { GetRecepitPlacementQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-placement-query';
import { Warehouse } from '../../../entities/warehouse';
import { PlacementWarehouseCommand } from '../../../repositories/receipt/commands/reciept/placement-in-warehouse-command';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';


@Component({
  selector: 'app-placement-warehouse-direct-receipt',
  templateUrl: './placement-warehouse-direct-receipt.component.html',
  styleUrls: ['./placement-warehouse-direct-receipt.component.scss']
})
export class PlacementWarehouseDirectReceiptComponent  extends BaseComponent {

  tableConfigurations!: TableConfigurations;
  public receipt: Receipt | undefined = undefined;
  public ReceiptItem: ReceiptItem | undefined = undefined;
  public isSubmitForm: boolean = false;
  public warehouseId: number | undefined = undefined;
  public CommodityId: number | undefined = undefined;
  public warehouseLayoutId: number | undefined = undefined;




  constructor(private route: ActivatedRoute,
    private router: Router,
    private identityService: IdentityService,
    public _notificationService: NotificationService,
    private Service: PagesCommonService,
    private _mediator: Mediator) {
    super(route, router);

  }

  async ngOnInit() {

  }
  async ngAfterViewInit() {
    await this.resolve()
  }
  async resolve(params?: any) {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.save,
      FormActionTypes.list,
    ];
    await this.initialize();
  }
  async initialize(entity?: any) {
    //-----------------------------------
    if (entity || this.getQueryParam('id')) {
      this.receipt = await this.getLayout(this.getQueryParam('id'), this.getQueryParam('warehouseId'))

      this.pageMode = PageModes.Update;
    }


  }
  async onChoseWarhouseLayout(ReceiptItem: ReceiptItem, warehouseLayoutId: number) {


    this.CommodityId = undefined;
    this.ReceiptItem = undefined;
    this.warehouseLayoutId = undefined;
    this.CommodityId = ReceiptItem.commodity?.id;
    this.ReceiptItem = ReceiptItem;
    this.warehouseLayoutId = warehouseLayoutId;


  }

  //-----------------------انتخاب و ذخیره اطلاعات محل انتخاب شده
  onChose(ReceiptItem: ReceiptItem) {

    this.ReceiptItem = ReceiptItem;
    var request = new PlacementWarehouseCommand();
    request.CommodityId = this.ReceiptItem?.commodity?.id;
    request.documentItemId = Number(this.ReceiptItem?.id);
    request.id = this.receipt?.id;
    request.Quantity = Number(this.ReceiptItem?.quantity) - Number(this.ReceiptItem?.quantityUsed);
    request.WarehouseLayoutId = ReceiptItem?.warehouseLayoutQuantity?.warehouseLayoutId;
    request.WarehouseLayoutQuantityId = Number(this.ReceiptItem?.warehouseLayoutQuantity?.id);

    //------------------------اگر جابه جایی بین انبارها باشد
    request.warehouseId = this.receipt?.warehouseId;


    this.update(request);

  }
  //-----------------------انتخاب و ذخیره اطلاعات محل انتخاب شده
  async SelectWarhouseLayout(addModelArgs: addModelArgs) {

    var request = new PlacementWarehouseCommand();
    request.CommodityId = this.ReceiptItem?.commodity?.id;
    request.documentItemId = Number(this.ReceiptItem?.id);
    request.id = this.receipt?.id;
    request.Quantity = Number(addModelArgs.quantity);
    request.WarehouseLayoutId = addModelArgs.warhouseLayoutId;

    //------------------------اگر جابه جایی بین انبارها باشد
    request.warehouseId = this.receipt?.warehouseId;

    this.update(request);

  }
  async update(request: PlacementWarehouseCommand) {

    var response = await this._mediator.send(<PlacementWarehouseCommand>request);
    this.receipt = await this.getLayout(this.getQueryParam('id'), Number(this.receipt?.warehouseId))

    var ReceiptItem = this.receipt?.items?.find(a => a.documentItemId == request.documentItemId)

    if (ReceiptItem != undefined) {
      this.onChoseWarhouseLayout(ReceiptItem, Number(request.WarehouseLayoutId))
    }


  }


  async get(Id: number) {

  }
  async getLayout(Id: number, warehouseId:number) {
    let response = await this._mediator.send(new GetRecepitPlacementQuery(Id, warehouseId))
    this.warehouseId = response?.warehouseId;

    return response
  }
  async WarehouseIdSelect(item: Warehouse) {


    if (this.receipt != undefined && item != undefined) {
      this.receipt.warehouseId = item.id;

      await this.getLayout(this.getQueryParam('id'), item.id).then(res => {
        this.receipt = res;
      });


    }

  }
  async navigateToReceiptList() {

    await this.router.navigateByUrl('/inventory/temporaryReceiptList')
  }


  async add() {

  }

  async edit() {
  }

  async reset() {

  }
  close(): any {
  }

  delete(param?: any): any {
  }






}


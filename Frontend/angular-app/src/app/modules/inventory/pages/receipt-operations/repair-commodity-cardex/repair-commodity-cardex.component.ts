import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { IdentityService } from "../../../../identity/repositories/identity.service";
import { Warehouse } from '../../../entities/warehouse';
import { Receipt } from '../../../entities/receipt';
import { Commodity } from '../../../../commodity/entities/commodity';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { DocumentState } from '../../../entities/documentState';
import { CreateStartDocumentCommand } from '../../../repositories/receipt/commands/reciept/add-start-document-receipt';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { FormControl, FormGroup } from '@angular/forms';
import { UpdateStartDocumentCommand } from '../../../repositories/receipt/commands/reciept/edit-start-document-receipt';
import { GetStartReceiptsItemQuery } from '../../../repositories/receipt/queries/receipt/get-start-receipt-item-query';
import { UserYear } from '../../../../identity/repositories/models/user-year';
import { UpdateWarehouseCardexCommand } from '../../../repositories/receipt/commands/reciept/update-warehouse-cardex';

@Component({
  selector: 'app-repair-commodity-cardex',
  templateUrl: './repair-commodity-cardex.component.html',
  styleUrls: ['./repair-commodity-cardex.component.scss']
})
export class RepairCommodityCardexComponent extends BaseComponent {
    add(param?: any) {
        throw new Error('Method not implemented.');
    }

  documentTags: string[] = [];
  allowedYears: UserYear[] = [];

  public _Receipt: Receipt | undefined = undefined;
  public commodityCategorylevelCode: any = undefined;
  public commodities: Commodity[] = [];
  public pageModeTypeUpdate: boolean = false;
 
  public editForm = new FormGroup({
    commodityId: new FormControl(),
    warehouseId: new FormControl(),
    unitPrice: new FormControl(),
    quantity: new FormControl(),
    yearId: new FormControl(),
  
  });
  public editCordexForm = new FormGroup({
    
    warehouseId: new FormControl(),
    yearId: new FormControl(),

  });
  
  //---------------شمارش تعداد سطرهای کالا -----------------
  public rowCount: number = 1;

  constructor(
    private router: Router,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);


    this.pageModeTypeUpdate = false;
    this.request = new CreateStartDocumentCommand()
    
    identityService._applicationUser.subscribe(res => {
      this.allowedYears = res.years;

        this.form.patchValue({
          yearId: +res.yearId,

        });
        this.editForm.patchValue({
          yearId: +res.yearId,

        });
      this.editCordexForm.patchValue({
        yearId: +res.yearId,

      });
      
      
    });

  }

  async ngOnInit() {
    await this.resolve()

  }

  async resolve(params?: any) {
    this.formActions = [
     
    ];


    this.request = new CreateStartDocumentCommand();



    await this.initialize()


  }


  async initialize(entity?: any) {

    this.form.controls['documentNo'].disable();
  }


  
  async handleYearChange(yearId: number) {
    this.form.patchValue({
      yearId: yearId,

    });
    this.editForm.patchValue({
      yearId: yearId,

    });
  }
  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);


  }
  WarehouseIdEditSelect(item: Warehouse) {

    this.editForm.controls.warehouseId.setValue(item?.id);


  }

  WarehouseIdEditSelectCordex(item: Warehouse) {

    this.editCordexForm.controls.warehouseId.setValue(item?.id);


  }
  ReferenceSelect(item: any) {

    this.form.controls.creditAccountReferenceId.setValue(item?.id);
    this.form.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);

  }

  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.form.controls.codeVoucherGroupId.setValue(item?.id);
    if (item?.id == undefined) {
      this.WarehouseIdSelect(new Warehouse());
    }

  }

  async reset() {
    this.form.controls['warehouseId'].value = undefined;

    return super.reset();
  }
  TagSelect(tagstring: string[]) {
    this.documentTags = tagstring;
  }
 
 async getCommodityById(item: Commodity) {
   this.editForm.controls.commodityId.setValue(item?.id);

   await this._mediator.send(new GetStartReceiptsItemQuery(this.editForm.controls.commodityId.value, this.editForm.controls.warehouseId.value)).then(async (res) => {
     this.editForm.controls.unitPrice.setValue(res.unitPrice);
     this.editForm.controls.quantity.setValue(res.quantity);
    })
    
  }
  async edit() {
    var edit = new UpdateStartDocumentCommand();
    edit.commodityId = this.editForm.controls.commodityId.value;
    edit.warehouseId = this.editForm.controls.warehouseId.value;
    edit.unitPrice = this.editForm.controls.unitPrice.value;
    edit.quantity = this.editForm.controls.quantity.value;
    edit.yearId = this.editForm.controls.yearId.value;
    await this._mediator.send(<UpdateStartDocumentCommand>edit);
  }
  async updateWarehouseCardex () {
    var edit = new UpdateWarehouseCardexCommand();
    edit.warehouseId = this.editCordexForm.controls.warehouseId.value
    edit.yearId = this.editCordexForm.controls.yearId.value;
    await this._mediator.send(<UpdateWarehouseCardexCommand>edit);
  }
  close(): any {
  }

  delete(param?: any): any {
  }

  async get(Id: number) {

  }

  async update() {

  }
  
}

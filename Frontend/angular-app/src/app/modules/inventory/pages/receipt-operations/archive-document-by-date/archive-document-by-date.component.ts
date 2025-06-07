import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { IdentityService } from "../../../../identity/repositories/identity.service";
import { Warehouse } from '../../../entities/warehouse';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { ArchiveDocumentHeadsByDocumentDateCommand } from '../../../repositories/receipt/commands/reciept/archive-documentHeads-by-documentDate';

@Component({
  selector: 'app-archive-document-by-date',
  templateUrl: './archive-document-by-date.component.html',
  styleUrls: ['./archive-document-by-date.component.scss']
})
export class ArchiveDocumentHeadsByDocumentDateComponent extends BaseComponent {
    

  constructor(
    private router: Router,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);


    this.request = new ArchiveDocumentHeadsByDocumentDateCommand()
    
    

  }

  async ngOnInit() {
    await this.resolve()

  }

  async resolve(params?: any) {
    this.formActions = [
     
    ];


    this.request = new ArchiveDocumentHeadsByDocumentDateCommand();



    await this.initialize()


  }


  async initialize(entity?: any) {

  }


  
  async documentStatuesBaseValueChange(documentStatuesBaseValue: number) {
    this.form.patchValue({
      documentStatuesBaseValue: documentStatuesBaseValue,

    });
    
  }
  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);


  }
  

  async reset() {
    this.form.controls['warehouseId'].value = undefined;

    return super.reset();
  }
  
 
  async archive() {

    if (!this.form.controls.warehouseId.value) {
      this.Service.showHttpFailMessage('انبار را انتخاب نمایید')
      return
    } else if (!this.form.controls.fromDate.value) {
      this.Service.showHttpFailMessage('از تاریخ را انتخاب نمایید')
      return
    } else if (!this.form.controls.toDate.value) {
      this.Service.showHttpFailMessage('تا تاریخ را انتخاب نمایید')
      return
    }
    else if (!this.form.controls.documentStatuesBaseValue.value) {
      this.Service.showHttpFailMessage('نوع رسید / حواله را انتخاب نمایید')
      return
    }
    let request = new ArchiveDocumentHeadsByDocumentDateCommand()
        request.warehouseId=this.form.controls.warehouseId.value ,
        request.fromDate=new Date(new Date(<Date>(this.form.controls.fromDate.value)).setHours(0, 0, 0, 0)),
        request.toDate =new Date(new Date(<Date>(this.form.controls.toDate.value)).setHours(24, 0, 0, -1)),
        request.documentStatuesBaseValue =this.form.controls.documentStatuesBaseValue.value
      
    await this._mediator.send(<ArchiveDocumentHeadsByDocumentDateCommand>request);
  }
  
  close(): any {
  }

  delete(param?: any): any {
  }

  async get(Id: number) {

  }

  async update() {

  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
  
}

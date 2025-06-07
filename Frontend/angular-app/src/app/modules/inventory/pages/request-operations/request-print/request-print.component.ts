import { Component, OnInit, EventEmitter, HostListener, Inject } from '@angular/core';
import { ActivatedRoute, Data, Router } from "@angular/router";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { IdentityService } from "../../../../identity/repositories/identity.service";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { Receipt } from '../../../entities/receipt';
import { FormAction } from '../../../../../core/models/form-action';
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { GetTemporaryRecepitQuery } from '../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-query';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { environment } from '../../../../../../environments/environment';
import { LocalStorageRepository } from '../../../../../core/services/storage/local-storage-repository.service';
import { UpdatePrintCountCommand } from '../../../repositories/receipt/commands/reciept/update-print-count-receipt';


@Component({
  selector: 'app-request-print',
  templateUrl: './request-print.component.html',
  styleUrls: ['./request-print.component.scss']
})

export class RequestPrintComponent extends BaseComponent {


  date: Date = new Date();
  public pageTitle:string=''
  public receipt: Receipt | undefined = undefined;
  DivId: string = '';


  @HostListener("window:beforeunload", [])
  async onWindowAfterPrint() {

  }


  formActions: FormAction[] = [
    FormActionTypes.refresh,

  ]
  constructor(
    private  router: Router,
    private  _mediator: Mediator,
    private  route: ActivatedRoute,
    public   Service: PagesCommonService,
    private  cookieService: LocalStorageRepository,
    private  identityService: IdentityService,
    public  _notificationService: NotificationService,
  ) {
    super(route, router);
  }


  async ngOnInit() {
    await this.resolve()

  }


  async resolve(params?: any) {

    await this.initialize();
  }
  async initialize(entity?: any) {
    //-----------------------------------
    if (this.getQueryParam('id') != undefined) {
      this.receipt = await this.get(this.getQueryParam('id'))
      this.DivId = "div" + this.getQueryParam('id');

    }
    if (this.getQueryParam('displayPage') != undefined) {
      this.pageTitle = this.getQueryParam('displayPage')=='buy'?'درخواست خرید کالا':'درخواست دریافت کالا'
    }


  }
  async myPrint(cmpName: string = 'mainTable') {
    let printContents = document.getElementById(cmpName)?.innerHTML;
    let callBackMethod = await this.update_Status();
    this.Service.onPrint(printContents?.toString(), '', callBackMethod);
  }
  //----------------------تغییر وضعیت به پرینت شده----------------------------
  async update_Status() {

    var uri = environment.apiURL
    var request_ = new UpdatePrintCountCommand();
    request_.id = this.getQueryParam('id');



    uri = uri + request_.url;
    var htmlData = '';

    htmlData = `
    const item = {
      id:${request_.id}

    };
    fetch('${uri}', {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ${this.cookieService.get('token')}'
    },
    body: JSON.stringify(item)
    })
    .then(response => response.json())
    .catch(error => alert(error))`;

    return htmlData
  }
  async get(Id: number) {
    let response = await this._mediator.send(new GetTemporaryRecepitQuery(Id))


    return response
  }

  async navigateToReceiptList() {

    await this.router.navigateByUrl('/inventory/request-list/requesBuyList')
  }

  add(param?: any) {
    throw new Error('Method not implemented.');
  }
  update(param?: any) {
    throw new Error('Method not implemented.');
  }
  delete(param?: any) {
    throw new Error('Method not implemented.');
  }
  close() {
    throw new Error('Method not implemented.');
  }

}
